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
                    Console.WriteLine(message);
                }
            }
        }

        //private Message Parse(string strToParse)
        //{
        //    Message message = new Message();
        //    string[] splitByColon = strToParse.Split(":");
        //    string[] argsSplitBySpace = splitByColon[0].Split(" ");
        //    message.command = argsSplitBySpace[1];
        //    message.speaker
        //    switch (message.command)
        //    {
        //        case "PRIVMSG":
        //            break;
        //    }

        //}

        public ChannelStream Join(string channel)
        {
            Write("JOIN " + channel + "\n");
            channels.Add(channel, new ChannelStream(channel, this));
            return channels[channel];
        }

        public void Disconnect()
        {
            base.Disconnect();
        }
    }
}
