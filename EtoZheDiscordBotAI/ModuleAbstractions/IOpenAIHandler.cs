namespace EtoZheDiscordBotAI.ModuleAbstractions
{
    internal interface IOpenAIHandler
    {
        Task<string> HandleOpenAIRequest(string question, CancellationToken cancellationToken);
    }
}
