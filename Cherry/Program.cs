using System;
using Discord;
using Discord.API;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Cherry
{
    class Program
    {
        private readonly DiscordSocketClient _client;
        private static string token;
        static void Main(string[] args)
        {
            token = Console.ReadLine();
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            _client = new DiscordSocketClient();

            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task MainAsync()
        {
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            if (message.Content == "!ping")
                await message.Channel.SendMessageAsync("pong!");
        }
    }
}
 
