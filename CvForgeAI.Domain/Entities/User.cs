using CvForgeAI.Domain.Common;
namespace CvForgeAI.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    public ICollection<Resume> Resumes { get; set; }
        = new List<Resume>();
}