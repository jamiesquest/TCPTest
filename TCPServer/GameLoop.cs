using System;
using System.Collections.Generic;
using System.Text;

namespace TCPServer
{
    class GameLoop
    {
        public static void Update()
        {
            ThreadManager.UpdateMain();
        }
    }
}
