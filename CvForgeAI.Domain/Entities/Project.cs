using CvForgeAI.Domain.Entities;

public class Project
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string TechnologiesUsed { get; set; } = string.Empty;

    public string? GithubUrl { get; set; }

    public string? LiveUrl { get; set; }

    public int ResumeId { get; set; }

    public Resume Resume { get; set; } = null!;
}