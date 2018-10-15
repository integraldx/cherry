using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


namespace Cherry.Network
{
    class NetworkHandler
    {
        /// <summary>
        /// Handles Tcp network connection
        /// </summary>
        
        private IPAddress ipAddress;
        private TcpClient tcpClient;
        private int targetPort;
        private SslStream sslStream;
        private IPHostEntry hostEntry;
        private readonly string localHostName = Dns.GetHostName();
        public string hostName
        {
            get
            {
                return hostEntry.HostName;
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
