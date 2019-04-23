using System.Threading.Tasks;
using System;
using Discord;
using Discord.WebSocket;
using Cherry.Shared;

namespace Cherry.Discord
{
    class Discord
    {
        private readonly DiscordSocketClient discordClient = new DiscordSocketClient();
        private string botLoginToken;
        private event MessageHandler messageHandler;
        public Discord(string token)
        {
            botLoginToken = token;
            discordClient.MessageReceived += MessageReceived;
        }

        public async Task MainAsync()
        {
            await discordClient.LoginAsync(TokenType.Bot, botLoginToken);
            await discordClient.StartAsync();
            await Task.Delay(-1);
        }

        public void addMessageHandler(MessageHandler func)
        {
            messageHandler += func;
        }

        public void resetMessageHandler(MessageHandler func)
        {
            messageHandler -= messageHandler;
            messageHandler += func;
        }

        private void LoginAsBot()
        {
            discordClient.LoginAsync(TokenType.Bot, botLoginToken);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            Console.WriteLine(message.Content);
            if (message.Author.Id == discordClient.CurrentUser.Id)
                return;

            messageHandler.Invoke(parseDiscordMessage(message));
        }

        public void logout()
        {
            discordClient.StopAsync();
            discordClient.LogoutAsync();
        }

        public void sendMessage(Message m)
        {
            var channel = (ISocketMessageChannel)discordClient.GetChannel(ulong.Parse(m.speakingChannel.id));
            channel.SendMessageAsync(m.Content);
        }

        static private Message parseDiscordMessage(SocketMessage s)
        {
            Message m = new Message();

            m.speakingChannel.id = s.Channel.Id.ToString();
            m.speakingChannel.name = s.Channel.Name;

            m.Author.name = s.Author.Id.ToString();
            m.Author.id = s.Author.Username;

            m.Content = s.Content;

            return m;
        }
    }
}
