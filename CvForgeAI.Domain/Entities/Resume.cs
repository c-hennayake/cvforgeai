namespace CvForgeAI.Domain.Entities;

public class Resume
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public ICollection<Experience> Experiences { get; set; }
        = new List<Experience>();

    public ICollection<Skill> Skills { get; set; }
    = new List<Skill>();

    public ICollection<Education> Educations { get; set; }
    = new List<Education>();

    public ICollection<Project> Projects { get; set; }
    = new List<Project>();

    public ICollection<Certificate> Certificates { get; set; }
    = new List<Certificate>();

}