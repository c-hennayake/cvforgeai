using CvForgeAI.Application.DTO.Education;
using CvForgeAI.Application.Services.Education;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace CvForgeAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EducationController : ControllerBase
{
    private readonly IEducationService _educationService;

    public EducationController(
        IEducationService educationService)
    {
        _educationService = educationService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateEducationRequest request)
    {
        var userId = User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Invalid token.");
        }

        var result = await _educationService.CreateAsync(
            Guid.Parse(userId),
            request);

        return Ok(result);
    }

    [HttpGet("{resumeId}")]
    public async Task<IActionResult> GetResumeEducations(
        int resumeId)
    {
        var userId = User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Invalid token.");
        }

        var result = await _educationService
            .GetResumeEducationsAsync(
                Guid.Parse(userId),
                resumeId);

        return Ok(result);
    }
}