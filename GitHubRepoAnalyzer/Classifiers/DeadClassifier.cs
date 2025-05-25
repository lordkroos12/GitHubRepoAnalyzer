using GitHubRepoAnalyzer.Interfaces;
using GitHubRepoAnalyzer.Models;

namespace GitHubRepoAnalyzer.Classifiers
{
    public class DeadClassifier : IStatusClassifier
    {
        public string? Classify(ExtendedRepositoryData data)
        {
            if (data.BasicData.IsArchived ||
           (!data.LastCommitDate.HasValue || data.LastCommitDate <= DateTime.UtcNow.AddYears(-1)) &&
           (!data.LastActivityByMaintainer.HasValue || data.LastActivityByMaintainer <= DateTime.UtcNow.AddYears(-1)))
            {
                return "Dead";
            }

            return null!;
        }
    }
}
