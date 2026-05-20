using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace CvForgeAI.Infrastructure.Persistence.Repositories;

public class ResumeRepository : IResumeRepository
{
    private readonly AppDbContext _context;

    public ResumeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Resume resume)
    {
        await _context.Resumes.AddAsync(resume);
    }

    public async Task<List<Resume>> GetUserResumesAsync(
        Guid userId)
    {
        return await _context.Resumes
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> ResumeBelongsToUserAsync(
        int resumeId,
        Guid userId)
    {
        return await _context.Resumes
            .AnyAsync(x =>
                x.Id == resumeId &&
                x.UserId == userId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}