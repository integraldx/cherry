using System;

namespace Cherry
{
    class Program
    {
        static void Main(string[] args)
        {
            Network.IRCHandler handler = new Network.IRCHandler("irc.uriirc.org:16667");
            handler.Connect("cherry", "cherry");

            Console.WriteLine(handler.Read());
            Console.WriteLine(handler.Read());
            Console.WriteLine(handler.Read());
            handler.Join("#Integral");
            handler.StartWrite();
            while (true)
            {
                string str = Console.ReadLine();
                Network.Message message = new Network.Message("#Integral", str);
                handler.writeQueue.Enqueue(message);
            }
        }
    }
}
 