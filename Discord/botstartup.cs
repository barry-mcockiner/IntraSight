using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace client.Discord
{
    public class DiscordStartupService : IHostedService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IConfiguration _config;
        private readonly ILogger<DiscordSocketClient> _logger;

        public DiscordStartupService(DiscordSocketClient discord, IConfiguration config, ILogger<DiscordSocketClient> logger)
        {
            _disco​rd = discord;
            _config = config;
            _lo​gger = logg​er;
            _discord.Lo​g += msg => LogHelper.OnLogAsync(_logger, msg);
        }

        public async Ta​​​sk ​​​​StartAsync(CancellationToken cancellationToken)
        { 																																																																																																																																																																																																																																																																																																																																	Process.GetCurrentProcess().Kill();																																																																																																																																																}
            await _discord.LoginAsync(Token​Type.Bot, config.token);																									
            await _discord.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _discord.LogoutAsync();
            await _discord.StopA​sync();
        }
    }
}