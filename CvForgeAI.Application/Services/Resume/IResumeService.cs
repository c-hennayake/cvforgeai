using CvForgeAI.Application.DTO.Resume;

namespace CvForgeAI.Application.Services.Resume;

public interface IResumeService
{
    Task<string> CreateAsync(
        Guid userId,
        CreateResumeRequest request);

    Task<List<ResumeResponse>> GetUserResumesAsync(Guid userId);
}