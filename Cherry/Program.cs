using System;
using Cherry.Network;
using Cherry.Service;

namespace Cherry
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Please Input IRC server's url with port number at arguments");
            }
            ServiceManager serviceManager = new ServiceManager(new IRCHandler(new NetworkHandler(args[0]), "cherry","Cherry"));
        }
    }
}
 
