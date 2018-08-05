using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry.Network
{
    class ChannelStream
    {
        string channelName;
        IRCHandler handler;
        Dictionary<string, User> users;
        public delegate void OnRead(Message message);
        public event OnRead ToReadEvent;
        public ChannelStream(string channel, IRCHandler ircHandler)
        {
            handler = ircHandler;
            channelName = channel;
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
