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
        private UdpClient _udp;
        private IPEndPoint _serverEndPoint = new IPEndPoint(IPAddress.Any, 9050);
        private IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
        private SoundRecorder _soundRecorder;

        public Form1()
        {
            InitializeComponent();

            string ipAddresses = string.Join("\n", IPAddressManager.GetLocalIPAddress());
            lblIPAdressValue.Text = ipAddresses;

            _udp = new UdpClient(_serverEndPoint);
            _udp.BeginReceive(DataRecevied, null);
        }

        private void DataRecevied(IAsyncResult ar)
        {
            var bufferReceive = _udp.EndReceive(ar, ref _clientEndPoint);
            _udp.BeginReceive(DataRecevied, null);

            if (bufferReceive.Length == 0)
                return;

            RequestHandler request = new RequestHandler(bufferReceive);

            if (request.StartStream)
            {
                if (request.BitPerSecond != 0 && request.SampleRate != 0)
                    StartSoundStreaming(request.SampleRate, request.BitPerSecond);
                else
                    StartSoundStreaming();
            }
            else if (request.StopStream)
            {
                StopSoundStreaming();
            }
            else if (request.ChangeAudioQuality)
            {
                if (request.BitPerSecond == 0 && request.SampleRate == 0)
                    throw new Exception("Bit per second or sample rate must not be null or empty.");

                StartSoundStreaming(request.SampleRate, request.BitPerSecond);
            }
        }

        private void StartSoundStreaming(int sampleRate = 44100, int bitsPerSecond = 16)
        {
            StopSoundStreaming();
            _soundRecorder = new SoundRecorder(sampleRate, bitsPerSecond);
            _soundRecorder.SoundCaptured += SoundRecorderOnSoundCaptured;
            _soundRecorder.Start();
        }

        private void StopSoundStreaming()
        {
            if (_soundRecorder != null)
            {
                _soundRecorder.Stop();
                _soundRecorder.SoundCaptured -= SoundRecorderOnSoundCaptured;
            }
        }

        private void SoundRecorderOnSoundCaptured(byte[] buffer, int bytes)
        {
            _udp.Send(buffer, bytes, _clientEndPoint);
        }
    }
}
