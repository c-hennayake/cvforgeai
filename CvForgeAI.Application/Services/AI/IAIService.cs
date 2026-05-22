namespace CvForgeAI.Application.Services.AI;

public interface IAIService
{
    Task<string> GenerateSummaryAsync(
        string prompt);
}