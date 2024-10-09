using HRmanagement.Models;
using Microsoft.EntityFrameworkCore;
using HRmanagement.Data.enums;

namespace HRmanagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Designation> Designations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Designation>().HasData(
                new Designation
                {
                    Id = 1,
                    Name = "Senier Dot Net Developer",
                    Level = EmployeeLevel.Senior
                },
                new Designation
                {
                    Id = 2,
                    Name = "Mid level Frontend Developer",
                    Level = EmployeeLevel.Mid
                },
                new Designation
                {
                    Id = 3,
                    Name = "Junior UI/UX Designer",
                    Level = EmployeeLevel.Junior
                }
                );
        }
    }
}