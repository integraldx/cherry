using System;
using System.Reflection;

namespace Cherry
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings s = new Settings();
            PluginManager.LoadPlugins(s);

            Discord.Discord d = new Discord.Discord(s.BotToken);
            d.addMessageHandler(PluginManager.GetPluginMessageHandlers());
            PluginManager.SetPluginsMessageSendTarget(d.sendMessage);
            d.MainAsync().GetAwaiter().GetResult();
        }
    }
}
 
