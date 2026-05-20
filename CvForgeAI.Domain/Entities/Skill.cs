using CvForgeAI.Domain.Entities;

public class Skill
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Level { get; set; } = string.Empty;

    public int ResumeId { get; set; }

    public Resume Resume { get; set; } = null!;
}