using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClient
{
    class ClientHandle
    {
        public static void Welcome(Packet packet)
        {
            string msg = packet.ReadString();
            int myID = packet.ReadInt();

            Console.WriteLine("Message from server: " + msg);

            Client.myID = myID;

            ClientSend.WelcomeReceived();
        }

    }
}
