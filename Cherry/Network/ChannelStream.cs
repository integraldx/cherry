using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry.Network
{
    class ChannelStream
    {
        string channelName;
        IRCHandler handler;
        public Dictionary<string, User> users = new Dictionary<string, User>();
        public delegate void OnRead(Message message);
        public event OnRead NewMessageFromChannelEvent;
        public ChannelStream(string channel, IRCHandler ircHandler)
        {
            handler = ircHandler;
            channelName = channel;
            NewMessageFromChannelEvent += ManageChannelUsers;
        }

        public void WriteMessage(Message message)
        {
            handler.EnqueueMessage(message);
        }

        public void InvokeReadBehavior(Message message)
        {
            NewMessageFromChannelEvent.Invoke(message);
        }

        private void ManageChannelUsers(Message message)
        {
            if(message.command == Command.JOIN)
            {
                if (users.ContainsKey(message.speakerNickName))
                {

                }
                else
                {
                    users.Add(message.speakerNickName, User.Parse(message.speakerNickName + "!" + message.speakerRealName));
                    Console.WriteLine("{0} joins channel", message.speakerNickName);
                }

            }
            else if(message.command == Command.PART)
            {
                if (users.ContainsKey(message.speakerNickName))
                {
                    users.Remove(message.speakerNickName);
                    Console.WriteLine("{0} leaves channel", message.speakerNickName);
                }
                else
                {
                    Console.WriteLine("{0} not found in channel user data. WTF?!", message.speakerNickName);
                }
            }
        }
    }
}
