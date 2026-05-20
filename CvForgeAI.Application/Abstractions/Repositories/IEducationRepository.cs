using CvForgeAI.Domain.Entities;

namespace CvForgeAI.Application.Abstractions.Repositories;

public interface IEducationRepository
{
    Task AddAsync(Education education);

    Task<List<Education>> GetByResumeIdAsync(
        int resumeId);

    Task SaveChangesAsync();
}