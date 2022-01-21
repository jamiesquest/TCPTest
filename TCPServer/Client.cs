using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    class Client
    {
        public static int dataBufferSize = 4096;

        public int ID;
        public TCP tcp;

        public Client(int clientID)
        {
            ID = clientID;
            tcp = new TCP(ID);
        }

        public class TCP
        {
            public TcpClient socket;

            private readonly int ID;

            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;
            
            public TCP(int id)
            {
                ID = id;
            }

            public void Connect(TcpClient socket)
            {
                this.socket = socket;
                this.socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receivedData = new Packet();
                receiveBuffer = new byte[dataBufferSize];


                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                //Send Welcome Packet

                ServerSend.Welcome(ID, "Welcome to the server.");

            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    int byteLength = stream.EndRead(result);
                    if (byteLength <= 0)
                    {
                        //TODO: Disconnect?
                        return;
                    }
                    byte[] data = new byte[byteLength];
                    Array.Copy(receiveBuffer, data, byteLength);

                    //TODO: Handle Data.
                    receivedData.Reset(HandleData(data));
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                }
                catch(Exception e)
                {
                    Console.WriteLine("Server: Error Receiving TCP Data: " + e);
                }
            }

            private bool HandleData(byte[] data)
            {
                int packetLength = 0;

                receivedData.SetBytes(data);

                if (receivedData.UnreadLength() >= 4)
                {
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }

                while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
                {
                    byte[] packetBytes = receivedData.ReadBytes(packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet packet = new Packet(packetBytes))
                        {
                            int packetID = packet.ReadInt();
                            Server.packetHandlers[packetID](ID, packet);
                        }
                    });
                    packetLength = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        packetLength = receivedData.ReadInt();
                        if (packetLength <= 0)
                        {
                            return true;
                        }
                    }
                }
                if (packetLength <=1)
                {
                    return true;
                }
                return false;

            }

            internal void SendData(Packet packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);


                    }
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
        }
    }
}
