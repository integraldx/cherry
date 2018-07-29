using System;
using Cherry.Network;
using Cherry.Service;

namespace Cherry
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceManager serviceManager = new ServiceManager(new IRCHandler(new NetworkHandler("irc.uriirc.org:16667"), "cherry", "Cherry"));
            
            while (true)
            {
            }
        }
    }
}
 