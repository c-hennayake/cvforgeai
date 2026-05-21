namespace CvForgeAI.Application.Services.Pdf;

public interface IPdfService
{
    Task<byte[]> GenerateResumePdfAsync(
        Guid userId,
        int resumeId);
}