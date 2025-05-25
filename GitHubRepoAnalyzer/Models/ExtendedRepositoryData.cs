using System.Text;

namespace GitHubRepoAnalyzer.Models
{
    public class ExtendedRepositoryData
    {
        public GithubRepo BasicData { get; set; }
        public List<ContributorData> Contributors { get; set; }
        public List<CommitData> Commits { get; set; }
        public List<LanguageData> Languages { get; set; }
        public PullRequestData PullRequests { get; set; }
        public IssueData Issues { get; set; }
        public DateTime? LastCommitDate { get; set; }
        public DateTime? LastIssueActivityDate { get; set; }
        public DateTime? LastActivityByMaintainer { get; set; }
        public string? Status { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(BasicData.ToString());
            sb.AppendLine("Top 5 Contributors:");
            if (Contributors.Count > 0)
            {
                foreach (var contributor in Contributors)
                {
                    sb.AppendLine(contributor.ToString());
                }
            }
            else
            {
                sb.AppendLine("No contributors found.");
                sb.AppendLine();
            }
            sb.AppendLine("Commits in the last 7 days:");
            if (Commits.Count > 0)
            {
                foreach (var commit in Commits)
                {
                    sb.AppendLine(commit.ToString());
                }
            }
            else
            {
                sb.AppendLine("No commits found.");
                sb.AppendLine();
            }
            sb.AppendLine("Languages:");
            if (Languages.Count > 0)
            {
                foreach (var language in Languages)
                {
                    sb.AppendLine(language.ToString());
                }
            }
            else
            {
                sb.AppendLine("No languages found.");
                sb.AppendLine();
            }
            sb.AppendLine("Pull Requests:");
            sb.AppendLine(PullRequests.ToString());
            sb.AppendLine("Issues:");
            sb.AppendLine(Issues.ToString());
            sb.AppendLine("Status");
            if (Status == null) sb.AppendLine("-");
            else sb.AppendLine(Status.ToString());
            return sb.ToString();
        }
    }
}
