using GitHubRepoAnalyzer.Extensions;
using GitHubRepoAnalyzer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GitHubRepoAnalyzer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var printHelper = host.Services.GetRequiredService<IPrintHelper>();
            await printHelper.PrintMenuAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddGitHubAnalyzerServices(context.Configuration);
                });
        }
    }
}
