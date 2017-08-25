using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XClientSocket.XClientSocket
{
    static class Program
    {
        //mengdong
        static string ip_login = "192.168.1.10", port_login = "26680";
        static string ip_game = "192.168.1.10", port_game = "27663";
        
        //local server
        //static string ip_login = "192.168.1.196", port_login = "27662";
        //static string ip_game = "192.168.1.196", port_game = "27663";

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            App_Update.Init();
            App_Event.Init();



            App_ClientSocket.InitSocket(EServerType.Type_Login, SIpPort.Create(ip_login, port_login));
          //  App_ClientSocket.InitSocket(EServerType.Type_Game, SIpPort.Create(ip_game, port_game));


            AsynchronousClient clientSocket = App_ClientSocket.ClientSockets[EServerType.Type_Login];
            clientSocket.conFinishedCallback = (bool succeed) =>
            {
                if (succeed)
                {
                    Log("Type_Login"+"===Connected Succeed");
                    PacketSend pk = PacketSend.Create(EEvents.Event_LoginHeart);
                    pk.Write((byte) 1);
                    pk.Write("meng dong..你好");
                    
                    App_ClientSocket.Send(pk, EServerType.Type_Login);

                    pk = PacketSend.Create(EEvents.Event_ServerTime);
                    App_ClientSocket.Send(pk, EServerType.Type_Login);

                }else
                {
                    Log("Type_Login" + "===Connected Failed!!!");
                }
            };
            clientSocket.HandleConnect();


            //if (!App_ClientSocket.ClientSockets.ContainsKey(EServerType.Type_Game))
            //{
            //    Log("Error!!!===!App_ClientSocket.ClientSockets.ContainsKey(EServerType.Type_Game)");
            //    return;
            //}
            
           //clientSocket = App_ClientSocket.ClientSockets[EServerType.Type_Game];
           //clientSocket.conFinishedCallback = (bool succeed) =>
           //{
           //    if (succeed)
           //    {
           //        Log("Type_Game" + "===Connected Succeed");
           //        PacketSend pk = PacketSend.Create(EEvents.Event_GameHeart);
           //        App_ClientSocket.Send(pk, EServerType.Type_Game);
           //    }
           //    else
           //    {
           //        Log("Type_Game" + "===Connected Failed!!!");
           //    }
           //};
           //clientSocket.HandleConnect();
           
            Start();

        }

        static void Start()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void Log(string s)
        {
            Console.WriteLine(s);
        }
    }
}
