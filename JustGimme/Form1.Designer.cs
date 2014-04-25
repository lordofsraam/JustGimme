namespace JustGimme
{
    partial class fMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.bAddress = new System.Windows.Forms.Button();
            this.lAddress = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvQueue = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvSent = new System.Windows.Forms.ListView();
            this.bSend = new System.Windows.Forms.Button();
            this.niStatus = new System.Windows.Forms.NotifyIcon(this.components);
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.tsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBookmarks = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ssStatus.SuspendLayout();
            this.msMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbAddress
            // 
            this.tbAddress.Location = new System.Drawing.Point(64, 33);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(314, 20);
            this.tbAddress.TabIndex = 0;
            // 
            // bAddress
            // 
            this.bAddress.Location = new System.Drawing.Point(384, 31);
            this.bAddress.Name = "bAddress";
            this.bAddress.Size = new System.Drawing.Size(75, 23);
            this.bAddress.TabIndex = 1;
            this.bAddress.Text = "Connect";
            this.bAddress.UseVisualStyleBackColor = true;
            this.bAddress.Click += new System.EventHandler(this.bAddress_Click);
            // 
            // lAddress
            // 
            this.lAddress.AutoSize = true;
            this.lAddress.Location = new System.Drawing.Point(13, 36);
            this.lAddress.Name = "lAddress";
            this.lAddress.Size = new System.Drawing.Size(45, 13);
            this.lAddress.TabIndex = 2;
            this.lAddress.Text = "Address";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvQueue);
            this.groupBox1.Location = new System.Drawing.Point(16, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 305);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Queue";
            // 
            // lvQueue
            // 
            this.lvQueue.AllowDrop = true;
            this.lvQueue.Location = new System.Drawing.Point(6, 19);
            this.lvQueue.Name = "lvQueue";
            this.lvQueue.Size = new System.Drawing.Size(194, 280);
            this.lvQueue.TabIndex = 0;
            this.lvQueue.UseCompatibleStateImageBehavior = false;
            this.lvQueue.View = System.Windows.Forms.View.SmallIcon;
            this.lvQueue.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvQueue_DragDrop);
            this.lvQueue.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvQueue_DragEnter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvSent);
            this.groupBox2.Location = new System.Drawing.Point(272, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 304);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sent";
            // 
            // lvSent
            // 
            this.lvSent.Location = new System.Drawing.Point(6, 18);
            this.lvSent.Name = "lvSent";
            this.lvSent.Size = new System.Drawing.Size(175, 280);
            this.lvSent.TabIndex = 1;
            this.lvSent.UseCompatibleStateImageBehavior = false;
            this.lvSent.View = System.Windows.Forms.View.Tile;
            // 
            // bSend
            // 
            this.bSend.Location = new System.Drawing.Point(228, 196);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(38, 23);
            this.bSend.TabIndex = 5;
            this.bSend.Text = "->";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // niStatus
            // 
            this.niStatus.Text = "Just Gimme Status";
            this.niStatus.Visible = true;
            // 
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus});
            this.ssStatus.Location = new System.Drawing.Point(0, 376);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(471, 22);
            this.ssStatus.TabIndex = 6;
            this.ssStatus.Text = "statusStrip1";
            // 
            // tsslStatus
            // 
            this.tsslStatus.Name = "tsslStatus";
            this.tsslStatus.Size = new System.Drawing.Size(108, 17);
            this.tsslStatus.Text = "Waiting to connect";
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEdit});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(471, 24);
            this.msMain.TabIndex = 7;
            this.msMain.Text = "menuStrip1";
            // 
            // tsmiEdit
            // 
            this.tsmiEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBookmarks,
            this.tsmiPreferences});
            this.tsmiEdit.Name = "tsmiEdit";
            this.tsmiEdit.Size = new System.Drawing.Size(39, 20);
            this.tsmiEdit.Text = "Edit";
            // 
            // tsmiBookmarks
            // 
            this.tsmiBookmarks.Name = "tsmiBookmarks";
            this.tsmiBookmarks.Size = new System.Drawing.Size(152, 22);
            this.tsmiBookmarks.Text = "Bookmarks";
            // 
            // tsmiPreferences
            // 
            this.tsmiPreferences.Name = "tsmiPreferences";
            this.tsmiPreferences.Size = new System.Drawing.Size(152, 22);
            this.tsmiPreferences.Text = "Preferences";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 398);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.msMain);
            this.Controls.Add(this.bSend);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lAddress);
            this.Controls.Add(this.bAddress);
            this.Controls.Add(this.tbAddress);
            this.MaximizeBox = false;
            this.Name = "fMain";
            this.Text = "Just Gimme";
            this.Resize += new System.EventHandler(this.fMain_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Button bAddress;
        private System.Windows.Forms.Label lAddress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.NotifyIcon niStatus;
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
        private System.Windows.Forms.ListView lvQueue;
        private System.Windows.Forms.ListView lvSent;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiBookmarks;
        private System.Windows.Forms.ToolStripMenuItem tsmiPreferences;
    }
}

