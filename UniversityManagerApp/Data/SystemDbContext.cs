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

            var courseEnrolled = new Course { CourseID = 1, CourseName = "Financial and Stock Markets" };

            modelBuilder.Entity<Course>().HasData(
                new Course { CourseID = courseEnrolled.CourseID, CourseName = courseEnrolled.CourseName },
                new Course { CourseID = 2, CourseName = "Corporate Management"},
                new Course { CourseID = 3, CourseName = "Company's Payments and Transactions"});

            string ADMIN_ID = Guid.NewGuid().ToString();
            string ROLE_ID = Guid.NewGuid().ToString();
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ROLE_ID,
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            PasswordHasher<Student> passwordHasher = new PasswordHasher<Student>();

            var user = new Student
            {
                Id = ADMIN_ID,
                Email = "test@email.com",
                NormalizedEmail = "TEST@EMAIL.COM",
                UserName = "test_user",
                NormalizedUserName = "TEST_USER",
                EmailConfirmed = true,
                PasswordHash = passwordHasher.HashPassword(null, "123456"), 
                SecurityStamp = string.Empty
            };

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

            modelBuilder.Entity<Student>().HasData(user);

            modelBuilder.Entity<CourseStudent>()
                .HasData(new CourseStudent { CourseID = courseEnrolled.CourseID, StudentID = user.Id });

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
