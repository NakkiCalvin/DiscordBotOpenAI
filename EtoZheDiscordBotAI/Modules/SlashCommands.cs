using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using EtoZheDiscordBotAI.ModuleAbstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EtoZheDiscordBotAI.Modules
{
    public class SlashCommands : ApplicationCommandModule
    {
        [SlashCommand("ask", "Returns answer from OpenAI")]
        public async Task TestCommand(InteractionContext context, [Option("question", "Your question to OpenAI")] string question)
        {
            await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            var openAIHandler = context.Services.GetService<IOpenAIHandler>();
            var answer = await openAIHandler!.HandleOpenAIRequest(question, CancellationToken.None);

            await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"```{answer}```"));

            //await context
            //    .CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder()
            //    .WithContent(answer));
        }
    }
}