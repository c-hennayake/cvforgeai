namespace CvForgeAI.Application.DTO.Experience;

public class CreateExperienceRequest
{
    public string CompanyName { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsCurrentJob { get; set; }

    public int ResumeId { get; set; }
}