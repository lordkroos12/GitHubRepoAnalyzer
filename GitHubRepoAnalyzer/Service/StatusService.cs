using GitHubRepoAnalyzer.Classifiers;
using GitHubRepoAnalyzer.Enums;
using GitHubRepoAnalyzer.Interfaces;
using GitHubRepoAnalyzer.Models;

namespace GitHubRepoAnalyzer.Service
{
    public class StatusService : IStatusService
    {
        private readonly List<IStatusClassifier> _classifiers;
        public StatusService()
        {
            _classifiers = new List<IStatusClassifier>
            {
                new LiveClassifier(),
                new StagnantClassifier(),
                new DeadClassifier(),
            };
        }
        public RepositoryStatus? GetStatus(ExtendedRepositoryData data)
        {
            foreach (var classifier in _classifiers)
            {
                var status = classifier.Classify(data);
                if (status != null)
                {
                    return status;
                }
            }
            return null;
        }
    }
}
