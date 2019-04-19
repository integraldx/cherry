using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Cherry
{
    class Settings
    {
        public string BotToken { get; private set; }
        private List<string> pluginNames = new List<string>();

        public Settings()
        {
            FileStream settingsFileStream;
            if(File.Exists("./config.json"))
            {
                settingsFileStream = File.OpenRead("./config.json");
            }
            else
            {
                settingsFileStream = File.Create("./config.json");
            }

            byte[] byteArray = new byte[8192];
            settingsFileStream.Read(byteArray, 0, 8192);
            settingsFileStream.Close();


            var jsonObject = JObject.Parse(Encoding.UTF8.GetString(byteArray));
            BotToken = jsonObject.GetValue("BotToken").ToString();
            var pluginArrs = jsonObject.GetValue("Plugins");
            foreach(var i in pluginArrs)
            {
                pluginNames.Add(i.ToString());
                Console.WriteLine(i.ToString());
            }
            
        }

        public void SetBotToken(string token)
        {
            BotToken = token;
        }

        public string[] GetPluginNames()
        {
            return pluginNames.ToArray();
        }
    }
}
