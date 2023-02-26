using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace EtoZheDiscordBotAI.Modules
{
    internal sealed class Commands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns answer from bot")]
        //[RequireRoles(RoleCheckMode.Any)]
        public async Task Ping(CommandContext context)
        {
            await context.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }
    }
}
