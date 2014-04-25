using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Diagnostics;

namespace JustGimme
{
    public partial class fMain
    {
        /// <summary>
        /// Performs all socket listening actions.
        /// </summary>
        private void Listen()
        {
            try
            {
                while (true)
                {

                    if (!sck.Connected)
                    {
                        throw new SocketException(10054);
                    }

                    localBuffer = new byte[sck.SendBufferSize];

                    int bytesRead = sck.Receive(localBuffer, sck.SendBufferSize, SocketFlags.None);

                    if (bytesRead == 0)
                    {
                        Connected = false;
                        Debug.WriteLine("Server disconnected. Closing socket...");
                        sck.Close();
                        Debug.WriteLine("Socket closed. Successfully disconnected from server.");
                        tsslStatus.Text = "Disconnected from receiver.";
                    }


                    /** Format buffer to take care of whitespace or extra crap*/
                    List<byte> formatted = new List<byte>(bytesRead);
                    for (int i = 0; i < bytesRead; i++)
                    {
                        if (localBuffer[i] != default(byte))
                        {
                            formatted.Add(localBuffer[i]);
                        }
                    }

                    string strData = Encoding.ASCII.GetString(formatted.ToArray()).Trim();
                    if (string.IsNullOrWhiteSpace(strData)) continue;
                    List<string> toks = strData.Split(' ').ToList();
                    string protocol = toks[0];

                    if (Numerics.ContainsKey(protocol))
                    {
                        Numerics[protocol](toks);
                    }
                    else
                    {
                        Debug.WriteLine("Server sent erroneous protocol. Message was '" + strData + "'");
                    }
                }
            }
            catch (SocketException error)
            {
                Debug.WriteLine("Socket error: " + error.Message);
                tsslStatus.Text = "Disconnected from receiver.";
            }
        }

        void SendToServer(string msg)
        {
            msg = msg.Trim();
            if (Connected && !string.IsNullOrWhiteSpace(msg))
            {
                byte[] data = Encoding.ASCII.GetBytes(msg);
                Array.Resize(ref data, sck.SendBufferSize);
                sck.Send(data);
            }
        }

        void SendToServer(byte[] filedata)
        {
            if (Connected)
            {
                if (filedata.Length <= sck.SendBufferSize)
                {
                    Array.Resize(ref filedata, sck.SendBufferSize);
                    sck.Send(filedata);
                }
                else
                {
                    Debug.WriteLine("Byte array too large to send.");
                }
            }
        }

        void SendToServer(HardFile h)
        {
            if (Connected)
            {
                while (h.NeedToStream)
                {
                    byte[] buff = h.StreamBytes();
                    Array.Resize(ref buff, sck.SendBufferSize);
                    sck.Send(buff);
                    Debug.WriteLine("Sent: {0}/{1} ( {2} % )", h.BytesSent, h.Information.Length,
                        ((float) h.BytesSent/(float) h.Information.Length)*100);
                    Program.pMain.SetStatusText("Sent " +
                                                          ((float) h.BytesSent/ (float)h.Information.Length)*100 +
                                                          " % of file " + h.ShortName);
                }
                lvQueue.Items.RemoveByKey(h.Information.Name);
            }
        }

        private void SendToServer(object h)
        {
            if (Connected && h is HardFile)
            {
                HardFile _h = (HardFile) h;
                while (_h.NeedToStream)
                {
                    byte[] buff = _h.StreamBytes();
                    Array.Resize(ref buff, sck.SendBufferSize);
                    sck.Send(buff);
                    Debug.WriteLine("Sent: {0}/{1} ( {2} % )", _h.BytesSent, _h.Information.Length,
                        ((float)_h.BytesSent / (float)_h.Information.Length) * 100);
                    Program.pMain.SetStatusText("Sent " +
                                                          ((float)_h.BytesSent / (float)_h.Information.Length) * 100 +
                                                          " % of file " + _h.ShortName);
                }
                lvQueue.Items.RemoveByKey(_h.Information.Name);
            }
            else
            {
                throw new Exception("Either not connected, or tried to send something that wasnt of type <HardFile>");
            }
        }
    }
}
