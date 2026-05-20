using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Application.DTO.Skill;
using CvForgeAI.Domain.Entities;

namespace CvForgeAI.Application.Services.Skills;

public class SkillService : ISkillService
{
    private readonly ISkillRepository _skillRepository;
    private readonly IResumeRepository _resumeRepository;

    public SkillService(
        ISkillRepository skillRepository,
        IResumeRepository resumeRepository)
    {
        _skillRepository = skillRepository;
        _resumeRepository = resumeRepository;
    }

    public async Task<SkillResponse> CreateAsync(
        Guid userId,
        CreateSkillRequest request)
    {
        var resumeExists = await _resumeRepository
            .ResumeBelongsToUserAsync(
                request.ResumeId,
                userId);

        if (!resumeExists)
        {
            throw new Exception("Resume not found.");
        }

        var skill = new Skill
        {
            Name = request.Name,
            Level = request.Level,
            ResumeId = request.ResumeId
        };

        await _skillRepository.AddAsync(skill);

        await _skillRepository.SaveChangesAsync();

        return new SkillResponse
        {
            Id = skill.Id,
            Name = skill.Name,
            Level = skill.Level,
            ResumeId = skill.ResumeId
        };
    }

    public async Task<List<SkillResponse>> GetResumeSkillsAsync(
        Guid userId,
        int resumeId)
    {
        var resumeExists = await _resumeRepository
            .ResumeBelongsToUserAsync(
                resumeId,
                userId);

        if (!resumeExists)
        {
            throw new Exception("Resume not found.");
        }

        var skills = await _skillRepository
            .GetByResumeIdAsync(resumeId);

        return skills.Select(x => new SkillResponse
        {
            Id = x.Id,
            Name = x.Name,
            Level = x.Level,
            ResumeId = x.ResumeId
        }).ToList();
    }
}