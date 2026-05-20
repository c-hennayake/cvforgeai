using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace CvForgeAI.Infrastructure.Persistence.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly AppDbContext _context;

    public SkillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Skill skill)
    {
        await _context.Skills.AddAsync(skill);
    }

    public async Task<List<Skill>> GetByResumeIdAsync(
        int resumeId)
    {
        return await _context.Skills
            .Where(x => x.ResumeId == resumeId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}