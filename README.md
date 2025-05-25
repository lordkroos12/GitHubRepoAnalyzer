# GitHub Repository Analyzer

A console application that analyzes GitHub repositories

## 🚀 Features

### Core Analysis
- **Repository Overview**: Name, description, stars, forks, and open issues
- **Top Contributors**: Top 5 contributors ranked by commit count
- **Recent Activity**: Commits from the last 7 days
- **Language Statistics**: Programming languages used with percentages
- **Pull Request Metrics**: Open, closed, and merged PR counts
- **Issue Analytics**: Average time to close issues

### Repository Classification
The application automatically classifies repositories into three categories:

- 🟢 **Live**: Active repositories with recent contributions and maintainer engagement
  - Not archived
  - Has contributions in the last 6 months
  - Maintainer responded to issues in the last 3 months

- 🟡 **Stagnant**: Repositories with limited recent activity
  - No contributions in 6+ months
  - Some issue activity in the last 6 months

- 🔴 **Dead**: Inactive repositories
  - Archived repositories
  - No contributions in 1+ year
  - No maintainer response in 1+ year

## 🏗️ Project Structure

```
GitHubRepoAnalyzer/
├── Classifiers/              # Status classification logic
│   ├── DeadClassifier.cs
│   ├── LiveClassifier.cs
│   └── StagnantClassifier.cs
├── Enums/
│   └── RepositoryStatus.cs   # Status enumeration
├── Helper/
│   ├── PrintHelper.cs        # Console menu handling
│   └── TokenProvider.cs      # GitHub token management
├── Interfaces/               # Abstraction layer
│   ├── IApiService.cs
│   ├── IPrintHelper.cs
│   ├── IStatusClassifier.cs
│   └── IStatusService.cs
├── Models/                   # Models needed to print data
│   ├── CommitData.cs
│   ├── ContributorData.cs
│   ├── ExtendedRepositoryData.cs
│   ├── GithubRepo.cs
│   ├── IssueData.cs
│   ├── LanguageData.cs
│   └── PullRequestData.cs   
├── Service/
│   ├── ApiService.cs         # GitHub API integration
│   └── StatusService.cs      # Repository status determination
└── Program.cs               # Application entry point
```

## 🔧 Setup and Installation

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/GitHubRepoAnalyzer.git
cd GitHubRepoAnalyzer
```

### 2. Install Dependencies
```bash
dotnet restore
```

### 3. Configure GitHub Token (Optional)

#### Option 1: Environment Variable
```bash
# Windows PowerShell
$env:GITHUB_TOKEN="your_github_token_here"

# Windows Command Prompt
set GITHUB_TOKEN=your_github_token_here

# Linux/macOS
export GITHUB_TOKEN="your_github_token_here"
```

#### Option 2: User Secrets (Recommended for development)
```bash
dotnet user-secrets init
dotnet user-secrets set "GitHubToken" "your_github_token_here"
```

#### Option 3: Manual Entry
If no token is configured, the application will prompt you to enter it manually when started.

### 4. Run the Application
```bash
dotnet run
```

## 🎯 How to Use

### Getting Started
1. **Launch the application**: Run `dotnet run` in the project directory
2. **Provide GitHub token**: Enter your token when prompted (or skip for limited API access)
3. **Enter repository URL**: Provide a GitHub repository URL in the format:
   ```
   https://github.com/owner/repository-name
   ```

### Available Options
Once you've entered a repository URL, you can choose from the following analysis options:

1. **Basic Info** - Repository name, description, stars, forks, and open issues
2. **Pull Requests** - Statistics on open, closed, and merged pull requests
3. **Contributors** - Top 5 contributors by commit count
4. **Commits** - Recent commits from the last 7 days
5. **Languages** - Programming languages used with percentage breakdown
6. **Issue Analytics** - Average time to close issues
7. **Full Report** - Complete analysis including repository health status
8. **Change Repository** - Analyze a different repository
9. **Exit** - Close the application

### Example Usage
```
Enter GitHub repository as a link:
https://github.com/dotnet/runtime

What do you want to fetch?
1. Basic info
2. Pull Requests
3. Contributors
4. Commits (last 7 days)
5. Languages
6. Average time to close issue
7. Full Report
8. Change repository
0. Exit

Enter your choice: 7
```

## 🔑 GitHub Token Setup

### Why Use a Token?
- **Higher Rate Limits**: 5,000 requests/hour vs 60 requests/hour without authentication
- **Access to Private Repositories**: If you have access permissions
- **Better Performance**: Avoid rate limiting delays

### Creating a Personal Access Token
1. Go to GitHub Settings → Developer settings → Personal access tokens
2. Click "Generate new token"
3. Select appropriate scopes (public_repo is sufficient for public repositories)
4. Copy the generated token

## 🏛️ Architecture

The application follows SOLID principles and is designed for extensibility:

### Key Design Patterns
- **Strategy Pattern**: Status classifiers can be easily added or modified
- **Dependency Injection**: Services are loosely coupled through interfaces
- **Repository Pattern**: Clean separation between data access and business logic

### Extensibility
To add new repository status classifications:

1. Create a new classifier implementing `IStatusClassifier`
- The system automatically discovers and registers all classifiers at startup using reflection.


## 📊 Data Sources

The application fetches data from various GitHub API endpoints:
- Repository information
- Contributors statistics
- Commit history
- Language statistics
- Pull request data
- Issue tracking
- Maintainer activity patterns
