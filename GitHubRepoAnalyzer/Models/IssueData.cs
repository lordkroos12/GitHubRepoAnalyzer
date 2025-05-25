namespace GitHubRepoAnalyzer.Models
{
    public class IssueData
    {
        public double AverageCloseTime { get; set; }

        public override string ToString()
        {
            return $"Average Close Time: {AverageCloseTime:F2} days\n";
        }
    }
}
