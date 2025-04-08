using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        
    }
}
