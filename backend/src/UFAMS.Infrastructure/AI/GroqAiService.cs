using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Options;

using UFAMS.Application.AI;

namespace UFAMS.Infrastructure.AI;

public sealed class GroqAiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly AiOptions _options;

    public GroqAiService(
        HttpClient httpClient,
        IOptions<AiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> GenerateTreeSummaryAsync(
        string prompt,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(
                _options.ApiKey))
        {
            throw new InvalidOperationException(
                "AI API key is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _options.Model))
        {
            throw new InvalidOperationException(
                "AI model is not configured.");
        }

        var request = new
        {
            model = _options.Model,

            messages = new[]
            {
                new
                {
                    role = "system",
                    content =
                        """
                        You are an AI assistant for UFAMS,
                        an urban forest asset management system.

                        Analyze tree asset information provided
                        by the application.

                        Provide practical, concise information
                        suitable for municipal urban forestry
                        staff.

                        Do not invent facts.

                        If the available information is insufficient
                        to make a recommendation, clearly state that.

                        Organize your response using:

                        Summary
                        Risk Assessment
                        Recommended Actions
                        """
                },

                new
                {
                    role = "user",
                    content = prompt
                }
            },

            temperature = 0.2,

            max_completion_tokens = 800
        };

        using var requestMessage =
            new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.groq.com/openai/v1/chat/completions");

        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                _options.ApiKey);

        requestMessage.Content =
            new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

        using var response =
            await _httpClient.SendAsync(
                requestMessage,
                cancellationToken);

        var responseBody =
            await response.Content.ReadAsStringAsync(
                cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Groq API request failed " +
                $"with status {(int)response.StatusCode}: " +
                responseBody);
        }

        using var json =
            JsonDocument.Parse(responseBody);

        var content =
            json.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidOperationException(
                "Groq returned an empty response.");
        }

        return content;
    }
}