using System;
using System.Collections.Generic;
using System.Text;
using Cherry.Network;

namespace Cherry.Service
{
    class ServiceManager
    {
        ChannelStream managerStream;
        Dictionary<ChannelStream, Service> serviceList = new Dictionary<ChannelStream, Service>();
        IRCHandler ircHandler;

        public ServiceManager(IRCHandler ircHandler)
        {
            this.ircHandler = ircHandler;
            managerStream = ircHandler.Connect();

            managerStream.NewMessageFromChannelEvent += PingResponse;
            managerStream.NewMessageFromChannelEvent += Echo;
            managerStream.NewMessageFromChannelEvent += HandleChannelNames;
            managerStream.NewMessageFromChannelEvent += JoinInvitedChannel;
        }

        void HandleChannelNames(Message message)
        {

            if(message.origStr.Split(' ').Length > 5 && message.origStr.Split(' ')[1] == "353")
            {
                ChannelStream channel = ircHandler.channels[message.origStr.Split(' ')[4]];
                int iter0 = 5;
                while (true)
                {
                    try
                    {
                        User user = User.Parse(message.origStr.Split(' ')[iter0].TrimStart(':'));
                        channel.AssignNewUserInManagmentList(user);
                        iter0++;
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        break;
                    }
                }
            }
        }

        void PingResponse(Message message)
        {
            if(message.command == Command.PING)
            {
                Message msg = new Message();
                msg.command = Command.PONG;
                msg.commandArgs = message.commandArgs;
                managerStream.WriteMessage(msg);

                foreach (var channel in ircHandler.channels)
                {
                    if (channel.Key == "manager")
                    {
                        break;
                    }
                    Message channelName = new Message();
                    channelName.command = Command.NAMES;
                    channelName.channel = channel.Key;
                    managerStream.WriteMessage(channelName);
                }
            }
            
        }

        void Echo(Message message)
        {
            Console.WriteLine(message.origStr);
        }

        void JoinInvitedChannel(Message message)
        {
            if(message.command == Command.INVITE)
            {
                AssignNewServiceToChannel(message.commandArgs[1]);
            }
        }

        public void AssignNewServiceToChannel(string channel)
        {
            ChannelStream channelStream = ircHandler.Join(channel);
            if (!serviceList.ContainsKey(channelStream))
            {
                serviceList.Add(channelStream, new Service(channelStream));
            }
        }
    }
}
