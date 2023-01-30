using UniversityManagerApp.Services;

namespace UniversityManagerApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<ICourseService, CourseService>();

            return services;
        }
    }
}
