using System;
using System.Collections.Generic;

namespace Cherry.Shared
{
    public interface ICherryPlugin
    {
        void MessageHandler(Message m);

        void SetMessageSendTarget(MessageHandler m);
    }
}
