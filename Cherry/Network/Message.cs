using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry.Network
{
    enum Command { PRIVMSG, MODE, PING, PONG, INVITE, TOPIC, LIST, NAMES, NOTICE, JOIN};
    class Message
    {
        public string origStr;
        public Command command;
        public List<string> commandArgs = new List<string>();
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
        public Message(Command command, string targetChannel, string content)
        {
            this.command = command;
            channel = targetChannel;
            this.content = content;
        }

        override public string ToString()
        {
            string returnStr = "";
            switch (command)
            {
                case Command.PRIVMSG:
                    returnStr = "PRIVMSG " + channel + " :" + content;
                    break;
                case Command.JOIN:
                    returnStr = "JOIN " + channel;
                    break;
                case Command.PONG:
                    returnStr = "PONG :" + commandArgs[0];
                    break;
            }
            return returnStr;
        }

        public static Message ToMessage(string fromString)
        {

            if (fromString == String.Empty)
            {
                return null;
            }
            Message message = new Message();
            message.origStr = fromString;
            var strSplitBySpace = fromString.Split(' ');
            if (strSplitBySpace[0][0] == ':')
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

                string commandInStr = strSplitBySpace[1];

                switch (commandInStr)
                {
                    case "PRIVMSG":
                        message.command = Network.Command.PRIVMSG;
                        message.channel = strSplitBySpace[2];
                        message.content = strSplitBySpace[3].Trim(':');
                        break;
                    case "MODE":
                        message.command = Network.Command.MODE;
                        break;
                }

            }
            else
            {
                string commandInStr = strSplitBySpace[0];
                switch (commandInStr)
                {
                    case "PING":
                        message.command = Network.Command.PING;
                        message.channel = "!Manager";
                        message.commandArgs.Add(strSplitBySpace[1].Trim(':'));
                        break;
                }
            }
            return message;
        }
    }
}
