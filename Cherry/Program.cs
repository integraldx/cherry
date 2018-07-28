using System;

namespace Cherry
{
    class Program
    {
        static void Main(string[] args)
        {
            Network.IRCHandler handler = new Network.IRCHandler("irc.uriirc.org:16667");
            handler.Connect("cherry", "cherry");

            handler.StartWrite();
            handler.StartRead();
            //Console.WriteLine(handler.Read());
            //Console.WriteLine(handler.Read());
            //Console.WriteLine(handler.Read());

            Network.ChannelStream chStream =  handler.Join("#botTestintint");

            
            while (true)
            {
                chStream.WriteMessage(Console.ReadLine());
            }
        }
    }
}
 