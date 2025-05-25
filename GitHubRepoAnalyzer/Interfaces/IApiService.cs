using GitHubRepoAnalyzer.Models;

namespace GitHubRepoAnalyzer.Interfaces
{
    public interface IApiService
    {
        Task<ExtendedRepositoryData> GetFullReportAsync(string user, string repoName);
        Task<GithubRepo> GetInfoForRepoAsync(string user, string repoName);
        Task<List<ContributorData>> GetTopContributorsAsync(string user, string repoName);
        Task<List<CommitData>> GetCommitsAsync(string user, string repoName);
        Task<List<LanguageData>> GetLanguagesAsync(string user, string repoName);
        Task<PullRequestData> GetPullRequestsAsync(string user, string repoName);
        Task<IssueData> GetIssuesAsync(string user, string repoName);
        Task<DateTime?> GetLatestMaintainerResponseWithinAYearAsync(string owner, string repo);
    }
}
