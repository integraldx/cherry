using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry.Network
{
    class User
    {
        public string nickName;
        public string userName;
        public bool isOp;
        

        public static User Parse(string strToParse)
        {
            User user = new User();
            if(strToParse[0] == '@')
            {
                user.isOp = true;
            }
            user.nickName = strToParse.Trim('@');
            return user;
        }
        public string ToFullString()
        {
            if (isOp)
            {
                return ":@" + nickName + "!" + userName;
            }
            else
            {
                return nickName + "!" + userName;
            }
        }

        public string ToSimpleString()
        {
            if (isOp)
            {
                return "@" + nickName;
            }
            else
            {
                return nickName;
            }
        }
    }
}
