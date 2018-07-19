using System;

namespace Cherry
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Network.NetworkHandler networkHandler = new Network.NetworkHandler("irc.uriirc.org:16667");
            networkHandler.Connect();
            networkHandler.Write("PASS testtesttest\n" + 
                "NICK cherry\n" 
                );
            while (true)
            {
                Console.Write(networkHandler.Read());
            }
        }
    }
}
 