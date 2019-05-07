using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Cherry.Shared.Plugins.Weather
{
    public class Weather : ICherryPlugin
    {
        MessageHandler sendTarget;
        string apiToken;
        void ICherryPlugin.MessageHandler(Message m)
        {
            if (m.Content.StartsWith("!날씨"))
            {
                string region = GetRegionIdFromRegionName(m.Content.Split(' ')[1]);
                Message sndM = new Message();
                sndM.speakingChannel = m.speakingChannel;
                HttpClient httpClient = new HttpClient();
                var request = httpClient.GetAsync(new Uri("http://api.openweathermap.org/data/2.5/weather?id=" + region + "&appid=" + apiToken)).GetAwaiter().GetResult();
                string str = request.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                JObject jsonObj = JObject.Parse(str);
                sndM.Content = jsonObj.SelectToken("weather").ToString();
                sendTarget.Invoke(sndM);
            }
        }

        Dictionary<string, string>  SearchAndGetWeatherData(string regionName)
        { 
            Dictionary<string, string> result = new Dictionary<string, string>();

            return result;
        }

        string GetRegionIdFromRegionName(string name)
        {
            return "1835847";
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
