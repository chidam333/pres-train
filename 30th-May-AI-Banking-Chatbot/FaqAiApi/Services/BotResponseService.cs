using FaqAiApi.Models;
using FaqAiApi.Repositories;
using System.Text;
using System.Text.Json;

namespace FaqAiApi.Services;

public class BotResponseService
{
    private readonly BotResponseRepository _botResponseRepository;
     private readonly string _geminiKey;

    public BotResponseService(BotResponseRepository botResponseRepository, IConfiguration configuration)
    {
        _botResponseRepository = botResponseRepository;
        _geminiKey = configuration["GEMINI"] ?? throw new ArgumentNullException(nameof(configuration), "GEMINI configuration key is missing.");
    }

    public async Task<BotResponse> CreateResponseAsync(BotRequest br)
    {
        HttpClient httpClient = new HttpClient();
        var jsonObject = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = $"You are a banking chatbot and you are supposed to answer all answers related to it, if it unrelated to banking say 'No!!! I can't' \n\n {br.UserMessage}" }
                    }
                }
            }
        };
        var response = await httpClient.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_geminiKey}", new StringContent(JsonSerializer.Serialize(jsonObject), Encoding.UTF8, "application/json"));
        var data = response.Content;
        BotResponse botResponse = new BotResponse
        {
            SessionId = br.SessionId,
            CreatedAt = DateTime.UtcNow,
            BotMessage = await data.ReadAsStringAsync()
        };
        await _botResponseRepository.CreateAsync(botResponse);
        return botResponse;
    }
}
