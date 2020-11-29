using FGrid.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace FGrid.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TestUser> Users { get; set; }
    }
}