using GitHubRepoAnalyzer.Interfaces;
using GitHubRepoAnalyzer.Models;
using Octokit;

namespace GitHubRepoAnalyzer.Service
{
    public class ApiService : IApiService
    {
        private readonly IGitHubClient _gitHubClient;
        private readonly IStatusService _statusService;

        public ApiService(string token)
        {
            _statusService = new StatusService();
            _gitHubClient = new GitHubClient(new ProductHeaderValue("GitHubAnalyzer"));

            if (!string.IsNullOrEmpty(token))
            {
                _gitHubClient.Connection.Credentials = new Credentials(token);
            }
        }

        public async Task<ExtendedRepositoryData> GetFullReportAsync(string user, string repoName)
        {
            var basicDataTask = GetInfoForRepoAsync(user, repoName);
            var contributorsTask = GetTopContributorsAsync(user, repoName);
            var commitsTask = GetCommitsAsync(user, repoName);
            var languagesTask = GetLanguagesAsync(user, repoName);
            var pullRequestsTask = GetPullRequestsAsync(user, repoName);
            var issuesTask = GetIssuesAsync(user, repoName);
            var lastCommitDateTask = GetLastCommitDateAsync(user, repoName);
            var lastIssueActivityDateTask = GetLastIssueActivityDateAsync(user, repoName);
            var lastActivityByMaintainerTask = GetLatestMaintainerResponseWithinAYearAsync(user, repoName);

            await Task.WhenAll(
                basicDataTask,
                contributorsTask,
                pullRequestsTask,
                commitsTask,
                issuesTask,
                languagesTask,
                lastCommitDateTask,
                lastIssueActivityDateTask,
                lastActivityByMaintainerTask
            );

            var extendedRepo = new ExtendedRepositoryData
            {
                BasicData = await basicDataTask,
                Contributors = await contributorsTask,
                Commits = await commitsTask,
                Languages = await languagesTask,
                PullRequests = await pullRequestsTask,
                Issues = await issuesTask,
                LastCommitDate = await lastCommitDateTask,
                LastIssueActivityDate = await lastIssueActivityDateTask,
                LastActivityByMaintainer = await lastActivityByMaintainerTask
            };
            extendedRepo.Status = _statusService.GetStatus(extendedRepo);
            return extendedRepo;
        }

        public async Task<GithubRepo> GetInfoForRepoAsync(string user, string repoName)
        {
            var repo = await _gitHubClient.Repository.Get(user, repoName);
            var githubRepo = new GithubRepo
            {
                Name = repo.Name,
                Description = repo.Description,
                Stars = repo.StargazersCount,
                OpenIssues = repo.OpenIssuesCount,
                ForksCount = repo.ForksCount,
                IsArchived = repo.Archived
            };
            return githubRepo;
        }

        public async Task<List<ContributorData>> GetTopContributorsAsync(string user, string repoName)
        {
            var options = new ApiOptions
            {
                PageSize = 5,
                PageCount = 1,
                StartPage = 1
            };
            var contributors = await _gitHubClient.Repository.GetAllContributors(user, repoName, options);

            return contributors?
                .Select(c => new ContributorData
                {
                    Login = c.Login,
                    Commits = c.Contributions
                }).ToList() ?? new List<ContributorData>();
        }

        public async Task<List<CommitData>> GetCommitsAsync(string user, string repoName)
        {
            var commits = await _gitHubClient.Repository.Commit.GetAll(user, repoName, new CommitRequest
            {
                Since = DateTimeOffset.UtcNow.AddDays(-7)
            });
            return commits.Select(
                c => new CommitData
                {
                    Message = c.Commit.Message,
                    Author = c.Commit.Author.Name,
                    CommitDate = c.Commit.Author.Date.UtcDateTime
                }).ToList();
        }

        public async Task<List<LanguageData>> GetLanguagesAsync(string user, string repoName)
        {
            var languages = await _gitHubClient.Repository.GetAllLanguages(user, repoName);
            double totalSize = languages.Sum(l => l.NumberOfBytes);

            var languageStats = languages
                .Select(l => new LanguageData
                {
                    Name = l.Name,
                    Percentage = (l.NumberOfBytes / totalSize) * 100
                })
                .ToList();

            var mainLanguages = languageStats
                .Where(l => l.Percentage >= 0.1)
                .ToList();

            double otherLanguages = languageStats
                .Where(l => l.Percentage < 0.1)
                .Sum(l => l.Percentage);

            if (otherLanguages > 0)
            {
                mainLanguages.Add(new LanguageData
                {
                    Name = "Others",
                    Percentage = Math.Max(0.1, otherLanguages)
                });
            }

            return mainLanguages
                .OrderByDescending(l => l.Name != "Others")
                .ThenByDescending(l => l.Percentage)
                .ToList();
        }

