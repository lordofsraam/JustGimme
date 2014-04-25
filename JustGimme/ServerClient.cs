using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace ServerEngine
{

    public partial class ServerClient
    {
        protected Server Host;
        Socket s;
        Thread t;
        public string IPAddress;

        public delegate void ServerCommandFunction(List<string> message);

        protected Dictionary<string, ServerCommandFunction> ServerCommandActions = new Dictionary<string, ServerCommandFunction>();

        byte[] Buffer { get; set; }
        int currentBytesNeeded;
        List<byte> currentBytesRecieved = new List<byte>();
        int currentPlaceInQueue;

        /// <summary>
        /// 0 - Info mode - All packets treated as strings.
        /// 1 - TX/RX mode - Handle raw bytes.
        /// </summary>
        int mode;
        public int Mode
        {
            get { return mode; }
            set 
            { 
                mode = value;
                Debug.WriteLine("Mode set to "+value.ToString());
            }
        }

        public ServerClient(Server host, Socket sock)
        {
            Host = host;
            s = sock;
            IPAddress = sock.LocalEndPoint.ToString().Split(':')[0];
            mode = 0;
            ServerCommandActions.Add("0", ServerCommands.InformationParse);
            currentPlaceInQueue = 0;
        }

        ~ServerClient()
        {
            Debug.WriteLine("Destructor was called.");
        }

        /// <summary>
        /// Starts the Listener() in a background thread.
        /// </summary>
        public void Run()
        {
            t = new Thread(new ThreadStart(this.Listener));
            t.IsBackground = true;
            t.Start();
        }

        /// <summary>
        /// Turns a string into bytes and sends it out the socket.
        /// </summary>
        /// <param name="msg">String msg to send out the socket.</param>
        public void SendStringMessage(string msg)
        {
            //Debug.WriteLine("Sending '" + msg + "'"+" to "+this.IPAddress);
            byte[] data = Encoding.ASCII.GetBytes(msg);
            Array.Resize<byte>(ref data, s.SendBufferSize);
            s.Send(data);
        }

        protected virtual void OnSocketClose() { }

        /// <summary>
        /// Close the socket and clean up.
        /// </summary>
        public void Close()
        {
            s.Close();
            Debug.WriteLine("Socket to " + IPAddress + " has been closed. Calling OnSocketClose()");
            this.OnSocketClose();
            Debug.WriteLine("Cleaning client from server...");
            Host.RemoveClient(this);
            JustGimme.Program.pMain.SetStatusText("Sender dropped.");
        }

        /// <summary>
        /// Accept packets from the socket and deal with them according to the mode.
        /// Runs as a separate thread.
        /// </summary>
        public void Listener()
        {
            Debug.WriteLine("Listener for " + this.IPAddress + " has been started.");
            try
            {
                while (true)
                {

                    if (!s.Connected) { throw new SocketException(10054); }

                    Buffer = new byte[s.SendBufferSize];
                    int bytesRead = 0;

                    try
                    {
                        bytesRead = s.Receive(Buffer, s.SendBufferSize, SocketFlags.None);
                    }
                    catch (SocketException)
                    {
                        Debug.WriteLine("Error on Receive(). Client probably disconnected. Will now Close().");
                        this.Close();
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        Debug.WriteLine("0 bytes read from client " + IPAddress + ". Closing client socket...");
                        this.Close();
                        break;
                    }

                    if (mode == 0)
                    {
                        /** Format buffer to take care of whitespace or extra crap*/
                        List<byte> formatted = new List<byte>(bytesRead);
                        for (int i = 0; i < bytesRead; i++)
                        {
                            if (Buffer[i] != default(byte))
                            {
                                formatted.Add(Buffer[i]);
                            }
                        }
                        string strData = Encoding.ASCII.GetString(formatted.ToArray()).Trim();

                        if (string.IsNullOrWhiteSpace(strData)) continue;

                        Debug.WriteLine(this.IPAddress + ": " + strData);

                        List<string> tokens = strData.Split(' ').ToList<string>();

                        Debug.WriteLine(tokens[0].ToLower());
                        //If we have a function made to handle this, send it the message in tokens.
                        if (ServerCommandActions.ContainsKey(tokens[0].ToLower()))
                        {
                            ServerCommandActions[tokens[0].ToLower()](tokens);
                        }
                    }
                    else if(mode == 1)
                    {
                        //Debug.WriteLine("Receiving file(s).");
                        JustGimme.Program.pMain.SetStatusText("Receiving file(s).");

                        currentBytesNeeded = JustGimme.Program.pMain.soft_recieve_queue[JustGimme.Program.pMain.recieve_queue_names[currentPlaceInQueue]];
                        //Debug.WriteLine("Bytes needed are : " + currentBytesNeeded);
                        List<byte> formatted = new List<byte>(bytesRead);
                        if (currentBytesRecieved.Count < currentBytesNeeded)
                        {
                            for (int i = 0; i < bytesRead && currentBytesRecieved.Count < currentBytesNeeded; i++)
                            {
                                currentBytesRecieved.Add(Buffer[i]);
                            }
                        }
                        if (currentBytesRecieved.Count >= currentBytesNeeded)
                        {
                            File.WriteAllBytes(JustGimme.Program.pMain.recieve_queue_names[currentPlaceInQueue], currentBytesRecieved.ToArray());
                            Debug.WriteLine("currentBytesRecieved.Count: " + currentBytesRecieved.Count);
                            if (currentBytesRecieved.Count == currentBytesNeeded)
                            {
                                Debug.WriteLine("Bytes matched exactly.");
                            }
                            else
                            {
                                Debug.WriteLine("Bytes were off by " + (currentBytesRecieved.Count - currentBytesNeeded));
                            }
                            currentBytesNeeded = 0;
                            currentBytesRecieved = new List<byte>();
                            Debug.WriteLine("pMain.recieve_queue_names.Count: " + JustGimme.Program.pMain.recieve_queue_names.Count);
                            Debug.WriteLine("currentPlaceInQueue: " + currentPlaceInQueue);
                            if (JustGimme.Program.pMain.recieve_queue_names.Count > currentPlaceInQueue)
                            {
                                currentPlaceInQueue++;
                                if (JustGimme.Program.pMain.recieve_queue_names.Count == currentPlaceInQueue)
                                {
                                    Debug.WriteLine("All files recieved.");
                                    this.OnRecieveDone();
                                }
                                else
                                {
                                    Debug.WriteLine("Moving on to next file.");
                                }
                            }
                        }
                        else
                        {
                            //Debug.WriteLine("Waiting for next packet.");
                        }
                    }

                    
                }
            }
            catch (SocketException error)
            {
                Debug.WriteLine(error.ToString());
                if (error.ErrorCode == 10054)
                {
                    JustGimme.Program.pMain.SetStatusText("Sender dropped.");
                }
                else
                {
                    JustGimme.Program.pMain.SetStatusText(error.Message);
                }
            }
        } //listener end

        /// <summary>
        /// Actions to perform after all files are done being recieved. 
        /// </summary>
        private void OnRecieveDone()
        {
            JustGimme.Program.pMain.SetStatusText("Done.");
        } 
    } //class end
} //ns end