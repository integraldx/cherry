using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


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
        protected SslStream sslStream;
        protected IPHostEntry hostEntry = new IPHostEntry();

        

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
            hostEntry.HostName = urlInfo[0];
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
                //networkStream = tcpClient.GetStream();
                sslStream = new SslStream(tcpClient.GetStream(), false, validateCertificate, null);
                sslStream.AuthenticateAsClient(hostEntry.HostName);
            }
            catch (Exception e)
            {

            }
        }
        private bool validateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void Write(string content)
        {
            try
            {
                byte[] bytesToWrite = Encoding.UTF8.GetBytes(content);
                sslStream.Write(bytesToWrite, 0, bytesToWrite.Length);
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
                int readLength = sslStream.Read(bytesToRead, 0, 4096);
                strToReturn = Encoding.UTF8.GetString(bytesToRead, 0, readLength);
            }
            catch (Exception e)
            {
                throw e;
            }
            return strToReturn;
        }
        
    }
}
