using Microsoft.EntityFrameworkCore;
using UniversityManagerApp.Models;

namespace UniversityManagerApp.Data
{
    public class UniSysDbContext /*: DbContext*/
    {
        //public UniSysDbContext(DbContextOptions<UniSysDbContext> options)
        //    : base(options)
        //{
        //}

        //public virtual DbSet<Student> Students { get; set; }

        //public virtual DbSet<Course> Courses { get; set; }

        //protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        //{
        //    base.ConfigureConventions(configurationBuilder);
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .Build();

        //    var connectionString = configuration.GetConnectionString("DefaultConnection");
        //    optionsBuilder.UseSqlServer(connectionString);
        //}
    }
}
