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

            string ipAddresses = string.Join("\n", IPAddressManager.GetLocalIPAddress());
            lblIPAdressValue.Text = ipAddresses;

            SoundStreamer soundStreamer = new SoundStreamer();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            (Process.GetCurrentProcess()).Kill();
        }
    }
}
