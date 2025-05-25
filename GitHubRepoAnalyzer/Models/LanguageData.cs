namespace GitHubRepoAnalyzer.Models
{
    public class LanguageData
    {
        public string Name { get; set; }
        public double Percentage { get; set; }

        public override string ToString()
        {
            return $"Name: {Name} \nPercentage: {Percentage:F1}%\n";
        }
    }
}
