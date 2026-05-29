using CvForgeAI.Application.DTO.Resume;

namespace CvForgeAI.Application.Services.Resume;

public interface IResumeService
{
    Task<string> CreateAsync(
        Guid userId,
        CreateResumeRequest request);

    Task<List<ResumeResponse>> GetUserResumesAsync(
        Guid userId);

    Task<string> GenerateAiSummaryAsync(
        Guid userId,
        int resumeId);

    Task UpdateAsync(
        Guid userId,
        int resumeId,
        UpdateResumeRequest request);

    Task<ResumeAnalysisResponse> AnalyzeResumeAsync(
    Guid userId,
    int resumeId);

    Task<JobMatchResponse> MatchJobAsync(
    Guid userId,
    JobMatchRequest request);
}