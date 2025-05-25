namespace GitHubRepoAnalyzer.Models
{
    public class CommitData
    {
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime CommitDate { get; set; }

        public override string ToString()
        {
            return $"Message: {Message} \nAuthor: {Author} \nDate: {CommitDate} \n_________________________________\n";
        }
    }
}
