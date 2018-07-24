using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net.NetworkInformation;

namespace Cherry.Network
{
    class IRCHandler : NetworkHandler
    {
        string userName;
        string realName;
        string[] channels;

        public IRCHandler(string ip, int port) : base(ip, port)
        {
            
        }

        public IRCHandler(string url) : base(url)
        {

        }

        public void Connect(string userName, string nickName)
        {
            base.Connect();
            
            base.Write("USER guest 0 * :" + userName + "\n");
            base.Write("PASS test\n");
            base.Write("NICK " + nickName + "\n");

        }

        public void Write()
        {

        }

        public void Command()
        {

        }

        public string Read()
        {
            return base.Read();
        }

        public void Disconnect()
        {

        }
    }
}
