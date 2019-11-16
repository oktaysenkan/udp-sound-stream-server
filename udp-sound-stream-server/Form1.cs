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
        private SoundRecorder _soundRecorder = new SoundRecorder();

        public Form1()
        {
            InitializeComponent();

            string ipAddresses = string.Join("\n", IPAddressManager.GetLocalIPAddress());
            lblIPAdressValue.Text = ipAddresses;

            _udp = new UdpClient(_serverEndPoint);
            _udp.BeginReceive(FirstDataReceived, null);
        }

        private void FirstDataReceived(IAsyncResult ar)
        {
            var data = _udp.EndReceive(ar, ref _clientEndPoint);
            _udp.BeginReceive(DataRecevied, null);

            if (data.Length == 0)
                return;

            _soundRecorder.SoundCaptured += (buffer, bytes) =>
            {
                _udp.Send(buffer, bytes, _clientEndPoint);
            };
            _soundRecorder.Start();
        }

        private void DataRecevied(IAsyncResult ar)
        {
            var data = _udp.EndReceive(ar, ref _clientEndPoint);
            _udp.BeginReceive(DataRecevied, null);

            if (data.Length == 0)
                return;

            RequestHandler requestHandler = new RequestHandler(data);
        }

    }
}
