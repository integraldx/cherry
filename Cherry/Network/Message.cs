using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry.Network
{
    class Message
    {
        public string command = String.Empty;
        public string[] commmandArgs;
        public string speakerNickName;
        public string speakerRealName;
        public string content;
        public string channel = String.Empty;
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
