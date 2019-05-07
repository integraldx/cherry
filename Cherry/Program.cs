using System;
using Cherry.Shared;
using System.Reflection;

namespace Cherry
{
    class Program
    {
        static Discord.Discord d;
        static void Main(string[] args)
        {
            Settings s = new Settings();
            PluginManager.LoadPlugins(s);

            d = new Discord.Discord(s.BotToken);
            d.addMessageHandler(PluginManager.GetPluginMessageHandlers());
            PluginManager.SetPluginsMessageSendTarget(d.sendMessage);
            PluginManager.FeedSettingsToPlugins(s);
            d.MainAsync();

            while(true)
            {
                bool terminateFlag = false;
                string inputContent = Console.ReadLine();
                string command = inputContent.Split(' ')[0];
                switch(command)
                {
                    case "reload":
                        s = new Settings();
                        PluginManager.LoadPlugins(s);
                        d.resetMessageHandler(PluginManager.GetPluginMessageHandlers());
                        PluginManager.SetPluginsMessageSendTarget(d.sendMessage);
                        PluginManager.FeedSettingsToPlugins(s);
                        break;
                    case "quit":
                        d.logout();
                        terminateFlag = true;
                        break;
                }

                if (terminateFlag)
                {
                    break;
                }
            }
        }
    }
}
 
