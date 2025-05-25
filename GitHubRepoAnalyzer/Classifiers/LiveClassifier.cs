using GitHubRepoAnalyzer.Interfaces;
using GitHubRepoAnalyzer.Models;

namespace GitHubRepoAnalyzer.Classifiers
{
    public class LiveClassifier : IStatusClassifier
    {
        public string? Classify(ExtendedRepositoryData data)
        {
            if (!data.BasicData.IsArchived &&
            data.LastCommitDate.HasValue && data.LastCommitDate > DateTime.UtcNow.AddMonths(-6) &&
            data.LastActivityByMaintainer.HasValue && data.LastActivityByMaintainer > DateTime.UtcNow.AddMonths(-3))
            {
                return "Live";
            }

            return null!;
        }
    }
}
