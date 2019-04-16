using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry
{
    class Settings
    {
        public string BotToken { get; private set; }

        public Settings()
        {

        }

        public void SetBotToken(string token)
        {
            BotToken = token;
        }


    }
}
