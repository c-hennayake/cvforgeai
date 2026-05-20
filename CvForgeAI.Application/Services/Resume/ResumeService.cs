using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Application.DTO.Resume;

namespace CvForgeAI.Application.Services.Resume;

public class ResumeService : IResumeService
{
    private readonly IResumeRepository _resumeRepository;

    public ResumeService(
        IResumeRepository resumeRepository)
    {
        _resumeRepository = resumeRepository;
    }

    public async Task<string> CreateAsync(
        Guid userId,
        CreateResumeRequest request)
    {
        var resume = new Domain.Entities.Resume
        {
            Title = request.Title,
            Summary = request.Summary,
            UserId = userId
        };

        await _resumeRepository.AddAsync(resume);

        await _resumeRepository.SaveChangesAsync();

        return "Resume created successfully.";
    }

    public async Task<List<ResumeResponse>> GetUserResumesAsync(
        Guid userId)
    {
        var resumes = await _resumeRepository
            .GetUserResumesAsync(userId);

        return resumes.Select(x => new ResumeResponse
        {
            Id = x.Id,
            Title = x.Title,
            Summary = x.Summary,
            CreatedAt = x.CreatedAt
        }).ToList();
    }
}