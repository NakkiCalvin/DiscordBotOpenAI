using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using EtoZheDiscordBotAI.ModuleAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EtoZheDiscordBotAI.Modules
{
    internal sealed class Bot : IBot
    {
        private readonly IConfiguration _configuration;
        private readonly IOpenAIHandler _openAIHandler;

        public Bot(IConfiguration configuration, IOpenAIHandler openAIHandler)
        {
            _configuration = configuration;
            _openAIHandler = openAIHandler;
        }

        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public SlashCommandsExtension SlashCommands { get; private set; }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var settings = _configuration.GetRequiredSection("Settings").Get<Config>();

            var config = new DiscordConfiguration
            {
                Token = settings.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
                Intents = DiscordIntents.All
            };

            Client = new DiscordClient(config);
            Client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new[] { settings.Prefix },
                EnableMentionPrefix = true,
                EnableDms = false,
                DmHelp = true,
            };

            SlashCommands = Client.UseSlashCommands(new SlashCommandsConfiguration
            {
                Services = new ServiceCollection()
                .AddSingleton(_openAIHandler)
                .BuildServiceProvider()
            });
            SlashCommands.RegisterCommands<SlashCommands>();
            SlashCommands.SlashCommandErrored += OnSlashError;

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<Commands>();

            await Client.ConnectAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private async Task OnSlashError(SlashCommandsExtension s, SlashCommandErrorEventArgs e)
        {
            await e.Context.Channel.SendMessageAsync(e.Exception.Message);

            //await e.Context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource).ConfigureAwait(false);
            //await e.Context.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"```GG```")).ConfigureAwait(false);
        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
