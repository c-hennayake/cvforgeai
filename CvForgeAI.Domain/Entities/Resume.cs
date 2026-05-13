namespace CvForgeAI.Domain.Entities;

public class Resume
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    public Guid UserId { get; set; }

    // Navigation Property
    public User User { get; set; } = null!;
}