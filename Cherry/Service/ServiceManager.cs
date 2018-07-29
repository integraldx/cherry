using System;
using System.Collections.Generic;
using System.Text;
using Cherry.Network;

namespace Cherry.Service
{
    class ServiceManager
    {
        ChannelStream managerStream;
        IRCHandler ircHander;
        public ServiceManager(IRCHandler ircHandler)
        {
            this.ircHander = ircHandler;
            managerStream = ircHander.Connect();

            managerStream.ToReadEvent += PingResponse;
            managerStream.ToReadEvent += Echo;
        }

        public void Echo(Message message)
        {
            Console.WriteLine(message.origStr);
        }

        public void PingResponse(Message message)
        {
            if(message.command == Command.PING)
            {
                Message msg = new Message();
                msg.command = Command.PONG;
                msg.commandArgs = message.commandArgs;
                managerStream.WriteMessage(msg);
            }
        }
    }
}
