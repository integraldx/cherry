using System;
using System.Collections.Generic;

namespace Cherry.Shared.Plugins.Weather
{
    public class Weather : ICherryPlugin
    {
        MessageHandler sendTarget;
        string apiToken;
        void ICherryPlugin.MessageHandler(Message m)
        {
            if(m.Content.StartsWith("!날씨"))
            {
                Console.WriteLine("Invoked");
                var result = SearchAndGetWeatherData(m.Content.Split()[1]);

                foreach(var tok in result)
                {
                    Message sndM = new Message();
                    sndM.Content = tok.Key + ":" + tok.Value;
                    sndM.speakingChannel = m.speakingChannel;

                    sendTarget.Invoke(sndM);
                }

            }
        }

        Dictionary<string, string>  SearchAndGetWeatherData(string regionName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            return result;
        }

        void ICherryPlugin.SetMessageSendTarget(MessageHandler m)
        {
            sendTarget = m;
        }

        void ICherryPlugin.SetRequiredSettings(Settings s)
        {
            apiToken = s.GetValueByName("weather.token");
        }
    }
}
