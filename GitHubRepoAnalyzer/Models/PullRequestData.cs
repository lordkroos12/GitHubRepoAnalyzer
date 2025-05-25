namespace GitHubRepoAnalyzer.Models
{
    public class PullRequestData
    {
        public int? Open { get; set; }
        public int? Closed { get; set; }
        public int? Merged { get; set; }

        public override string ToString()
        {
            return $"Open: {Open ?? '-'} \nClosed: {Closed ?? '-'} \nMerged: {Merged ?? '-'}\n";
        }
    }
}
