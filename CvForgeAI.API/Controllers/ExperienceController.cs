using CvForgeAI.Application.DTO.Experience;
using CvForgeAI.Application.Services.Experience;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace CvForgeAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExperienceController : ControllerBase
{
    private readonly IExperienceService _experienceService;

    public ExperienceController(
        IExperienceService experienceService)
    {
        _experienceService = experienceService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateExperienceRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Invalid token.");
        }

        var result = await _experienceService.CreateAsync(
            Guid.Parse(userId),
            request);

        return Ok(result);
    }

    [HttpGet("{resumeId}")]
    public async Task<IActionResult> GetResumeExperiences(
        int resumeId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Invalid token.");
        }

        var result = await _experienceService
            .GetResumeExperiencesAsync(
                Guid.Parse(userId),
                resumeId);

        return Ok(result);
    }
}