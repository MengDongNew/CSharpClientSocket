using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XClientSocket.XClientSocket
{
    public class HandleCommon
    {
        /// <summary>
        /// 99 服务器时间
        /// </summary>
        public static void Handle_ServerTime(ArrByteReader reader)
        {
            string time = reader.ReadUTF8String();
            Log("Handle_ServerTime" + time);

        }


        /// <summary>
        /// 心跳包
        /// </summary>
        public static void Handle_LoginHeart(ArrByteReader reader)
        {
            //int utc = reader.ReadInt();
            //Log("Handle_LoginHeart=" + utc);
            int key = reader.ReadByte();
            string value = reader.ReadUTF8String();
            Log("key="+key+" value="+value);
            //if (utc == 0)
            {
                //User.GetInstance ().Logout ();
                //HintMgr.CreateDisConnect("请求超时，点击确认重新连接！");
            }
        }

        /// <summary>
        /// 心跳包
        /// </summary>
        public static void Handle_GameHeart(ArrByteReader reader)
        {
            int utc = reader.ReadInt();
            Log("Handle_GameHeart=" + utc);
            if (utc == 0)
            {
                //User.GetInstance ().Logout ();
                //HintMgr.CreateDisConnect("请求超时，点击确认重新连接！");
            }
        }



        static void Log(string s)
        {
            Console.WriteLine(s);
        }

    }



}
