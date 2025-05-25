namespace GitHubRepoAnalyzer.Models
{
    public class ContributorData
    {
        public string Login { get; set; }
        public int Commits { get; set; }

        public override string ToString()
        {
            return $"Author: {Login}\nTotal number of commits: {Commits}\n";
        }
    }
}
