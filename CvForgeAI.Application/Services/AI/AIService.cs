using Microsoft.Extensions.Configuration;

using RestSharp;

using System.Text.Json;

namespace CvForgeAI.Application.Services.AI;

public class AIService : IAIService
{
    private readonly IConfiguration _configuration;

    public AIService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GenerateSummaryAsync(
        string prompt)
    {
        var apiKey = _configuration["OpenRouter:ApiKey"];

        var client = new RestClient(
            "https://openrouter.ai/api/v1/chat/completions");

        var request = new RestRequest("", Method.Post);

        request.AddHeader(
            "Authorization",
            $"Bearer {apiKey}");

        request.AddHeader(
            "Content-Type",
            "application/json");

        var body = new
        {
            model = "openai/gpt-3.5-turbo",

            messages = new[]
            {
                new
                {
                    role = "user",
                    content = prompt
                }
            }
        };

        request.AddJsonBody(body);

        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            throw new Exception(
                $"Status: {response.StatusCode}\n{response.Content}");
        }

        using var doc = JsonDocument.Parse(response.Content!);

        var text = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return text ?? "No response.";
    }
}