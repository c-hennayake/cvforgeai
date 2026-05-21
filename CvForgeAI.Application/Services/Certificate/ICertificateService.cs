using CvForgeAI.Application.DTO.Certificate;

namespace CvForgeAI.Application.Services.Certificates;

public interface ICertificateService
{
    Task<CertificateResponse> CreateAsync(
        Guid userId,
        CreateCertificateRequest request);

    Task<List<CertificateResponse>> GetResumeCertificatesAsync(
        Guid userId,
        int resumeId);
} 