using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        UdpClient UDP;
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 9050);
        IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

        public Form1()
        {
            InitializeComponent();

            string ipAddresses = string.Join("\n", IPAddressManager.GetLocalIPAddress());
            lblIPAdressValue.Text = ipAddresses;

            UDP = new UdpClient(serverEndPoint);
            UDP.BeginReceive(FirstDataReceived, null);
        }

        private void FirstDataReceived(IAsyncResult ar)
        {
            var data = UDP.EndReceive(ar, ref clientEndPoint);

            if (data.Length == 0)
                return;

            StartAudioStreaming();
        }

        private void StartAudioStreaming()
        {
            SoundRecorder soundRecorder = new SoundRecorder();
            soundRecorder.SoundCaptured += (buffer, bytes) => { UDP.Send(buffer, bytes, clientEndPoint); };
            soundRecorder.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
