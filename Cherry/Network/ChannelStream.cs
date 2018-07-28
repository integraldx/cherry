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
        OnRead onRead;
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

        public void InvokeReadBehavior(string msg)
        {

        }
        private void Echo(Message message)
        {
            Console.WriteLine(message.content);
        }
    }
}
