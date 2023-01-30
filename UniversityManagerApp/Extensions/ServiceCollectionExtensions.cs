using Microsoft.AspNetCore.Identity;
using UniversityManagerApp.Data;
using UniversityManagerApp.Models;
using UniversityManagerApp.Services;

namespace UniversityManagerApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddIdentityCore<Student>(options =>
                    {
                                 options.Password.RequireDigit = false;
                                 options.Password.RequiredLength = 6;
                                 options.Password.RequireNonAlphanumeric = false;
                                 options.Password.RequireUppercase = false;
                                 options.Password.RequireLowercase = false;
                                 options.SignIn.RequireConfirmedAccount = false;
                    })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<SystemDbContext>()
                    .AddSignInManager<SignInManager<Student>>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication()
                    .AddCookie(IdentityConstants.ApplicationScheme, options =>
                    {
                        options.Cookie.HttpOnly = true;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                        options.LoginPath = "/Home/Login";
                        options.LogoutPath = "/Home/Logout";
                        options.SlidingExpiration = true;
                    });

            services.AddTransient<ICourseService, CourseService>();

            services.AddControllersWithViews();

            return services;
        }
    }
}
