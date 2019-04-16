using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry
{
    delegate int MessageHandler(Message m);
    public class Message
    {
        public class User
        {
            public string name;
            public string id;
        }

        public class Channel
        {
            public string name;
            public string id;
        }
        public string Content { get; set; }
        public User Author { get; set; }
        public Channel speakingChannel { get; set; }
    }
}
