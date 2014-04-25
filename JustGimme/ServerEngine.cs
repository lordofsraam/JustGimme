using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace ServerEngine
{
    public partial class Server
    {
        static Socket sck;
        public string buffer;
        public const string Version = "0.0.2";
        public List<ServerClient> Clients;
        public uint MaximumConnections;

        int Port;

        public Server(int port)
        {
            Port = port;
            MaximumConnections = 1;
        }

        public void RemoveClient(ServerClient victim)
        {
            Clients.Remove(victim);
        }

        public void Start()
        {

            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sck.Bind(new IPEndPoint(0, Port));
            sck.Listen(100);

            Clients = new List<ServerClient>();

            bool stat_set = false;

            while (true)
            {
                try
                {
                    if (Clients.Count < MaximumConnections)
                    {
                        Debug.WriteLine("Waiting for clients...");
                        Clients.Add(new ServerClient(this, sck.Accept()));
                        Debug.WriteLine("Connection to " + Clients[Clients.Count - 1].IPAddress + " made. Now starting client thread...");
                        Clients[Clients.Count - 1].Run();
                        Debug.WriteLine("Client thread started.");
                        JustGimme.Program.pMain.SetStatusText("Outside connection accepted.");
                    }
                    else
                    {
                        if (!stat_set)
                        {
                            JustGimme.Program.pMain.SetStatusText("No longer accepting outside connections.");
                            stat_set = true;
                        }
                    }
                }
                catch (SocketException ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
        }
    }
}