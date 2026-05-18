using CvForgeAI.Domain.Entities;

namespace CvForgeAI.Application.Abstractions.Repositories;

public interface IExperienceRepository
{
    Task AddAsync(Experience experience);

    Task<List<Experience>> GetByResumeIdAsync(
        int resumeId);

    Task SaveChangesAsync();
}