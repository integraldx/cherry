using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry.Network
{
    class ChannelStream
    {
        string channelName;
        IRCHandler handler;
        public delegate void OnRead(Message message);
        public event OnRead ToReadEvent;
        public ChannelStream(string channel, IRCHandler ircHandler)
        {
            handler = ircHandler;
            channelName = channel;
            ToReadEvent += Echo;
        }

        public void WriteMessage(string content)
        {
            handler.writeQueue.Enqueue(new Message("PRIVMSG" ,channelName, content));
        }

        public void InvokeReadBehavior(Message message)
        {
            ToReadEvent.Invoke(message);
        }

        private void Echo(Message message)
        {
            Console.WriteLine(message.speakerNickName + " : " + message.content);
        }
    }
}
