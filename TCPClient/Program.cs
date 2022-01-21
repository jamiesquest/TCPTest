using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPClient
{
    class Program
    {
        public static bool isRunning = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to connect to server...");
            Console.ReadKey();

            Console.WriteLine("Connecting to server...");
            Client.ConnectToServer();

            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();
            
        }

        private static void MainThread()
        {
            Console.WriteLine("Main Thread Running at " + Constants.TICKS_PER_SEC + " Ticks per second");

            DateTime nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (nextLoop < DateTime.Now)
                {
                    GameLoop.Update();

                    nextLoop = nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(nextLoop - DateTime.Now);
                    }

                }
            }


        }
    }
}
