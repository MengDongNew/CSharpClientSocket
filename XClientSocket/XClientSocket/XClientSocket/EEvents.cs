using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XClientSocket.XClientSocket
{
    public enum EEvents
    {
        Event_LoginHeart =11,//大厅服务器心跳包
        Event_GameHeart = 12,//游戏服务器心跳包
        Event_ServerTime = 99,//服务器时间



        Last

    }
}
