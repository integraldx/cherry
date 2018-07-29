using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


namespace Cherry.Network
{
    class NetworkHandler
    {
        /// <summary>
        /// Handles Tcp network connection
        /// </summary>
        
        protected IPAddress ipAddress;
        protected TcpClient tcpClient;
        protected int targetPort;
        protected SslStream sslStream;
        protected IPHostEntry hostEntry;
        protected readonly string localHostName = Dns.GetHostName();

        

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
            if(!int.TryParse(urlInfo[1], out targetPort) || !(0 < targetPort && targetPort < 65535))
            {
                FormatException formatException = new FormatException("Invalid Port number.");
            }
            hostEntry = Dns.GetHostEntry(urlInfo[0]);
        }

        public void Connect()
        {
            foreach (IPAddress addr in hostEntry.AddressList)
            {
                try
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(addr, targetPort);
                    sslStream = new SslStream(tcpClient.GetStream(), false, validateCertificate, null);
                    sslStream.AuthenticateAsClient(hostEntry.HostName);
                    if (sslStream.IsAuthenticated)
                    {
                        Console.WriteLine("SSL connection established.");
                        return;
                    }
                }
                catch (Exception e)
                {

                    tcpClient.Close();
                    tcpClient = null;
                }
            }
            Console.WriteLine("SSL connection failed.");
            throw new Exception();
        }
        private bool validateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            /*
             * 일단 지금은 인증서에 대한 무조건적인 신뢰
             */
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
        
        public void Disconnect()
        {
            sslStream.Close();
        }
    }
}
