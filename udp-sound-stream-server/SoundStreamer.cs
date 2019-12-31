using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using CSCore;

namespace udp_sound_stream_server
{
    class SoundStreamer: IMessageRecievedEventBroadcaster
    {
        private UdpClient _streamServer;
        private SoundRecorder _soundRecorder;
        private IPEndPoint _serverEndPoint = new IPEndPoint(IPAddress.Any, 9050);
        private IPEndPoint _clientEndPoint;
        public Form1 Context;

        public event Action<IDictionary<string, string>> MessageRecievedEvent = delegate { };

        public SoundStreamer(Form1 context)
        {
            _streamServer = new UdpClient(_serverEndPoint);
            Context = context;
            _streamServer.BeginReceive(DataReceived, null);
        }


        private void DataReceived(IAsyncResult ar)
        {
            var bufferReceive = _streamServer.EndReceive(ar, ref _clientEndPoint);
            _streamServer.BeginReceive(DataReceived, null);

            Request request = new Request(bufferReceive);
            IRequestHandler controller = new StreamController(this);
            controller.OnRequestRecieved(request);
            MessageRecievedEvent(request.Parameters);
        }

        private void SoundCaptured(byte[] buffer, int bytes)
        {
            _streamServer.Send(buffer, bytes, _clientEndPoint);
        }

        public void StartStream(int sampleRate = 44100, int bitsPerSecond = 16)
        {
            if (_soundRecorder == null)
            {
                _soundRecorder = new SoundRecorder(sampleRate, bitsPerSecond);
                _soundRecorder.SoundCaptured += SoundCaptured;
            }

            Context.ChangeConnectionStatusLabel("Connected!");
            _soundRecorder.Start();
        }

        public void StopStream()
        {
            Context.ChangeConnectionStatusLabel("Disconnected!");
            _soundRecorder?.Stop();
        }

        public void ChangeAudioQuality(int sampleRate, int bitsPerSecond)
        {
            _soundRecorder.ChangeQuality(sampleRate, bitsPerSecond);
        }
    }

    public interface IMessageRecievedEventBroadcaster
    {
        event Action<IDictionary<string, string>> MessageRecievedEvent;
    }
}
