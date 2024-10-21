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
        public DbSet<EmployeeUser> Employees { get; set; }
        public DbSet<TimeStorage> Timings { get; set; }
        public DbSet<Salary> salaries { get; set; }
        public DbSet<TaskGiven> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaskGiven>()
            .HasOne(t => t.Creator)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TaskGiven>()
                .HasOne(t => t.Assignee)
                .WithMany()
                .HasForeignKey(t => t.AssignedTO)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<EmployeeUser>()
                .HasOne(e => e.designation)
                .WithMany()
                .HasForeignKey(d => d.DesignationId)
                .OnDelete(DeleteBehavior.NoAction);

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
                },
                new Designation
                {
                    Id = 5,
                    Name = "Senior Account",
                    Level = EmployeeLevel.Senior
                },
                new Designation
                {
                    Id = 6,
                    Name = "Project Manager",
                    Level = EmployeeLevel.Senior
                }

                );
        }
    }
}