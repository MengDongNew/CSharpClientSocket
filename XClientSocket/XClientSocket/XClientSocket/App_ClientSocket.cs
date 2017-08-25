using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XClientSocket.XClientSocket
{

    /// <summary>
    /// 服务器连接管理
    /// </summary>
    public class App_ClientSocket
    {
        /// <summary>
        /// 存储多个客户端socket
        /// </summary>
        private static Dictionary<EServerType, AsynchronousClient> dicClients = new Dictionary<EServerType, AsynchronousClient>();
        public static Dictionary<EServerType, AsynchronousClient> ClientSockets { get { return dicClients; } }
        /// <summary>
        /// ip列表
        /// </summary>
        private static Dictionary<EServerType, SIpPort> dicIPs = new Dictionary<EServerType, SIpPort>();
       
        /// <summary>
        /// 初始化指定类型的socket
        /// </summary>
        /// <param name="type">EServerType</param>
        /// <param name="ipp">SIpPort</param>
        public static void InitSocket(EServerType type, SIpPort ipp)
        {
            dicIPs[type] = ipp;

            dicClients[type] = AsynchronousClient.Create(type, ipp);
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="_type"></param>
        public static AsynchronousClient ConnectServer(EServerType _type)
        {
            if (!dicClients.ContainsKey(_type))
            {
                Log("Error!!! !dicClients.ContainsKey(_type)");
                return null;
            }

            dicClients[_type].HandleConnect();
            return dicClients[_type];
        }

        /// <summary>
        /// 关闭与服务器的连接
        /// </summary>
        /// <param name="_type"></param>
        public static void CloseServerConnect(EServerType _type)
        {
            if (dicClients.ContainsKey(_type))
            {
                dicClients[_type].CloseSocket();

            }
        }

        /// <summary>
        /// 发送指定消息
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="type"></param>
        public static void Send(PacketSend pk, EServerType type)
        {
            dicClients[type].Send(pk);
        }

        /// <summary>
        /// 在UI线程执行
        /// </summary>
        public static void Update()
        {
            foreach (var v in dicClients.Values)
            {
                v.Update();
            }
        }
        public static void Log(string s)
        {
            Console.WriteLine(s);
        }
    }
}
