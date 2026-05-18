using CvForgeAI.Application.DTO.Project;

namespace CvForgeAI.Application.Services.Projects;

public interface IProjectService
{
    Task<ProjectResponse> CreateAsync(
        Guid userId,
        CreateProjectRequest request);

    Task<List<ProjectResponse>> GetResumeProjectsAsync(
        Guid userId,
        int resumeId);
}