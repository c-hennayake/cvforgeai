using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Application.DTO.Education;

namespace CvForgeAI.Application.Services.Education;

public class EducationService : IEducationService
{
    private readonly IEducationRepository _educationRepository;
    private readonly IResumeRepository _resumeRepository;

    public EducationService(
        IEducationRepository educationRepository,
        IResumeRepository resumeRepository)
    {
        _educationRepository = educationRepository;
        _resumeRepository = resumeRepository;
    }

    public async Task<EducationResponse> CreateAsync(
        Guid userId,
        CreateEducationRequest request)
    {
        var resumeExists = await _resumeRepository
            .ResumeBelongsToUserAsync(
                request.ResumeId,
                userId);

        if (!resumeExists)
        {
            throw new Exception("Resume not found.");
        }

        var education = new Domain.Entities.Education
        {
            InstituteName = request.InstituteName,
            Degree = request.Degree,
            FieldOfStudy = request.FieldOfStudy,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsCurrentlyStudying = request.IsCurrentlyStudying,
            ResumeId = request.ResumeId
        };

        await _educationRepository.AddAsync(education);

        await _educationRepository.SaveChangesAsync();

        return new EducationResponse
        {
            Id = education.Id,
            InstituteName = education.InstituteName,
            Degree = education.Degree,
            FieldOfStudy = education.FieldOfStudy,
            StartDate = education.StartDate,
            EndDate = education.EndDate,
            IsCurrentlyStudying = education.IsCurrentlyStudying,
            ResumeId = education.ResumeId
        };
    }

    public async Task<List<EducationResponse>> GetResumeEducationsAsync(
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

        var educations = await _educationRepository
            .GetByResumeIdAsync(resumeId);

        return educations.Select(x => new EducationResponse
        {
            Id = x.Id,
            InstituteName = x.InstituteName,
            Degree = x.Degree,
            FieldOfStudy = x.FieldOfStudy,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            IsCurrentlyStudying = x.IsCurrentlyStudying,
            ResumeId = x.ResumeId
        }).ToList();
    }
}