        public async Task<PullRequestData> GetPullRequestsAsync(string user, string repoName)
        {
            var openTask = _gitHubClient.Search.SearchIssues(new SearchIssuesRequest($"repo:{user}/{repoName} type:pr state:open") { PerPage = 1 });
            var closedTask = _gitHubClient.Search.SearchIssues(new SearchIssuesRequest($"repo:{user}/{repoName} type:pr state:closed") { PerPage = 1 });
            var mergedTask = _gitHubClient.Search.SearchIssues(new SearchIssuesRequest($"repo:{user}/{repoName} type:pr is:merged") { PerPage = 1 });

            await Task.WhenAll(openTask, closedTask, mergedTask);

            var openCount = (await openTask).TotalCount;
            var closedCount = (await closedTask).TotalCount;
            var mergedCount = (await mergedTask).TotalCount;

            return new PullRequestData
            {
                Open = openCount,
                Closed = closedCount,
                Merged = mergedCount
            };
        }

        public async Task<IssueData> GetIssuesAsync(string user, string repoName)
        {
            var options = new ApiOptions
            {
                PageSize = 100,
                PageCount = 5,
            };

            var issues = await _gitHubClient.Issue.GetAllForRepository(user, repoName, new RepositoryIssueRequest
            {
                State = ItemStateFilter.Closed,
                SortProperty = IssueSort.Created,
                SortDirection = SortDirection.Descending,

            }, options);

            return new IssueData
            {
                AverageCloseTime = issues.Count > 0 ?
                issues
                .Where(i => i.PullRequest == null)
                .Where(i => i.State == ItemState.Closed)
                .Average(i => (i.ClosedAt.Value - i.CreatedAt).Days) : 0
            };
        }

        public async Task<DateTime?> GetLatestMaintainerResponseWithinAYearAsync(string user, string repoName)
        {
            var oneYearAgo = DateTime.UtcNow.AddYears(-1).ToString("yyyy-MM-dd");
            var maintainers = await GetLikelyMaintainersAsync(user, repoName);

            DateTime? latestResponse = null;

            foreach (var maintainer in maintainers)
            {
                var searchQuery = $"repo:{user}/{repoName} involves:{maintainer} updated:>={oneYearAgo}";

                var request = new SearchIssuesRequest(searchQuery)
                {
                    PerPage = 1,
                    SortField = IssueSearchSort.Updated,
                    Order = SortDirection.Descending
                };
                try
                {
                    var result = await _gitHubClient.Search.SearchIssues(request);
                    var issue = result.Items.FirstOrDefault();

                    if (issue != null)
                    {
                        var timeline = await _gitHubClient.Issue.Timeline.GetAllForIssue(user, repoName, issue.Number);

                        var latestMaintainerActivity = timeline
                            .Where(e => e.Actor != null &&
                                       e.Actor.Login.Equals(maintainer, StringComparison.OrdinalIgnoreCase))
                            .Where(e => e.CreatedAt.UtcDateTime >= DateTime.UtcNow.AddYears(-1))
                            .OrderByDescending(e => e.CreatedAt)
                            .FirstOrDefault();

                        if (latestMaintainerActivity?.CreatedAt != null)
                        {
                            var activityDate = latestMaintainerActivity.CreatedAt.UtcDateTime;
                            if (latestResponse == null || activityDate > latestResponse)
                            {
                                latestResponse = activityDate;
                            }
                        }
                    }
                }
                catch (Exception) { }
            }
            return latestResponse;
        }
        private async Task<DateTime?> GetLastCommitDateAsync(string user, string repoName)
        {
            var options = new ApiOptions
            {
                PageSize = 1,
                PageCount = 1,
                StartPage = 1
            };
            var commits = await _gitHubClient.Repository.Commit.GetAll(user, repoName, new CommitRequest(), options);
            return commits.FirstOrDefault()?.Commit.Author.Date.UtcDateTime;
        }

        private async Task<DateTime?> GetLastIssueActivityDateAsync(string user, string repoName)
        {
            var options = new ApiOptions
            {
                PageSize = 1,
                PageCount = 1,
                StartPage = 1
            };

            var issues = await _gitHubClient.Issue.GetAllForRepository(user, repoName, new RepositoryIssueRequest
            {
                State = ItemStateFilter.All,
                SortProperty = IssueSort.Updated,
                SortDirection = SortDirection.Descending,
            },
            options);

            return issues.FirstOrDefault()?.UpdatedAt?.UtcDateTime;
        }

        private async Task<HashSet<string>> GetLikelyMaintainersAsync(string user, string repoName)
        {
            var options = new ApiOptions
            {
                PageSize = 10,
                PageCount = 1,
                StartPage = 1
            };

            var maintainers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            maintainers.Add(user);

            var closedIssues = await _gitHubClient.Issue.GetAllForRepository(user, repoName, new RepositoryIssueRequest
            {
                State = ItemStateFilter.Closed
            }, options);

            foreach (var issue in closedIssues)
            {
                if (issue.ClosedBy != null)
                {
                    maintainers.Add(issue.ClosedBy.Login);
                }
            }

            var closedPrs = await _gitHubClient.PullRequest.GetAllForRepository(user, repoName, new PullRequestRequest
            {
                State = ItemStateFilter.Closed
            }, options);

            foreach (var pr in closedPrs.Where(p => p.Merged == true))
            {
                if (pr.MergedBy != null)
                {
                    maintainers.Add(pr.MergedBy.Login);
                }
            }

            return maintainers;
        }
    }
}

