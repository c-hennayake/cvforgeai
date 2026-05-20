using CvForgeAI.Domain.Entities;

namespace CvForgeAI.Application.Abstractions.Repositories;

public interface IProjectRepository
{
    Task AddAsync(Project project);

    Task<List<Project>> GetByResumeIdAsync(
        int resumeId);

    Task SaveChangesAsync();
}