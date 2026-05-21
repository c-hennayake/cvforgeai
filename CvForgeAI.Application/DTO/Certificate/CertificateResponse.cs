namespace CvForgeAI.Application.DTO.Certificate;

public class CertificateResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; }

    public string? CredentialUrl { get; set; }

    public int ResumeId { get; set; }
}