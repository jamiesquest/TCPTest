using System;
using System.Threading;

namespace TCPServer
{
    class Program
    {
        private static bool isRunning = false;

        static void Main(string[] args)
        {
            Console.Title = "Mock Server";

            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();
            
            Server.Start(12, 29805);

        }

        private static void MainThread()
        {
            Console.WriteLine("Main thread running at "+Constants.TICKS_PER_SEC+" Ticks Per Second");

            DateTime nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (nextLoop< DateTime.Now)
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
