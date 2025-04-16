using Microsoft.EntityFrameworkCore;
using UserManagementAPI.DAL.Models;

namespace UserManagementAPI.DAL.Context
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.Migrate();
        }

    }
}