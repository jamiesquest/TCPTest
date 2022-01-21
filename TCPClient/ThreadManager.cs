using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClient
{
    class ThreadManager
    {
        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainTread = new List<Action>();

        private static bool actionToExecuteOnMainThread = false;

        public static void ExecuteOnMainThread(Action action)
        {
            if (action == null)
            {
                Console.WriteLine("No action to execute on main thread!");
                return;
            }
            lock (executeOnMainThread)
            {
                executeOnMainThread.Add(action);
                actionToExecuteOnMainThread = true;
            }

        }

        public static void UpdateMain()
        {
            if (actionToExecuteOnMainThread)
            {
                executeCopiedOnMainTread.Clear();
                lock (executeOnMainThread)
                {
                    executeCopiedOnMainTread.AddRange(executeOnMainThread);
                    executeOnMainThread.Clear();
                    actionToExecuteOnMainThread = false;
                }

                for (int i = 0; i < executeCopiedOnMainTread.Count; i++)
                {
                    executeCopiedOnMainTread[i]();
                }
            }
        }
    }
}
