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
        string nickName;
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
            this.userName = userName;
            this.nickName = nickName;
            Console.WriteLine("Connecting to " + base.hostEntry.HostName);
            base.Connect();

            base.Write("USER guest 0 * :" + userName + "\n");
            base.Write("PASS test\n");
            base.Write("NICK " + nickName + "\n");
            StartWrite();
            StartRead();
        }

        public void Write(string content)
        {
            base.Write(content);
        }

        private void StartWrite()
        {
            Thread writerThread = new Thread(new ThreadStart(ThreadedWrite));
            writerThread.Start();
        }

        /// <summary>
        /// constantly writes message from 
        /// </summary>
        private void ThreadedWrite()
        {
            while (true)
            {
                if(writeQueue.Count > 0)
                {
                    Message messageToSend = writeQueue.Dequeue();
                    string stringToSend = "";
                    if (messageToSend.command == Network.Command.PRIVMSG)
                    {
                        stringToSend += "PRIVMSG " + messageToSend.channel + " :" + messageToSend.content + "\n";
                        Write(stringToSend);
                    }
                }
            }
        }

        /// <summary>
        /// Manually Reads Data from stream.
        /// Don't use it unless it is urgent.
        /// </summary>
        /// <returns>string data from network</returns>
        public string Read()
        {
            return base.Read();
        }

        /// <summary>
        /// Starts ThreadedRead
        /// </summary>
        private void StartRead()
        {
            Thread readerThread = new Thread(new ThreadStart(ThreadedRead));
            readerThread.Start();
        }

        /// <summary>
        /// Constantly reads data from stream and pass data to ChannelStream object
        /// </summary>
        private void ThreadedRead()
        {
            while (true)
            {
                string str = Read();
                string[] messages = str.Split('\n');
                foreach(string messageFromStream in messages)
                {
                    Message msgToChannels = Message.ToMessage(messageFromStream);
                    if (msgToChannels != null)
                    {
                        if (msgToChannels.channel == string.Empty)
                        {
                            Console.WriteLine(messageFromStream);
                        }
                        else
                        {
                            channels[msgToChannels.channel].InvokeReadBehavior(msgToChannels);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Joins new channel and Get ChannelStream
        /// </summary>
        /// <param name="channel"> Channel's name to join</param>
        /// <returns> ChannelStream which is binded to joined Channel </returns>
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
