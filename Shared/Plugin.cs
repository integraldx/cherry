using System;

namespace Cherry.Shared
{
    public interface ICherryPlugin
    {
        void MessageHandler(Message m);

        void SetMessageSendTarget(MessageHandler m);
    }
    public class Plugin
    {
    }
}
