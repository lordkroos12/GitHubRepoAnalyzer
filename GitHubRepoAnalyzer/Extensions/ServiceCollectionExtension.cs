using GitHubRepoAnalyzer.Helper;
using GitHubRepoAnalyzer.Interfaces;
using GitHubRepoAnalyzer.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GitHubRepoAnalyzer.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGitHubAnalyzerServices(this IServiceCollection services, IConfiguration configuration)
        {
            var token = TokenProvider.GetToken();

            services.AddTransient<IStatusService, StatusService>();
            services.AddTransient<IPrintHelper, PrintHelper>();
            services.AddTransient<IApiService>(provider =>
            {
                var statusServices = provider.GetRequiredService<IStatusService>();
                return new ApiService(statusServices, token);
            });

            services.AddStatusClassifiers();

            return services;
        }

        public static IServiceCollection AddStatusClassifiers(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var classifiers = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IStatusClassifier).IsAssignableFrom(type));

            foreach (var classifier in classifiers)
            {
                services.AddTransient(typeof(IStatusClassifier), classifier);
            }

            return services;
        }
    }
}
