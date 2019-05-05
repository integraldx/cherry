using System;

namespace Cherry.Shared.Plugins
{
    public class Hello : ICherryPlugin
    {
        MessageHandler sendTarget;
        void ICherryPlugin.MessageHandler(Message m)
        {
            Message send = new Message();
            if(m.Content == "!체리 안녕")
            {
                send.speakingChannel = m.speakingChannel;
                send.Content = "안녕하세요!";
            }

            sendTarget.Invoke(send);
        }

        void ICherryPlugin.SetMessageSendTarget(MessageHandler m)
        {
            sendTarget += m;
        }

        void ICherryPlugin.SetRequiredSettings(Settings s)
        {
            return;
        }
    }
}
