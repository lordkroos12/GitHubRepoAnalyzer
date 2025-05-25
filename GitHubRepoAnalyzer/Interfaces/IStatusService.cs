using GitHubRepoAnalyzer.Models;

namespace GitHubRepoAnalyzer.Interfaces
{
    public interface IStatusService
    {
        string? GetStatus(ExtendedRepositoryData data);
    }
}
