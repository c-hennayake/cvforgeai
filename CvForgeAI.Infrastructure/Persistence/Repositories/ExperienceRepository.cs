
using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace CvForgeAI.Infrastructure.Persistence.Repositories;

public class ExperienceRepository : IExperienceRepository
{
    private readonly AppDbContext _context;

    public ExperienceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Experience experience)
    {
        await _context.Experiences.AddAsync(experience);
    }

    public async Task<List<Experience>> GetByResumeIdAsync(
        int resumeId)
    {
        return await _context.Experiences
            .Where(x => x.ResumeId == resumeId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}