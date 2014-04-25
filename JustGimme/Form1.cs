using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

using ServerEngine;

namespace JustGimme
{
    public partial class fMain : Form
    {
        Server reciever;
        public Server RecieverServer
        {
            get { return reciever;  }
        }
        Thread recieverThread;
        private int maxByteSendSize = 8192;

        byte[] localBuffer { get; set; }

        Socket sck;
        IPEndPoint localEndPoint;

        delegate void CommandAction();
        delegate void ProtocolAction(List<string> toks);
        delegate void CmdArgsActions(string[] args);

        Dictionary<string, CommandAction> Commands = new Dictionary<string, CommandAction>();
        Dictionary<string, ProtocolAction> Numerics = new Dictionary<string, ProtocolAction>();
        Dictionary<string, CmdArgsActions> CmdArgs = new Dictionary<string, CmdArgsActions>();

        bool Connected;

        List<string> soft_queue = new List<string>();
        public List<HardFile> hard_queue = new List<HardFile>();
        List<string> queue_names = new List<string>();
        public List<string> recieve_queue_names = new List<string>();
        public Dictionary<string, int> soft_recieve_queue = new Dictionary<string, int>();

        Thread Listener;

        ImageList imageList1;

        int orgWidth;
        int orgHeight;

        /// <summary>
        /// Initiate window. Also binds to port 8077 on start.
        /// </summary>
        public fMain()
        {
            InitializeComponent();
            imageList1 = new ImageList();
            lvQueue.SmallImageList = imageList1;

            reciever = new Server(8077);
            recieverThread = new Thread(new ThreadStart(reciever.Start));
            recieverThread.IsBackground = true;
            recieverThread.Start();
            bSend.Enabled = false;

            orgWidth = Width;
            orgHeight = Height;
        }

        /// <summary>
        /// Ran when the 'Connect' button is pressed.
        /// Initiates the socket and thread containing Listen()
        /// Enables the '->' button once a connection is fully established.
        /// </summary>
        /// <param name="sender">Inherited</param>
        /// <param name="e">Inherited</param>
        private void bAddress_Click(object sender, EventArgs e)
        {
            try
            {
                sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sck.SendBufferSize = maxByteSendSize;
                localEndPoint = new IPEndPoint(Dns.GetHostAddresses(tbAddress.Text)[Dns.GetHostAddresses(tbAddress.Text).Length - 1], 8077);
                sck.Connect(localEndPoint);
                Connected = true;
                Listener = new Thread(new ThreadStart(Listen));
                Listener.Start();
                tsslStatus.Text = "Fully connected";
                bAddress.Enabled = false;
                bSend.Enabled = true;
            }
            catch(SocketException sex)
            {
                Debug.WriteLine(sex);
                tsslStatus.Text = "Failed to connect.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Set the text on the bottom of the window.
        /// </summary>
        /// <param name="status">String with status</param>
        public void SetStatusText(string status)
        {
            tsslStatus.Text = status;
        }

        /// <summary>
        /// Ran once the drag and drop event has been completed.
        /// Extracts file paths from drag and drop event and adds it to the queue List<>.
        /// </summary>
        /// <param name="sender">Inherited</param>
        /// <param name="e">Inherited</param>
        private void lvQueue_DragDrop(object sender, DragEventArgs e)
        {
            soft_queue.InsertRange(0, (string[])(e.Data.GetData(DataFormats.FileDrop)));
            soft_queue = soft_queue.Distinct().ToList<string>();
            foreach (string s in soft_queue)
            {
                if (!queue_names.Contains(s.Split('\\')[s.Split('\\').Count() - 1]))
                {
                    queue_names.Add(s.Split('\\')[s.Split('\\').Count() - 1]);
                    hard_queue.Add(new HardFile(s));
                    Debug.WriteLine(s);
                }
            }
            lvQueue.Items.Clear();
            ListViewItem item;
            foreach (HardFile h in hard_queue)
            {
                // Set a default icon for the file.
                Icon iconForFile = SystemIcons.WinLogo;

                item = new ListViewItem(h.Information.Name, 1);
                iconForFile = Icon.ExtractAssociatedIcon(h.Information.FullName);

                // Check to see if the image collection contains an image 
                // for this extension, using the extension as a key. 
                if (!imageList1.Images.ContainsKey(h.Information.Extension))
                {
                    // If not, add the image to the image list.
                    iconForFile = Icon.ExtractAssociatedIcon(h.Information.FullName);
                    if (iconForFile != null) imageList1.Images.Add(h.Information.Extension, iconForFile);
                }
                item.ImageKey = h.Information.Extension;
                lvQueue.Items.Add(item);
            }
        }

        /// <summary>
        /// Ran once the mouse holding something enters the queue section.
        /// Sets the drag animation to 'Copy'.
        /// </summary>
        /// <param name="sender">Inherited</param>
        /// <param name="e">Inherited</param>
        private void lvQueue_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// Reloads the hard_queue.
        /// Checks to make sure all the files exists.
        /// Sends files out the socket.
        /// </summary>
        /// <param name="sender">Inherited</param>
        /// <param name="e">Inherited</param>
        private void bSend_Click(object sender, EventArgs e)
        {
            bool all_exist = true;
            
            foreach (HardFile h in hard_queue)
            {
                if (!h.Exists) all_exist = false;
                if (h.Information.Length > maxByteSendSize) Debug.WriteLine("File "+h.ShortName+" will need to be broken.");
            }
            if (!all_exist)
            {
                tsslStatus.Text = "A file in the queue is missing. Aborting.";
                return;
            }
            tsslStatus.Text = "Ready to send";
            string fileByteInfo = "0 ";
            foreach (HardFile h in hard_queue)
            {
                fileByteInfo += h.ShortName + " " + h.Information.Length + " ";
            }
            SendToServer(fileByteInfo.Trim());

            foreach (HardFile h in hard_queue)
            {
                SendToServer(h);
                lvQueue.Items.RemoveByKey(h.Information.Name);
            }
        }

        private void fMain_Resize(object sender, EventArgs e)
        {

        }
    }
}
