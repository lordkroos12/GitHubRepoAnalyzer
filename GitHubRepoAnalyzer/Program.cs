using GitHubRepoAnalyzer.Helper;
using GitHubRepoAnalyzer.Service;

namespace GitHubRepoAnalyzer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var apiService = new ApiService(TokenProvider.GetToken());
            var menu = new PrintHelper(apiService);
            await menu.PrintMenuAsync();
        }
    }
}
