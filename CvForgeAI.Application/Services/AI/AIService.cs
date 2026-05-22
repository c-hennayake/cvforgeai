using CvForgeAI.Application.Common.Models.OpenAI;
using Microsoft.Extensions.Configuration;

using RestSharp;
using System.Net.Http.Json;
using System.Text.Json;

namespace CvForgeAI.Application.Services.AI;

public class AIService : IAIService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public AIService(IConfiguration configuration ,HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
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



    public async Task<string> AnalyzeResumeAsync(
    string prompt)
    {
        var response = await _httpClient
            .PostAsJsonAsync(
                "chat/completions",
                new
                {
                    model = "deepseek/deepseek-chat",
                    messages = new[]
                    {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                    }
                });

        response.EnsureSuccessStatusCode();

        var result = await response
            .Content
            .ReadFromJsonAsync<OpenAIResponse>();

        return result!
            .Choices[0]
            .Message
            .Content;
    }
}