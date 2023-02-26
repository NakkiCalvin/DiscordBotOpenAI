namespace EtoZheDiscordBotAI.ModuleAbstractions
{
    internal interface IBot
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
