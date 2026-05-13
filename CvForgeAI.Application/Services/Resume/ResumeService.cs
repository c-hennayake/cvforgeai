using CvForgeAI.Application.DTO.Resume;
using CvForgeAI.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace CvForgeAI.Application.Services.Resume;

public class ResumeService : IResumeService
{
    private readonly AppDbContext _context;

    public ResumeService(AppDbContext context)
    {
        _context = context;
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

        _context.Resumes.Add(resume);

        await _context.SaveChangesAsync();

        return "Resume created successfully.";
    }

    public async Task<List<ResumeResponse>> GetUserResumesAsync(Guid userId)
    {
        return await _context.Resumes
            .Where(x => x.UserId == userId)
            .Select(x => new ResumeResponse
            {
                Id = x.Id,
                Title = x.Title,
                Summary = x.Summary,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }
}