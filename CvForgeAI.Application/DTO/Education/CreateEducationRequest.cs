namespace CvForgeAI.Application.DTO.Education;

public class CreateEducationRequest
{
    public string InstituteName { get; set; } = string.Empty;

    public string Degree { get; set; } = string.Empty;

    public string FieldOfStudy { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsCurrentlyStudying { get; set; }

    public int ResumeId { get; set; }
}