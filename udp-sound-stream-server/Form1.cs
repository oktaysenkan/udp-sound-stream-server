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

            UDP = new UdpClient(serverEndPoint);
            UDP.BeginReceive(DataReceived, null);
        }

        private void DataReceived(IAsyncResult ar)
        {
            byte[] data;
            try
            {
                data = UDP.EndReceive(ar, ref clientEndPoint);

                if (data.Length == 0)
                    return; // No more to receive

                //MessageBox.Show(Encoding.UTF8.GetString(data, 0, data.Length));
            }
            catch (ObjectDisposedException)
            {
                return; // Connection closed
            }

            // Send the data to the UI thread
            this.BeginInvoke((Action<IPEndPoint, string>)DataReceivedUI, clientEndPoint, Encoding.UTF8.GetString(data));
            StartAudioStreaming();
        }

        private void DataReceivedUI(IPEndPoint endPoint, string data)
        {
            lblStatusValue.Text = "Connected";
            lblIPAdressValue.Text = endPoint.ToString();
        }

        private void StartAudioStreaming()
        {
            soundIn = new WasapiLoopbackCapture();
            soundIn.Initialize();

            SoundInSource soundInSource = new SoundInSource(soundIn) { FillWithZeros = false };
            IWaveSource convertedSource = soundInSource
                .ChangeSampleRate(44100) // sample rate
                .ToSampleSource()
                .ToWaveSource(16); //bits per sample
            convertedSource = convertedSource.ToStereo();

            WaveWriter waveWriter = new WaveWriter("dump.wav", convertedSource.WaveFormat);

            soundInSource.DataAvailable += (s, e) =>
            {
                //read data from the converedSource
                //important: don't use the e.Data here
                //the e.Data contains the raw data provided by the 
                //soundInSource which won't have your target format
                byte[] buffer = new byte[convertedSource.WaveFormat.BytesPerSecond / 2];
                int read;

                //keep reading as long as we still get some data
                //if you're using such a loop, make sure that soundInSource.FillWithZeros is set to false
                while ((read = convertedSource.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //write the read data to a file
                    // ReSharper disable once AccessToDisposedClosure
                    //waveWriter.Write(buffer, 0, read);
                    UDP.Send(buffer, read, clientEndPoint);
                }
            };

            soundIn.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (soundIn != null)
                soundIn.Stop();
        }
    }
}
