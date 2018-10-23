using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry.Network
{
    class ChannelStream
    {
        string channelName;
        public string SelfName
        {
            get
            {
                return handler.NickName;
            }
        }
        IRCHandler handler;
        public Dictionary<string, User> users = new Dictionary<string, User>();
        public delegate void OnRead(Message message);
        public event OnRead NewMessageFromChannelEvent;
        public ChannelStream(string channel, IRCHandler ircHandler)
        {
            handler = ircHandler;
            channelName = channel;
            NewMessageFromChannelEvent += ManageChannelUsers;
        }

        public void WriteMessage(Message message)
        {
            handler.EnqueueMessage(message);
        }

        public void InvokeReadBehavior(Message message)
        {
            NewMessageFromChannelEvent.Invoke(message);
        }

        private void ManageChannelUsers(Message message)
        {
            if(message.command == Command.JOIN)
            {
                AssignNewUserInManagmentList(User.Parse(message.speakerNickName));
            }
            else if(message.command == Command.PART)
            {
                RemoveUserFromManagmentList(message.speakerNickName);
            }
            else if(message.command == Command.KICK)
            {
                RemoveUserFromManagmentList(message.commandArgs[0]);

                if(message.commandArgs[0] == handler.NickName)
                {
                    
                }
            }
            else if (message.command == Command.MODE)
            {
                if (users.ContainsKey(message.commandArgs[1]))
                {                                   
                    if (message.commandArgs[0] == "+o")
                    {                               
                        users[message.commandArgs[1]].isOp = true;
                                                    
                    }                               
                    else if (message.commandArgs[0] == "-o")
                    {                               
                        users[message.commandArgs[1]].isOp = false;
                    }                               
                }
                else
                {
                    Console.WriteLine("User not found!");
                }
                                                    
            }                                       
        }                 
        
        public void AssignNewUserInManagmentList(User user)
        {
            if (!users.ContainsKey(user.nickName))
            {
                users.Add(user.nickName, user);
            }
            else if(users[user.nickName].isOp != user.isOp)
            {
                users[user.nickName].isOp = user.isOp;
            }
            
        }
        public void RemoveUserFromManagmentList(string userNick)
        {
            if (userNick.StartsWith('@'))
            {
                userNick = userNick.TrimStart('@');
            }
            if (users.ContainsKey(userNick))
            {
                users.Remove(userNick);
            }
            else
            {
                Console.WriteLine($"{userNick} not found in users list.");
            }
        }
        

    }
}
