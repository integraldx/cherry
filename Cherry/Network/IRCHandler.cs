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
        string[] channels;
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
                    if (!messageToSend.isOperation)
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
        public delegate void ReadHandler(object obj, EventArgs eventArgs);
        public event ReadHandler SomethingToRead;
        public void BeginRead()
        {
            while (true)
            {
                
            }
        }

        public ChannelStream Join(string channel)
        {
            Write("JOIN " + channel + "\n");
            return new ChannelStream(channel);
        }
        //public Message Parse(string content)
        //{

        //}
        public void Disconnect()
        {
            base.Disconnect();
        }
    }
}
