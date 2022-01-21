using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClient
{
    class ClientSend
    {

        private static void SendTCPData(Packet packet)
        {
            packet.WriteLength();
            Client.tcp.SendData(packet);
        }

        public static void WelcomeReceived()
        {
            using (Packet packet = new Packet((int)ClientPackets.WelcomeReceived))
            {
                packet.Write(Client.myID);

                SendTCPData(packet);
            }
        }
    }
}
