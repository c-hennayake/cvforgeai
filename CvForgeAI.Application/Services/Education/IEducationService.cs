using CvForgeAI.Application.DTO.Education;

namespace CvForgeAI.Application.Services.Education;

public interface IEducationService
{
    Task<EducationResponse> CreateAsync(
        Guid userId,
        CreateEducationRequest request);

    Task<List<EducationResponse>> GetResumeEducationsAsync(
        Guid userId,
        int resumeId);
}