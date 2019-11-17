using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace udp_sound_stream_server
{
    class SoundStreamer
    {
        private SoundRecorder _soundRecorder;
        private UdpClient _streamServer;
        private IPEndPoint _serverEndPoint = new IPEndPoint(IPAddress.Any, 9050);
        private IPEndPoint _clientEndPoint;

        public SoundStreamer()
        {
            _soundRecorder = new SoundRecorder();
            _streamServer = new UdpClient(_serverEndPoint);
            _streamServer.BeginReceive(DataReceived, null);
        }


        private void DataReceived(IAsyncResult ar)
        {
            var bufferReceive = _streamServer.EndReceive(ar, ref _clientEndPoint);
            _streamServer.BeginReceive(DataReceived, null);

            if (bufferReceive.Length == 0)
                return;

            Request request = new Request(bufferReceive);
            RequestHandler requestHandler = new RequestHandler(request);
            requestHandler.Start();

            requestHandler.StreamStarted += (rate, second) =>
            {
            };
            requestHandler.StreamStopped += () => { };
            requestHandler.AudioQualityChanged += (rate, second) => { };


            StopSoundStreaming();
            StartSoundStreaming(request.SampleRate, request.BitPerSecond);

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
            _streamServer.Send(buffer, bytes, _clientEndPoint);
        }
    }
}
