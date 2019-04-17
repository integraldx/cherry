using System;

namespace Cherry.Shared.Plugins
{
    public class Hello : ICherryPlugin
    {
        MessageHandler sendTarget;
        void ICherryPlugin.MessageHandler(Message m)
        {
            Message send = new Message();
            if(m.Content == "!ping")
            {
                send.speakingChannel = m.speakingChannel;
                send.Content = "pong!";
            }

            sendTarget.Invoke(send);
        }

        void ICherryPlugin.SetMessageSendTarget(MessageHandler m)
        {
            sendTarget += m;
        }
    }
}
