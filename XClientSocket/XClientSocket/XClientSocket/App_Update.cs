using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XClientSocket.XClientSocket
{
    class App_Update
    {
        public static void Init()
        {
            new Thread(
                () =>
                {
                    while (true)
                    {
                        UpdateSecend();
                        Thread.Sleep(1000);//参数：毫秒
                    }
                }).Start();
            new Thread(
            () =>
            {
                while (true)
                {
                     UpdateFrame();
                     Thread.Sleep(20);//每帧
                }
            }).Start(); 
        }
        /// <summary>
        /// 每帧执行 1/60s = 20毫秒
        /// </summary>
        public static void UpdateFrame()
        {
            App_ClientSocket.Update();
            //AsynchronousClient.Update();
        }

        /// <summary>
        /// 每秒执行
        /// </summary>
        public static void UpdateSecend()
        {
            //Log(DateTime.Now.ToString());
            
        }

        public static void Log(string s)
        {
            Console.WriteLine(s);
        }
    }
}
