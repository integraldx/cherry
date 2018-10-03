using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net.NetworkInformation;
using System.Threading;
namespace Cherry.Network
{
    class IRCHandler
    {
        string userName;
        public string NickName { get; private set; }
        NetworkHandler networkHandler;
        public Dictionary<string, ChannelStream> channels = new Dictionary<string, ChannelStream>();
        ChannelStream managerStream;
        Queue<Message> writeQueue = new Queue<Message>();

        public IRCHandler(NetworkHandler networkHandler, string userName, string nickName)
        {
            this.networkHandler = networkHandler;
            this.userName = userName;
            this.NickName = nickName;
        }

        public ChannelStream Connect()
        {
            Console.WriteLine("Connecting to " + networkHandler.hostName);
            networkHandler.Connect();

            managerStream = new ChannelStream("manager", this);

            networkHandler.Write("USER guest 0 * :" + userName + "\n");
            networkHandler.Write("PASS test\n");
            networkHandler.Write("NICK " + NickName + "\n");
            StartWrite();
            StartRead();

            return managerStream;
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
                if (writeQueue.Count > 0)
                {
                    Message messageToSend = writeQueue.Dequeue();
                    string stringToSend = messageToSend.ToString();

                    networkHandler.Write(stringToSend + "\n");
                    Console.WriteLine(stringToSend);

                    Thread.Sleep(100);
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
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
                string str = networkHandler.Read();
                string[] messages = str.Split('\n');
                foreach(string messageFromStream in messages)
                {

                    Message msgToChannels = Message.ToMessage(messageFromStream.Trim('\r'));
                    if (msgToChannels != null)
                    {
                        if (msgToChannels.channel == string.Empty || msgToChannels.channel == "!Manager" || !channels.ContainsKey(msgToChannels.channel))
                        {
                            managerStream.InvokeReadBehavior(msgToChannels);
                        }
                        else
                        {
                            if (msgToChannels.speakerNickName != NickName)
                            {
                                channels[msgToChannels.channel].InvokeReadBehavior(msgToChannels);
                            }
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
            Message message = new Message();
            message.command = Command.JOIN;
            message.channel = channel;

            if (!channels.ContainsKey(channel))
            {
                channels.Add(channel, new ChannelStream(channel, this));
            }
            this.writeQueue.Enqueue(message);
            return channels[channel];
        }

        public void EnqueueMessage(Message message)
        {
            writeQueue.Enqueue(message);
        }
        public void Disconnect()
        {
            networkHandler.Disconnect();
        }

        void RemoveChannel(string channel)
        {
            if (channels.ContainsKey(channel))
            {
                channels.Remove(channel);
            }
        }
    }
}
