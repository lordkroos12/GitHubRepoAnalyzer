using Microsoft.Extensions.Configuration;

namespace GitHubRepoAnalyzer.Helper
{
    public static class TokenProvider
    {
        public static string GetToken()
        {
            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            if (!string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("*Using GitHub token from environment variable*");
                return token;
            }

            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            token = config["GitHubToken"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("*Using GitHub token from User secrets*");
                return token;
            }

            Console.WriteLine("Note: You can use the app without GitHub token (just leave empty) but the API rate limit is much lower");
            Console.WriteLine("Enter your GitHub token:");

            token = Console.ReadLine();
            return token;
        }
    }
}
