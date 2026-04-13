using contact.Migrations;
using contact.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace contact.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> user { get; set; }
        public DbSet<Contact> contact { get; set; }
    }
}
