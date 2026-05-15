using CvForgeAI.Application.DTO.Experience;
using CvForgeAI.Domain.Entities;
using CvForgeAI.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace CvForgeAI.Application.Services.Experience;

public class ExperienceService : IExperienceService
{
    private readonly AppDbContext _context;

    public ExperienceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ExperienceResponse> CreateAsync(
        Guid userId,
        CreateExperienceRequest request)
    {
        var resume = await _context.Resumes
            .FirstOrDefaultAsync(x =>
                x.Id == request.ResumeId &&
                x.UserId == userId);

        if (resume == null)
        {
            throw new Exception("Resume not found.");
        }

        var experience = new Domain.Entities.Experience
        {
            CompanyName = request.CompanyName,
            Position = request.Position,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsCurrentJob = request.IsCurrentJob,
            ResumeId = request.ResumeId
        };

        _context.Experiences.Add(experience);

        await _context.SaveChangesAsync();

        return new ExperienceResponse
        {
            Id = experience.Id,
            CompanyName = experience.CompanyName,
            Position = experience.Position,
            Description = experience.Description,
            StartDate = experience.StartDate,
            EndDate = experience.EndDate,
            IsCurrentJob = experience.IsCurrentJob,
            ResumeId = experience.ResumeId
        };
    }

    public async Task<List<ExperienceResponse>> GetResumeExperiencesAsync(
        Guid userId,
        int resumeId)
    {
        var resumeExists = await _context.Resumes
            .AnyAsync(x =>
                x.Id == resumeId &&
                x.UserId == userId);

        if (!resumeExists)
        {
            throw new Exception("Resume not found.");
        }

        return await _context.Experiences
            .Where(x => x.ResumeId == resumeId)
            .Select(x => new ExperienceResponse
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                Position = x.Position,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                IsCurrentJob = x.IsCurrentJob,
                ResumeId = x.ResumeId
            })
            .ToListAsync();
    }
}