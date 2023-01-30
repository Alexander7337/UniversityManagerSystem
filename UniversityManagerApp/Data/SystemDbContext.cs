using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityManagerApp.Models;

namespace UniversityManagerApp.Data
{
    public class SystemDbContext : IdentityDbContext<Student, IdentityRole, string>
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<CourseStudent> CourseStudents { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseStudent>()
            .HasKey(bc => new { bc.CourseID, bc.StudentID });

            modelBuilder.Entity<CourseStudent>()
                .HasOne(c => c.Course)
                .WithMany(s => s.CourseStudents)
                .HasForeignKey(c => c.CourseID);

            modelBuilder.Entity<CourseStudent>()
                .HasOne(s => s.Student)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(s => s.StudentID);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
