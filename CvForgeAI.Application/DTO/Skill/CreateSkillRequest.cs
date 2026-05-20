namespace CvForgeAI.Application.DTO.Skill;

public class CreateSkillRequest
{
    public string Name { get; set; } = string.Empty;

    public string Level { get; set; } = string.Empty;

    public int ResumeId { get; set; }
}