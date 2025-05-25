using GitHubRepoAnalyzer.Interfaces;

namespace GitHubRepoAnalyzer.Helper
{
    internal class PrintHelper : IPrintHelper
    {
        private readonly IApiService _apiService;

        public PrintHelper(IApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task PrintMenuAsync()
        {
            var (owner, repo) = GetRepositoryLink();
            while (true)
            {
                Console.WriteLine("\nWhat do you want to fetch?");
                Console.WriteLine("1. Basic info");
                Console.WriteLine("2. Pull Requests");
                Console.WriteLine("3. Contributors");
                Console.WriteLine("4. Commits (last 7 days)");
                Console.WriteLine("5. Languages");
                Console.WriteLine("6. Average time to close issue");
                Console.WriteLine("7. Full Report");
                Console.WriteLine("8. Change repository");
                Console.WriteLine("0. Exit");
                Console.Write("\nEnter your choice: ");
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            var basicData = await _apiService.GetInfoForRepoAsync(owner, repo);
                            Console.WriteLine(basicData.ToString());
                            break;
                        case "2":
                            var prs = await _apiService.GetPullRequestsAsync(owner, repo);
                            Console.WriteLine("Pull request:");
                            Console.WriteLine(prs.ToString());
                            break;
                        case "3":
                            var contributors = await _apiService.GetTopContributorsAsync(owner, repo);
                            Console.WriteLine("Top 5 Contributors:");
                            foreach (var contributor in contributors)
                            {
                                Console.WriteLine(contributor);
                            }
                            break;
                        case "4":
                            var commits = await _apiService.GetCommitsAsync(owner, repo);
                            Console.WriteLine("Commits in the last 7 days: ");
                            foreach (var commit in commits)
                            {
                                Console.WriteLine(commit);
                            }
                            break;
                        case "5":
                            var languages = await _apiService.GetLanguagesAsync(owner, repo);
                            Console.WriteLine("Languages:");
                            foreach (var item in languages)
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        case "6":
                            var issue = await _apiService.GetIssuesAsync(owner, repo);
                            Console.WriteLine("Issues:");
                            Console.WriteLine(issue.ToString());
                            break;
                        case "7":
                            var fullReport = await _apiService.GetFullReportAsync(owner, repo);
                            Console.WriteLine(fullReport.ToString());
                            break;
                        case "8":
                            (owner, repo) = GetRepositoryLink();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please select a valid option.\n");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private (string owner, string repo) GetRepositoryLink()
        {
            while (true)
            {
                Console.WriteLine("Enter GitHub repository as a link:");
                var link = Console.ReadLine();

                try
                {
                    return GetOwnerAndRepo(link);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }

        private (string owner, string repo) GetOwnerAndRepo(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !url.StartsWith("https://github.com/"))
            {
                throw new ArgumentException("URL must start with https://github.com/ \n");
            }

            var parts = url.Replace("https://github.com/", "").Split('/');

            if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
            {
                throw new ArgumentException("URL must contain both owner and repository name.\n");
            }

            return (parts[0], parts[1]);
        }
    }
}

