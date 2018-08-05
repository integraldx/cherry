using System;
using System.Collections.Generic;
using System.Text;
using Cherry.Network;

namespace Cherry.Service
{
    class ServiceManager
    {
        ChannelStream managerStream;
        List<Service> serviceList = new List<Service>();
        IRCHandler ircHandler;

        public ServiceManager(IRCHandler ircHandler)
        {
            this.ircHandler = ircHandler;
            managerStream = ircHandler.Connect();

            managerStream.ToReadEvent += PingResponse;
            managerStream.ToReadEvent += Echo;
        }
        

        public void AssignNewServiceToChannel(string channel)
        {
            ChannelStream channelStream = ircHandler.Join(channel);
            serviceList.Add(new Service(channelStream));
        }

        void PingResponse(Message message)
        {
            if(message.command == Command.PING)
            {
                Message msg = new Message();
                msg.command = Command.PONG;
                msg.commandArgs = message.commandArgs;
                managerStream.WriteMessage(msg);
            }
        }

        void Echo(Message message)
        {
            Console.WriteLine(message.origStr);
        }

    }
}
