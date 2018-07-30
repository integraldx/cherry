using System;
using System.Collections.Generic;
using System.Text;
using Cherry.Network;

namespace Cherry.Service
{
    class ServiceManager
    {
        ChannelStream managerStream;
        ChannelStream testerStream;
        IRCHandler ircHander;

        public ServiceManager(IRCHandler ircHandler)
        {
            this.ircHander = ircHandler;
            managerStream = ircHander.Connect();

            managerStream.ToReadEvent += PingResponse;
            managerStream.ToReadEvent += Echo;
        }

        public void JoinTester()
        {
            testerStream = ircHander.Join("#botTestintint");

            testerStream.ToReadEvent += Echo;
            testerStream.ToReadEvent += Hello;
        }

        void Echo(Message message)
        {
            Console.WriteLine(message.origStr);
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

        void Hello(Message message)
        {
            if(message.command == Command.PRIVMSG)
            {
                if(message.content.Split(' ')[0] == "체리")
                {
                    if(message.content.Split(' ')[1] == "안녕!")
                    {
                        Message msg = new Message();
                        msg.command = Command.PRIVMSG;
                        msg.content = "안녕하세요 " + message.speakerNickName + ".";
                        msg.channel = message.channel;
                        testerStream.WriteMessage(msg);
                    }
                }
            }
        }
    }
}
