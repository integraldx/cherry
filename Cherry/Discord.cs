using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Cherry.Discord
{
    class Discord
    {
        private readonly DiscordSocketClient discordClient;
        private string botLoginToken;
        private event MessageHandler messageHandler;
        Discord(string token)
        {
            botLoginToken = token;
            discordClient.MessageReceived += MessageReceived;
        }

        public void addMessageHandler(MessageHandler func)
        {
            messageHandler += func;
        }

        private void LoginAsBot()
        {
            discordClient.LoginAsync(TokenType.Bot, botLoginToken);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            messageHandler.BeginInvoke(parseDiscordMessage(message), null, null);
        }

        private async Task sendMessage(Message m)
        {
            var channel = (ISocketMessageChannel)discordClient.GetChannel(ulong.Parse(m.speakingChannel.id));
            await channel.SendMessageAsync(m.Content);
        }

        private Message parseDiscordMessage(SocketMessage s)
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
