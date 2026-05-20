using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace CvForgeAI.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
    }

    public async Task<List<Project>> GetByResumeIdAsync(
        int resumeId)
    {
        return await _context.Projects
            .Where(x => x.ResumeId == resumeId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}