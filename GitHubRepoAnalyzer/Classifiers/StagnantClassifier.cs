using GitHubRepoAnalyzer.Enums;
using GitHubRepoAnalyzer.Interfaces;
using GitHubRepoAnalyzer.Models;

namespace GitHubRepoAnalyzer.Classifiers
{
    public class StagnantClassifier : IStatusClassifier
    {
        public RepositoryStatus? Classify(ExtendedRepositoryData data)
        {
            if ((data.LastIssueActivityDate.HasValue && data.LastIssueActivityDate.Value > DateTime.UtcNow.AddMonths(-6) &&
                data.LastCommitDate < DateTime.UtcNow.AddMonths(-6)))
            {
                return RepositoryStatus.Stagnant;
            }

            return null!;
        }
    }
}
