using GitHubRepoAnalyzer.Enums;
using GitHubRepoAnalyzer.Models;

namespace GitHubRepoAnalyzer.Interfaces
{
    public interface IStatusService
    {
        RepositoryStatus? GetStatus(ExtendedRepositoryData data);
    }
}
