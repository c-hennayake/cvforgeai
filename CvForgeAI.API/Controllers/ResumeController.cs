using CvForgeAI.Application.DTO.Resume;
using CvForgeAI.Application.Services.Pdf;
using CvForgeAI.Application.Services.Resume;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace CvForgeAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ResumeController : ControllerBase
{
    private readonly IResumeService _resumeService;
    private readonly IPdfService _pdfService;

    public ResumeController(
        IResumeService resumeService,
        IPdfService pdfService)
    {
        _resumeService = resumeService;
        _pdfService = pdfService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateResumeRequest request)
    {
        var userId = User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Invalid token.");
        }

        var result = await _resumeService.CreateAsync(
            Guid.Parse(userId),
            request);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyResumes()
    {
        var userId = User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Invalid token.");
        }

        var resumes = await _resumeService
            .GetUserResumesAsync(
                Guid.Parse(userId));

        return Ok(resumes);
    }

    [HttpGet("{resumeId}/pdf")]
    public async Task<IActionResult> DownloadPdf(
        int resumeId)
    {
        var userId = User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Invalid token.");
        }

        var pdfBytes = await _pdfService
            .GenerateResumePdfAsync(
                Guid.Parse(userId),
                resumeId);

        return File(
            pdfBytes,
            "application/pdf",
            "resume.pdf");
    }
}