using EtoZheDiscordBotAI.ModuleAbstractions;
using EtoZheDiscordBotAI.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CancellationTokenSource cts = new();

var configuration = new ConfigurationBuilder()
     .AddJsonFile($"config.json");

var config = configuration.Build();

var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(config)
            .AddSingleton<IBot, Bot>()
            .AddSingleton<IOpenAIHandler, OpenAIHandler>()
            .BuildServiceProvider();

var bot = serviceProvider.GetService<IBot>();

bot!.RunAsync(cts.Token).GetAwaiter().GetResult();

cts.Cancel();