namespace GitHubRepoAnalyzer.Models
{
    public class GithubRepo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }
        public int OpenIssues { get; set; }
        public int ForksCount { get; set; }
        public bool IsArchived { get; set; }

        public override string ToString()
        {
            return $"Name: {Name ?? "-"} \nDescription: {Description ?? "-"} \nStars: {Stars} \nOpen Issues: {OpenIssues} \nForks Count: {ForksCount}\n";
        }
    }
}
