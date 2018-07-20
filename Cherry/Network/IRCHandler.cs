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
        
        public IRCHandler(string ip, int port) : base(ip, port)
        {
            
        }
    }
}
