using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XClientSocket.XClientSocket
{
    public static partial class App_Event
    {
        /// <summary>
        /// 注册游戏服务器协议
        /// </summary>
        private static void RegGameEvent()
        {
            RegEvent(EEvents.Event_GameHeart, HandleCommon.Handle_GameHeart);
        }
    }
}
