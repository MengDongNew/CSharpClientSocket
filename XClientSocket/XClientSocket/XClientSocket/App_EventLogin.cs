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
        /// 注册登陆服务器协议
        /// </summary>
        private static void RegLoginEvent()
        {
            RegEvent(EEvents.Event_LoginHeart, HandleCommon.Handle_LoginHeart);
            RegEvent(EEvents.Event_ServerTime, HandleCommon.Handle_ServerTime);



        }
    }
}
