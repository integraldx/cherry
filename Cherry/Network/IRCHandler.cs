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

        public void Connect()
        {

        }

        public void Write()
        {

        }

        public void Command()
        {

        }

        public void Read()
        {

        }

        public void Disconnect()
        {

        }
    }
}
