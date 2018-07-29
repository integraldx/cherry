using System;
using Cherry.Network;

namespace Cherry
{
    class Program
    {
        static void Main(string[] args)
        {
            IRCHandler handler = new IRCHandler("irc.uriirc.org:16667");
            handler.Connect("cherry", "cherry");

            ChannelStream chStream =  handler.Join("#botTestintint");
            
            while (true)
            {
                chStream.WriteMessage(Console.ReadLine());
            }
        }
    }
}
 