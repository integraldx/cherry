using System;
using System.Reflection;

namespace Cherry
{
    class Program
    {
        static void Main(string[] args)
        {
            string token;
            Console.Write("Bot Token : ");
            token = Console.ReadLine();
            Console.Write("dll path : ");
            string dllPath = Console.ReadLine();

            Assembly asm = Assembly.LoadFrom(dllPath);

            Type pluginType = typeof(Shared.ICherryPlugin);
            Type targ = typeof(Shared.ICherryPlugin);
            foreach(Type t in asm.GetTypes())
            {
                if(t.IsAbstract || t.IsInterface)
                {
                    continue;
                }
                else if(t.GetInterface(pluginType.FullName) != null)
                {
                    targ = t;
                    break;
                }
            }
            Shared.ICherryPlugin p = (Shared.ICherryPlugin)Activator.CreateInstance(targ);

            Discord.Discord d = new Discord.Discord(token);
            d.addMessageHandler(p.MessageHandler);
            p.SetMessageSendTarget(d.sendMessage);
            d.MainAsync().GetAwaiter().GetResult();
        }
    }
}
 
