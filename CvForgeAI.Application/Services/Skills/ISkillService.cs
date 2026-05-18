using CvForgeAI.Application.DTO.Skill;

namespace CvForgeAI.Application.Services.Skills;

public interface ISkillService
{
    Task<SkillResponse> CreateAsync(
        Guid userId,
        CreateSkillRequest request);

    Task<List<SkillResponse>> GetResumeSkillsAsync(
        Guid userId,
        int resumeId);
}