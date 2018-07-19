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

        public NetworkHandler(string url)
        {
            string[] urlInfo = url.Split(':');
            IPHostEntry hostEntry;
            ipAddress = Dns.GetHostAddresses(urlInfo[0])[0];
            if(!int.TryParse(urlInfo[1], out targetPort) || !(0 < targetPort && targetPort < 65535))
            {
                FormatException formatException = new FormatException("Invalid Port number.");
            }
        }

        public void Connect()
        {
            try
            {
                tcpClient.Connect(ipAddress, targetPort);
                networkStream = tcpClient.GetStream();
            }
            catch (Exception e)
            {

            }
        }

        public void Write(string content)
        {
            byte[] bytesToWrite = new byte[4096];

            try
            {
                bytesToWrite.Initialize();
                bytesToWrite = Encoding.UTF8.GetBytes(content);
                networkStream.Write(bytesToWrite, 0, bytesToWrite.Length);
            }
            catch (Exception e)
            {

            }

        }
        
        public string Read()
        {
            byte[] bytesToRead = new byte[4096];
            string strToReturn;
            try
            {
                bytesToRead.Initialize();
                networkStream.Read(bytesToRead, 0, 4096);
                strToReturn = Encoding.UTF8.GetString(bytesToRead);
            }
            catch (Exception e)
            {
                throw e;
            }
            return strToReturn;
        }
        
    }
}
