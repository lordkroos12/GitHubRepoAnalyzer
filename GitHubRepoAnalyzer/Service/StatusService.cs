using GitHubRepoAnalyzer.Interfaces;
using GitHubRepoAnalyzer.Models;

namespace GitHubRepoAnalyzer.Service
{
    public class StatusService : IStatusService
    {
        private readonly List<IStatusClassifier> _classifiers;
        public StatusService(IEnumerable<IStatusClassifier> classifiers)
        {
            _classifiers = classifiers.ToList();
        }
        public string? GetStatus(ExtendedRepositoryData data)
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
