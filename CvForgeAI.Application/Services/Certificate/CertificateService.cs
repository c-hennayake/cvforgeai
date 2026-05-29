using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Application.DTO.Certificate;
using CvForgeAI.Application.Exceptions;
using CvForgeAI.Domain.Entities;

namespace CvForgeAI.Application.Services.Certificates;

public class CertificateService : ICertificateService
{
    private readonly ICertificateRepository _certificateRepository;
    private readonly IResumeRepository _resumeRepository;

    public CertificateService(
        ICertificateRepository certificateRepository,
        IResumeRepository resumeRepository)
    {
        _certificateRepository = certificateRepository;
        _resumeRepository = resumeRepository;
    }

    public async Task<CertificateResponse> CreateAsync(
        Guid userId,
        CreateCertificateRequest request)
    {
        var resumeExists = await _resumeRepository
            .ResumeBelongsToUserAsync(
                request.ResumeId,
                userId);

        if (!resumeExists)
        {
            throw new NotFoundException(
     "Resume not found.");
        }

        var certificate = new Certificate
        {
            Name = request.Name,
            Issuer = request.Issuer,
            IssueDate = request.IssueDate,
            CredentialUrl = request.CredentialUrl,
            ResumeId = request.ResumeId
        };

        await _certificateRepository.AddAsync(certificate);

        await _certificateRepository.SaveChangesAsync();

        return new CertificateResponse
        {
            Id = certificate.Id,
            Name = certificate.Name,
            Issuer = certificate.Issuer,
            IssueDate = certificate.IssueDate,
            CredentialUrl = certificate.CredentialUrl,
            ResumeId = certificate.ResumeId
        };
    }

    public async Task<List<CertificateResponse>> GetResumeCertificatesAsync(
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

        var certificates = await _certificateRepository
            .GetByResumeIdAsync(resumeId);

        return certificates.Select(x => new CertificateResponse
        {
            Id = x.Id,
            Name = x.Name,
            Issuer = x.Issuer,
            IssueDate = x.IssueDate,
            CredentialUrl = x.CredentialUrl,
            ResumeId = x.ResumeId
        }).ToList();
    }
}