using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net.NetworkInformation;
using System.Net;

namespace Cherry.Network
{
    class NetworkHandler
    {
        /// <summary>
        /// Handles Tcp network connection
        /// </summary>
        
        protected NetworkStream networkStream;
        protected IPAddress ipAddress;
        protected TcpClient tcpClient = new TcpClient();
        protected int targetPort;
        protected bool isConnectionEstablished;

        

        /// <summary>
        /// Initalizes NetworkHandler object with ip and port
        /// </summary>
        /// <param name="ip"></param>
        /// Target host's ip address
        /// <param name="port"></param>
        /// Target host's port number
        public NetworkHandler(string ip, int port)
        {
            

            if(!IPAddress.TryParse(ip, out ipAddress))
            {
                FormatException formatException = new FormatException("Invalid IP address.");
                throw formatException;
            }

            if (!(0 < port && port < 65535))
            {
                FormatException formatException = new FormatException("Invalid Port number.");
                throw formatException;
            }
            else
            {
                targetPort = port;
            }
        }
        public void Connect()
        {
            try
            {
                tcpClient.Connect(ipAddress, targetPort);
            }
            catch
            {

            }
        }

        public void Write()
        {

        }

        public string Read()
        {

        }
    }
}
