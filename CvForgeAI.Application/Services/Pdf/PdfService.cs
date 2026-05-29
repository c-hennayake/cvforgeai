using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Application.Exceptions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection.Metadata;

namespace CvForgeAI.Application.Services.Pdf;

public class PdfService : IPdfService
{
    private readonly IResumeRepository _resumeRepository;
    private readonly IExperienceRepository _experienceRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICertificateRepository _certificateRepository;

    public PdfService(
        IResumeRepository resumeRepository,
        IExperienceRepository experienceRepository,
        IEducationRepository educationRepository,
        ISkillRepository skillRepository,
        IProjectRepository projectRepository,
        ICertificateRepository certificateRepository)
    {
        _resumeRepository = resumeRepository;
        _experienceRepository = experienceRepository;
        _educationRepository = educationRepository;
        _skillRepository = skillRepository;
        _projectRepository = projectRepository;
        _certificateRepository = certificateRepository;
    }

    public async Task<byte[]> GenerateResumePdfAsync(
        Guid userId,
        int resumeId)
    {
        var resumeExists = await _resumeRepository
            .ResumeBelongsToUserAsync(
                resumeId,
                userId);

        if (!resumeExists)
        {
            throw new NotFoundException(
      "Resume not found.");
        }

        var resumes = await _resumeRepository
            .GetUserResumesAsync(userId);

        var resume = resumes.First(x => x.Id == resumeId);

        var experiences = await _experienceRepository
            .GetByResumeIdAsync(resumeId);

        var educations = await _educationRepository
            .GetByResumeIdAsync(resumeId);

        var skills = await _skillRepository
            .GetByResumeIdAsync(resumeId);

        var projects = await _projectRepository
            .GetByResumeIdAsync(resumeId);

        var certificates = await _certificateRepository
            .GetByResumeIdAsync(resumeId);

        QuestPDF.Settings.License = LicenseType.Community;

        var pdf = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header()
                    .Text(resume.Title)
                    .FontSize(24)
                    .Bold();

                page.Content()
                    .Column(column =>
                    {
                        column.Spacing(10);

                        column.Item()
                            .Text(resume.Summary);

                        column.Item()
                            .Text("Skills")
                            .FontSize(18)
                            .Bold();

                        foreach (var skill in skills)
                        {
                            column.Item()
                                .Text($"• {skill.Name} - {skill.Level}");
                        }

                        column.Item()
                            .Text("Experience")
                            .FontSize(18)
                            .Bold();

                        foreach (var exp in experiences)
                        {
                            column.Item()
                                .Text($"{exp.Position} - {exp.CompanyName}");

                            column.Item()
                                .Text(exp.Description);
                        }

                        column.Item()
                            .Text("Education")
                            .FontSize(18)
                            .Bold();

                        foreach (var edu in educations)
                        {
                            column.Item()
                                .Text($"{edu.Degree} - {edu.InstituteName}");
                        }

                        column.Item()
                            .Text("Projects")
                            .FontSize(18)
                            .Bold();

                        foreach (var project in projects)
                        {
                            column.Item()
                                .Text(project.Title);

                            column.Item()
                                .Text(project.Description);
                        }

                        column.Item()
                            .Text("Certificates")
                            .FontSize(18)
                            .Bold();

                        foreach (var cert in certificates)
                        {
                            column.Item()
                                .Text($"{cert.Name} - {cert.Issuer}");
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Generated by CVForgeAI");
                    });
            });
        }).GeneratePdf();

        return pdf;
    }
}