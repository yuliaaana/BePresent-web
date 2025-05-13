using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using BePresent.Domain.Users;

namespace BePresent.Infrastructure.AppData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<GiftBoard> GiftBoards { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<GiftReservation> GiftReservations { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }
        public DbSet<EmailConfirmation> EmailConfirmations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конвертер для List<string> -> JSON string
            var interestsConverter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
            );

            modelBuilder.Entity<User>()
                .Property(u => u.Interests)
                .HasColumnType("text[]");
            modelBuilder.Entity<Gift>()
                .HasOne(g => g.ReservedUser)
                .WithMany()
                .HasForeignKey(g => g.ReservedBy)
                .IsRequired(false);
            base.OnModelCreating(modelBuilder);
        }

    }
}

