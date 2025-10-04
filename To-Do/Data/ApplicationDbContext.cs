using Microsoft.EntityFrameworkCore;
using To_Do.Models;

namespace To_Do.Data
{
    public class ApplicationDbContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Database=New TO-DO; Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(U => U.Email).IsUnique();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<OwnToDoList> OwnToDoLists { get; set; }
        public DbSet<AllToDoItem> AllToDoItems { get; set; }
    }
}
