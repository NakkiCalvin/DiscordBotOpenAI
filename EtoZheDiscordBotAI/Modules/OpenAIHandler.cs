using EtoZheDiscordBotAI.Models;
using EtoZheDiscordBotAI.ModuleAbstractions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace EtoZheDiscordBotAI.Modules
{
    // TODO: create a separate package (OpenAI treatment) for this 2 bots: Telegram/Discord. Also implement better variaty of HttpClient handling.
    internal sealed class OpenAIHandler : IOpenAIHandler
    {
        public static readonly HttpClient _httpClient = new HttpClient();

        public OpenAIHandler(IConfiguration configuration)
        {
            var settings = configuration.GetRequiredSection("Settings").Get<Config>();

            _httpClient.DefaultRequestHeaders.Add("authorization", $"Bearer {settings.OpenAiToken}");
        }

        public async Task<string> HandleOpenAIRequest(string question, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(
               new OpenAIRequest
               {
                   Model = "text-davinci-003", // or 001 cheaper
                   Prompt = question,
                   Temperature = 1, // 0.7 or lower result will be more accuracy
                   MaxTokens = 1000, // how many symbols in response text
               });

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/completions")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // dummy case
                return string.Empty;
            }

            try
            {
                var dyData = JsonConvert.DeserializeObject<dynamic>(responseJson);

                var guess = dyData!.choices[0].text;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"---> My guess is: {guess}");
                Console.ResetColor();
                return guess;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not deserialize the JSON: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
