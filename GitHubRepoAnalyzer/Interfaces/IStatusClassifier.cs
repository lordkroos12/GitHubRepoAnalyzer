using GitHubRepoAnalyzer.Enums;
using GitHubRepoAnalyzer.Models;

namespace GitHubRepoAnalyzer.Interfaces
{
    public interface IStatusClassifier
    {
        RepositoryStatus? Classify(ExtendedRepositoryData data);
    }
}
