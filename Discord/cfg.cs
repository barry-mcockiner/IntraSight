using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace client.Discord
{
    public class InteractionHandlingService : IHostedService
    {
        private readonly DiscordSocketClient _discord;
        private readonly InteractionService _interactions;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _config;
        private readonly ILogger<InteractionService> _logger;

        public InteractionHandlingService(
            DiscordSocketClient discord,
            InteractionService interactions,
            IServiceProvider services,
            IConfiguration config,
            ILogger<InteractionService> logger)
        {
            _discord = discord;
            _interactions = interactions;
            _services = services;
            _config = config;
            _logger = logger;

            _interactions.Log += msg => LogHelper.OnLogAsync(_logger, msg);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string entropy = Environment.MachineName + WindowsIdentity.GetCurrent().User.Value;
            byte[] hashBytes;
            using (var sha = SHA256.Create())
            {
                hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(entropy));
            }

            int seed = BitConverter.ToInt32(hashBytes, 0);
            Random rng = new Random(seed);
            Settings.client_id = rng.Next(100000, 999999); //differenciate clientid

            _discord.InteractionCreated += OnInteractionAsync;

            await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _discord.Ready += async () =>
            {
                Console.WriteLine("Bot is ready. Registering slash commands...");
                await _interactions.RegisterCommandsToGuildAsync(1399940339246039051, true);
                var channel = _discord.GetChannel(new byte[] { 0x00, 0xD2, 0x1F, 0x3B, 0xCF} ) as IMessageChannel; //convienent - best way to notify of new client
                if (channel != null)
                {
                    await channel.SendMessageAsync($"A new client has connected with id {Settings.client_id}");
                }
            };
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _interactions.Dispose();
            return Task.CompletedTask;
        }

        private async Task OnInteractionAsync(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(_discord, interaction);
                var result = await _interactions.ExecuteCommandAsync(context, _services);

                if (!result.IsSuccess)
                    await context.Channel.SendMessageAsync(result.ToString());
            }
            catch
            {
                if (interaction.Type == InteractionType.ApplicationCommand)
                {
                    await interaction.GetOriginalResponseAsync()
                        .ContinueWith(msg => msg.Result.DeleteAsync());
                }
            }
        }
    }
}