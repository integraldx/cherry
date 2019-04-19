using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Cherry
{
    static class PluginManager
    {
        public static List<Shared.ICherryPlugin> pluginList = new List<Shared.ICherryPlugin>();
        public static void LoadPlugins(Settings settings)
        {
            pluginList.Clear();
            if(!Directory.Exists("./Plugins"))
            {
                Directory.CreateDirectory("./Plugins");
            }
            string pluginDirPath = Directory.GetCurrentDirectory() + "/Plugins/";
            foreach(string pluginName in settings.GetPluginNames())
            {
                Assembly asm = Assembly.LoadFrom(pluginDirPath + pluginName + ".dll");
                Type targ = typeof(Shared.ICherryPlugin);
                foreach(Type t in asm.GetTypes())
                {
                    if(t.IsAbstract || t.IsInterface)
                    {
                        continue;
                    }
                    else if(t.GetInterface(typeof(Shared.ICherryPlugin).FullName) != null)
                    {
                        targ = t;
                        break;
                    }
                }
                Shared.ICherryPlugin p = (Shared.ICherryPlugin)Activator.CreateInstance(targ);
                pluginList.Add(p);
            }
        }

        public static Shared.MessageHandler GetPluginMessageHandlers()
        {
            Shared.MessageHandler m = delegate { };
            foreach(var i in pluginList)
            {
                m += i.MessageHandler;
            }

            return m;
        }

        public static void SetPluginsMessageSendTarget(Shared.MessageHandler m)
        {
            foreach(var i in pluginList)
            {
                i.SetMessageSendTarget(m);
            }
        }
    }
}
