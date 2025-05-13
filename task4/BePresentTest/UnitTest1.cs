using BePresent.Controllers;
using BePresent.Domain.Users;
using BePresent.Infrastructure.AppData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.EntityFrameworkCore.Query;

public class BoardsControllerTests
{
    private Mock<DbSet<GiftBoard>> GetMockDbSet(List<GiftBoard> data)
    {
        var queryable = data.AsQueryable();

        var mockSet = new Mock<DbSet<GiftBoard>>();
        mockSet.As<IQueryable<GiftBoard>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<GiftBoard>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<GiftBoard>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<GiftBoard>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        mockSet.As<IAsyncEnumerable<GiftBoard>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<GiftBoard>(queryable.GetEnumerator()));

        mockSet.As<IQueryable<GiftBoard>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<GiftBoard>(queryable.Provider));

        return mockSet;
    }

    [Fact]
    public async Task Index_ReturnsViewResultWithBoards()
    {
        // Arrange
        var boards = new List<GiftBoard>
        {
            new GiftBoard { Name = "Board1" },
            new GiftBoard { Name = "Board2" }
        };

        var mockSet = GetMockDbSet(boards);

        var mockContext = new Mock<AppDbContext>();
        mockContext.Setup(c => c.GiftBoards).Returns(mockSet.Object);

        var controller = new BoardsController(mockContext.Object);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<GiftBoard>>(viewResult.Model);
        Assert.Equal(2, model.Count);
    }

    [Fact]
    public void Create_Get_ReturnsView()
    {
        var mockContext = new Mock<AppDbContext>();
        var controller = new BoardsController(mockContext.Object);

        var result = controller.Create();

        Assert.IsType<ViewResult>(result);
    }
}

// Допоміжні класи для підтримки async LINQ
internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;

    public T Current => _inner.Current;

    public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(_inner.MoveNext());

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }
}

internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

    public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression);

    public object Execute(Expression expression) => _inner.Execute(expression);

    public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        => Task.FromResult(Execute<TResult>(expression)).Result;
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

    public TestAsyncEnumerable(Expression expression) : base(expression) { }
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
        new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}