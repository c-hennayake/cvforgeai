namespace CvForgeAI.Application.DTO.Resume;

public class ResumeAnalysisResponse
{

    public int Score { get; set; }

    public List<string> Feedback { get; set; }
        = new();

    public string AiSuggestions { get; set; }
        = string.Empty;
}