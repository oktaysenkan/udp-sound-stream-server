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

        private WasapiCapture soundIn;

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
            soundIn = new WasapiLoopbackCapture();
            soundIn.Initialize();

            SoundInSource soundInSource = new SoundInSource(soundIn) { FillWithZeros = false };
            IWaveSource convertedSource = soundInSource
                .ChangeSampleRate(44100)
                .ToSampleSource()
                .ToWaveSource(16)
                .ToStereo(); 

            soundInSource.DataAvailable += (s, e) =>
            {
                byte[] buffer = new byte[convertedSource.WaveFormat.BytesPerSecond / 2];
                int bytes;

                while ((bytes = convertedSource.Read(buffer, 0, buffer.Length)) > 0)
                {
                    UDP.Send(buffer, bytes, clientEndPoint);
                }
            };

            soundIn.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            soundIn?.Stop();
        }
    }
}
