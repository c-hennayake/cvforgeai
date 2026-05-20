using CvForgeAI.Application.DTO.Experience;

namespace CvForgeAI.Application.Services.Experience;

public interface IExperienceService
{
    Task<ExperienceResponse> CreateAsync(
        Guid userId,
        CreateExperienceRequest request);

    Task<List<ExperienceResponse>> GetResumeExperiencesAsync(
        Guid userId,
        int resumeId);
}