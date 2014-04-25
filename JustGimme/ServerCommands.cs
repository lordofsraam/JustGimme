using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using JustGimme;

namespace ServerEngine
{
    public static class ServerCommands
    {
        public static void InformationParse(List<string> message)
        {
            if (message.Count >= 3)
            {
                JustGimme.Program.pMain.SetStatusText("Waiting for " + (message.Count-1)/2 + " file(s).");
                Debug.WriteLine("Waiting for file(s):");
                //Find out how many files we're going to get and how big each one is.
                for (int i = 1; i < message.Count; i += 2)
                {
                    Debug.WriteLine(message[i] +" : "+ Int32.Parse(message[i + 1]));
                    Program.pMain.soft_recieve_queue.Add(message[i],Int32.Parse(message[i+1]));
                    Program.pMain.recieve_queue_names.Add(message[i]);
                }

                //Switch to TX/RX mode
                Program.pMain.RecieverServer.Clients[0].Mode = 1;
            }
            else
            {
                JustGimme.Program.pMain.SetStatusText("Weird information format recieved.");
            }
        }
    }
}
