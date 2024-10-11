using HRmanagement.Models;
using Microsoft.EntityFrameworkCore;
using HRmanagement.Data.enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HRmanagement.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Designation> Designations { get; set; }
        public DbSet<EmployeeUser> Emploees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Designation>().HasData(
                new Designation
                {
                    Id = 1,
                    Name = "None",
                    Level = EmployeeLevel.Junior,
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
                },
                new Designation
                {
                    Id = 4,
                    Name = "Senior DotNet Developer",
                    Level = EmployeeLevel.Senior
                }

                );
        }
    }
}