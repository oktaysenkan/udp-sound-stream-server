using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace udp_sound_stream_server
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            (Process.GetCurrentProcess()).Kill();
        }

        public void ChangeConnectionStatusLabel(string text)
        {
            lblStatusValue.Text = text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            lblIPAdressValue.Text = string.Join("\n", IPAddressManager.GetLocalIPAddress()); ;
            SoundStreamer soundStreamer = new SoundStreamer(this);
            PluginLoader loader = new PluginLoader();
            loader.Subscribe((IMessageRecievedEventBroadcaster)soundStreamer);
        }
    }
}
