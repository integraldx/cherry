using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net.NetworkInformation;
using System.Threading;

namespace Cherry.Network
{
    class IRCHandler : NetworkHandler
    {
        string userName;
        string realName;
        Dictionary<string, ChannelStream> channels = new Dictionary<string, ChannelStream>();
        public Queue<Message> writeQueue = new Queue<Message>();
        Queue<Message> readQueue;

        public IRCHandler(string ip, int port) : base(ip, port)
        {
            
        }

        public IRCHandler(string url) : base(url)
        {

        }

        public void Connect(string userName, string nickName)
        {
            Console.WriteLine("Connecting to " + base.hostEntry.HostName);
            base.Connect();

            base.Write("USER guest 0 * :" + userName + "\n");
            base.Write("PASS test\n");
            base.Write("NICK " + nickName + "\n");

        }

        public void Write(string content)
        {
            base.Write(content);
        }

        public void StartWrite()
        {
            Thread writerThread = new Thread(new ThreadStart(ThreadedWrite));
            writerThread.Start();


        }

        private void ThreadedWrite()
        {
            while (true)
            {
                if(writeQueue.Count > 0)
                {
                    Message messageToSend = writeQueue.Dequeue();
                    string stringToSend = "";
                    if (messageToSend.command == "PRIVMSG")
                    {
                        stringToSend += "PRIVMSG " + messageToSend.channel + " :" + messageToSend.content + "\n";
                        Write(stringToSend);
                    }
                }
            }
        }

        public void Command()
        {

        }

        public string Read()
        {
            return base.Read();
        }

        public void StartRead()
        {
            Thread readerThread = new Thread(new ThreadStart(ThreadedRead));
            readerThread.Start();
        }
        private void ThreadedRead()
        {
            while (true)
            {
                string str = Read();
                string[] messages = str.Split('\n');
                foreach(string message in messages)
                {
                    Message msgToChannels = strToMessage(message);
                    if (msgToChannels != null)
                    {
                        if (msgToChannels.channel == string.Empty)
                        {
                            Console.WriteLine(message);
                        }
                        else
                        {
                            channels[msgToChannels.channel].InvokeReadBehavior(msgToChannels);
                        }
                    }
                }
            }
        }
        

        public ChannelStream Join(string channel)
        {
            Write("JOIN " + channel + "\n");
            channels.Add(channel, new ChannelStream(channel, this));
            return channels[channel];
        }

        public Message strToMessage(string str)
        {
            if(str == String.Empty)
            {
                return null;
            }
            Message message = new Message();
            var strSplitBySpace = str.Split(' ');
            if(strSplitBySpace[0][0] == ':')
            {
                var userNames = strSplitBySpace[0].Trim(':').Split('!');
                if (userNames.Length > 1)
                {
                    message.speakerNickName = userNames[0];
                    message.speakerRealName = userNames[1];
                }
                else
                {
                    message.speakerRealName = userNames[0];
                }

                message.command = strSplitBySpace[1];

                switch (message.command)
                {
                    case "PRIVMSG":
                        message.channel = strSplitBySpace[2];
                        message.content = strSplitBySpace[3].Trim(':');
                        break;
                    case "MODE":
                        break;
                }
                
            }
            else
            {
                message.command = strSplitBySpace[0];
                switch (message.command)
                {
                    case "PING":
                        message.commmandArgs[0] = strSplitBySpace[1];
                        break;
                }
            }
            return message;
        }
        public void Disconnect()
        {
            base.Disconnect();
        }
    }
}
