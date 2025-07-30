using Discord.Interactions;
using Discord.WebSocket;
using client.Discord;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;




using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        //config.AddJsonFile("cfg.json"); obsolete and not needed as token is hardcoded
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton(x =>
            new InteractionService(x.GetRequiredService<DiscordSocketClient>())
        );
        services.AddHostedService<InteractionHandlingService>();
        // Add the slash command handler
        services.AddHostedService<DiscordStartupService>();         // Add the discord startup service
    })
    .Build();

await host.RunAsync();

internal class Settings
{
    public static int client_id { get; set; } //used for _amIRequestedClient
}