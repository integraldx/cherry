using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry.Network
{
    class Message
    {
        public bool isOperation;
        public string writer;
        public string content;
        public string channel;
        public DateTime timeStamp;
        public Message(string targetChannel, string contentToSend)
        {
            channel = targetChannel;
            content = contentToSend;
        }
    }
}
