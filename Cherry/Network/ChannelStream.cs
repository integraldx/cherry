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
        }

        public void WriteMessage(string content)
        {
            handler.writeQueue.Enqueue(new Message(Command.PRIVMSG ,channelName, content));
        }

        public void WriteMessage(Message message)
        {
            handler.writeQueue.Enqueue(message);
        }

        public void InvokeReadBehavior(Message message)
        {
            ToReadEvent.Invoke(message);
        }
    }
}
