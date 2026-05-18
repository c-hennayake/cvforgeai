namespace CvForgeAI.Application.DTO.Skill;

public class SkillResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Level { get; set; } = string.Empty;

    public int ResumeId { get; set; }
}