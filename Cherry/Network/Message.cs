using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry.Network
{
    class Message
    {
        public string command;
        public string speaker;
        public string content;
        public string channel;
        public DateTime timeStamp;
        public Message()
        {

        }
        public Message(string targetChannel, string content)
        {
            channel = targetChannel;
            this.content = content;
        }
        public Message(string command, string targetChannel, string content)
        {
            this.command = command;
            channel = targetChannel;
            this.content = content;
        }
    }
}
