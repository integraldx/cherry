using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Cherry.Shared
{
    public class Settings
    {
        public string BotToken { get; private set; }
        private List<string> pluginNames = new List<string>();
        JObject configJsonContent;

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


            configJsonContent = JObject.Parse(Encoding.UTF8.GetString(byteArray));
            BotToken = configJsonContent.GetValue("BotToken").ToString();
            var pluginArrs = configJsonContent.GetValue("Plugins");
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

        public string GetValueByName(string contentPath)
        {
            JToken tok = configJsonContent.SelectToken("$");
            var a = tok.SelectToken(contentPath);
            return a.ToString();
        }

    }
}
