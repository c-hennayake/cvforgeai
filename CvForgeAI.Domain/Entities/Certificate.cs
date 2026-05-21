namespace CvForgeAI.Domain.Entities;

public class Certificate
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; }

    public string? CredentialUrl { get; set; }

    public int ResumeId { get; set; }

    public Resume Resume { get; set; } = null!;
}