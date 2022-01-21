using System;
using System.Collections.Generic;
using System.Text;

namespace TCPServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int client, Packet packet)
        {
            int clientID = packet.ReadInt();
            //Read some other data maybe.

            Console.WriteLine(Server.clients[client].tcp.socket.Client.RemoteEndPoint + " connected to server and is now Player " + client);
            if (client != clientID)
            {
                Console.WriteLine("Player " + client + " assumed the wrong client ID.");
            }

        }
    }
}
