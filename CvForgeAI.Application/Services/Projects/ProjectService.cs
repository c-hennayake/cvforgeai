using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Application.DTO.Project;
using CvForgeAI.Application.Exceptions;
using CvForgeAI.Domain.Entities;

namespace CvForgeAI.Application.Services.Projects;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IResumeRepository _resumeRepository;

    public ProjectService(
        IProjectRepository projectRepository,
        IResumeRepository resumeRepository)
    {
        _projectRepository = projectRepository;
        _resumeRepository = resumeRepository;
    }

    public async Task<ProjectResponse> CreateAsync(
        Guid userId,
        CreateProjectRequest request)
    {
        var resumeExists = await _resumeRepository
            .ResumeBelongsToUserAsync(
                request.ResumeId,
                userId);

        if (!resumeExists)
        {
            throw new Exception("Resume not found.");
        }

        var project = new Project
        {
            Title = request.Title,
            Description = request.Description,
            TechnologiesUsed = request.TechnologiesUsed,
            GithubUrl = request.GithubUrl,
            LiveUrl = request.LiveUrl,
            ResumeId = request.ResumeId
        };

        await _projectRepository.AddAsync(project);

        await _projectRepository.SaveChangesAsync();

        return new ProjectResponse
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            TechnologiesUsed = project.TechnologiesUsed,
            GithubUrl = project.GithubUrl,
            LiveUrl = project.LiveUrl,
            ResumeId = project.ResumeId
        };
    }

    public async Task<List<ProjectResponse>> GetResumeProjectsAsync(
        Guid userId,
        int resumeId)
    {
        var resumeExists = await _resumeRepository
            .ResumeBelongsToUserAsync(
                resumeId,
                userId);

        if (!resumeExists)
        {
            throw new NotFoundException(
    "Resume not found.");
        }

        var projects = await _projectRepository
            .GetByResumeIdAsync(resumeId);

        return projects.Select(x => new ProjectResponse
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            TechnologiesUsed = x.TechnologiesUsed,
            GithubUrl = x.GithubUrl,
            LiveUrl = x.LiveUrl,
            ResumeId = x.ResumeId
        }).ToList();
    }
}