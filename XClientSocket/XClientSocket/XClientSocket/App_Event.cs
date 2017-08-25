using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XClientSocket.XClientSocket
{




    public static partial class App_Event
    {
        private delegate void ActionHandleEvent(ArrByteReader reader);
        private static Dictionary<EEvents, ActionHandleEvent> _dicActionHandleEvent = new Dictionary<EEvents, ActionHandleEvent>();


        /// <summary>
        /// 注册协议
        /// </summary>
        private static void RegEvent()
        {
            RegLoginEvent();
            RegGameEvent();
        }

        public static void Init()
        {
            if (_dicActionHandleEvent.Count > 0)
                return;
            RegEvent();
        }
        private static void RegEvent(EEvents eventId, ActionHandleEvent actionHandle)
        {
            _dicActionHandleEvent.Add(eventId, actionHandle);
        }

        public static void HandleEvent(ServerPacket serverPacket)
        {
            EEvents ee = (EEvents)serverPacket.eventId;
            ActionHandleEvent action;
            if (_dicActionHandleEvent.TryGetValue(ee, out action))
            {
                action(serverPacket.arrByteReader);
            }
            else
            {
                Log("Event " + ee.ToString() + "不存在！");
            }
        }

        public static void Log(string s)
        {
            Console.WriteLine(s);
        }


    }


    public abstract class EventBase
    {
        //public int connId;
        public abstract ushort GetEventId();
        public virtual void SetPacket(ArrByteReader reader) { }
    }
}
