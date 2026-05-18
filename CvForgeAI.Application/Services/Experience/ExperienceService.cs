using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Application.DTO.Experience;

namespace CvForgeAI.Application.Services.Experience;

public class ExperienceService : IExperienceService
{
    private readonly IExperienceRepository _experienceRepository;
    private readonly IResumeRepository _resumeRepository;

    public ExperienceService(
        IExperienceRepository experienceRepository,
        IResumeRepository resumeRepository)
    {
        _experienceRepository = experienceRepository;
        _resumeRepository = resumeRepository;
    }

    public async Task<ExperienceResponse> CreateAsync(
        Guid userId,
        CreateExperienceRequest request)
    {
        var resumeExists = await _resumeRepository
            .ResumeBelongsToUserAsync(
                request.ResumeId,
                userId);

        if (!resumeExists)
        {
            throw new Exception("Resume not found.");
        }

        var experience = new Domain.Entities.Experience
        {
            CompanyName = request.CompanyName,
            Position = request.Position,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsCurrentJob = request.IsCurrentJob,
            ResumeId = request.ResumeId
        };

        await _experienceRepository.AddAsync(experience);

        await _experienceRepository.SaveChangesAsync();

        return new ExperienceResponse
        {
            Id = experience.Id,
            CompanyName = experience.CompanyName,
            Position = experience.Position,
            Description = experience.Description,
            StartDate = experience.StartDate,
            EndDate = experience.EndDate,
            IsCurrentJob = experience.IsCurrentJob,
            ResumeId = experience.ResumeId
        };
    }

    public async Task<List<ExperienceResponse>> GetResumeExperiencesAsync(
        Guid userId,
        int resumeId)
    {
        var resumeExists = await _resumeRepository
            .ResumeBelongsToUserAsync(
                resumeId,
                userId);

        if (!resumeExists)
        {
            throw new Exception("Resume not found.");
        }

        var experiences = await _experienceRepository
            .GetByResumeIdAsync(resumeId);

        return experiences.Select(x => new ExperienceResponse
        {
            Id = x.Id,
            CompanyName = x.CompanyName,
            Position = x.Position,
            Description = x.Description,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            IsCurrentJob = x.IsCurrentJob,
            ResumeId = x.ResumeId
        }).ToList();
    }
}