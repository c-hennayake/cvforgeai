using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Application.DTO.Resume;
using CvForgeAI.Application.Services.AI;

namespace CvForgeAI.Application.Services.Resume;

public class ResumeService : IResumeService
{
    private readonly IResumeRepository _resumeRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IExperienceRepository _experienceRepository;
    private readonly IAIService _aiService;

    public ResumeService(
        IResumeRepository resumeRepository,
        ISkillRepository skillRepository,
        IProjectRepository projectRepository,
        IExperienceRepository experienceRepository,
        IAIService aiService)
    {
        _resumeRepository = resumeRepository;
        _skillRepository = skillRepository;
        _projectRepository = projectRepository;
        _experienceRepository = experienceRepository;
        _aiService = aiService;
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

    public async Task<string> GenerateAiSummaryAsync(
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

        var resumes = await _resumeRepository
            .GetUserResumesAsync(userId);

        var resume = resumes.First(x => x.Id == resumeId);

        var skills = await _skillRepository
            .GetByResumeIdAsync(resumeId);

        var projects = await _projectRepository
            .GetByResumeIdAsync(resumeId);

        var experiences = await _experienceRepository
            .GetByResumeIdAsync(resumeId);

        var prompt = $@"
Generate a professional ATS-friendly resume summary.

Skills:
{string.Join(", ", skills.Select(x => x.Name))}

Projects:
{string.Join(", ", projects.Select(x => x.Title))}

Experience:
{string.Join(", ", experiences.Select(x => x.Position))}
";

        var summary = await _aiService
            .GenerateSummaryAsync(prompt);

        resume.Summary = summary;

        await _resumeRepository.SaveChangesAsync();

        return summary;
    }

    public async Task UpdateAsync(
        Guid userId,
        int resumeId,
        UpdateResumeRequest request)
    {
        var resume = await _resumeRepository
            .GetByIdAsync(resumeId);

        if (resume == null ||
            resume.UserId != userId)
        {
            throw new Exception("Resume not found.");
        }

        resume.Title = request.Title;
        resume.Summary = request.Summary;

        await _resumeRepository.SaveChangesAsync();
    }

    public async Task<ResumeAnalysisResponse>
        AnalyzeResumeAsync(
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

        var resumes = await _resumeRepository
            .GetUserResumesAsync(userId);

        var resume = resumes.First(x => x.Id == resumeId);

        var skills = await _skillRepository
            .GetByResumeIdAsync(resumeId);

        var projects = await _projectRepository
            .GetByResumeIdAsync(resumeId);

        var experiences = await _experienceRepository
            .GetByResumeIdAsync(resumeId);

        int score = 0;

        var feedback = new List<string>();

        // Summary
        if (!string.IsNullOrWhiteSpace(
            resume.Summary))
        {
            score += 25;
        }
        else
        {
            feedback.Add(
                "Add professional summary.");
        }

        // Skills
        if (skills.Count >= 5)
        {
            score += 25;
        }
        else
        {
            feedback.Add(
                "Add more technical skills.");
        }

        // Projects
        if (projects.Count >= 2)
        {
            score += 25;
        }
        else
        {
            feedback.Add(
                "Add more projects.");
        }

        // Experience
        if (experiences.Count >= 1)
        {
            score += 25;
        }
        else
        {
            feedback.Add(
                "Add work experience.");
        }

        var prompt = $@"
Analyze this resume and give ATS improvement suggestions.

Summary:
{resume.Summary}

Skills:
{string.Join(", ", skills.Select(x => x.Name))}

Projects:
{string.Join(", ", projects.Select(x => x.Title))}

Experience:
{string.Join(", ", experiences.Select(x => x.Position))}
";

        var aiSuggestions = await _aiService
            .AnalyzeResumeAsync(prompt);

        return new ResumeAnalysisResponse
        {
            Score = score,
            Feedback = feedback,
            AiSuggestions = aiSuggestions
        };
    }


    public async Task<JobMatchResponse>
     MatchJobAsync(
         Guid userId,
         JobMatchRequest request)
    {
        var resumeExists = await _resumeRepository
            .ResumeBelongsToUserAsync(
                request.ResumeId,
                userId);

        if (!resumeExists)
        {
            throw new Exception("Resume not found.");
        }

        var resumes = await _resumeRepository
            .GetUserResumesAsync(userId);

        var resume = resumes.First(
            x => x.Id == request.ResumeId);

        var skills = await _skillRepository
            .GetByResumeIdAsync(request.ResumeId);

        var projects = await _projectRepository
            .GetByResumeIdAsync(request.ResumeId);

        var experiences = await _experienceRepository
            .GetByResumeIdAsync(request.ResumeId);

        var prompt = $@"
Compare this resume with the job description.

Return ONLY valid JSON.

Format:
{{
  ""matchPercentage"": 0,
  ""missingSkills"": [],
  ""suggestions"": """"
}}

Resume Summary:
{resume.Summary}

Skills:
{string.Join(", ", skills.Select(x => x.Name))}

Projects:
{string.Join(", ", projects.Select(x => x.Title))}

Experience:
{string.Join(", ", experiences.Select(x => x.Position))}

Job Description:
{request.JobDescription}

IMPORTANT:
- Return ONLY JSON
- No markdown
- No explanations
";

        var aiResult = await _aiService
            .AnalyzeResumeAsync(prompt);

        aiResult = aiResult
    .Replace("```json", "")
    .Replace("```", "")
    .Trim();

        Console.WriteLine(aiResult);

        var result = System.Text.Json.JsonSerializer
            .Deserialize<JobMatchResponse>(
                aiResult,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        if (result == null)
        {
            throw new Exception(
                "Failed to analyze job match.");
        }

        return result;
    }
}