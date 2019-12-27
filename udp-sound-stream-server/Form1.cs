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
using CSCore;
using CSCore.Codecs.WAV;
using CSCore.SoundIn;
using CSCore.Streams;

namespace udp_sound_stream_server
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            lblIPAdressValue.Text = string.Join("\n", IPAddressManager.GetLocalIPAddress()); ;

            SoundStreamer soundStreamer = new SoundStreamer
            {
                Context = this
            };
            soundStreamer.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            (Process.GetCurrentProcess()).Kill();
        }

        public void ChangeConnectionStatusLabel(string text)
        {
            lblStatusValue.Text = text;
        }
    }
}
