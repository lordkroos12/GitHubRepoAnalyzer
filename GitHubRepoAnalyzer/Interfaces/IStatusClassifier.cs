using GitHubRepoAnalyzer.Models;

namespace GitHubRepoAnalyzer.Interfaces
{
    public interface IStatusClassifier
    {
        string? Classify(ExtendedRepositoryData data);
    }
}
