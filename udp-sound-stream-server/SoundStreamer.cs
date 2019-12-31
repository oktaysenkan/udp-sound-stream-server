﻿using System;
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
    class SoundStreamer
    {
        private UdpClient _streamServer;
        private SoundRecorder _soundRecorder;
        private IPEndPoint _serverEndPoint = new IPEndPoint(IPAddress.Any, 9050);
        private IPEndPoint _clientEndPoint;
        public Form1 Context;

        public SoundStreamer(Form1 context)
        {
            _streamServer = new UdpClient(_serverEndPoint);
            Context = context;
        }

        public void Start()
        {
            _streamServer.BeginReceive(DataReceived, null);
        }

        public void Stop()
        {
            _streamServer.Close();
        }

        private void DataReceived(IAsyncResult ar)
        {
            var bufferReceive = _streamServer.EndReceive(ar, ref _clientEndPoint);
            _streamServer.BeginReceive(DataReceived, null);

            Request request = new Request(bufferReceive);
            RequestHandler requestHandler = new RequestHandler(request);

            requestHandler.StreamStarted += StreamStarted;
            requestHandler.StreamStopped += StreamStopped;
            requestHandler.AudioQualityChanged += AudioQualityChanged;
            requestHandler.Start();
        }

        private void SoundCaptured(byte[] buffer, int bytes)
        {
            _streamServer.Send(buffer, bytes, _clientEndPoint);
        }

        private void StreamStarted(int sampleRate = 44100, int bitsPerSecond = 16)
        {
            if (_soundRecorder == null)
            {
                _soundRecorder = new SoundRecorder(sampleRate, bitsPerSecond);
                _soundRecorder.SoundCaptured += SoundCaptured;
            }

            Context.ChangeConnectionStatusLabel("Connected!");
            _soundRecorder.Start();
        }

        private void StreamStopped()
        {
            Context.ChangeConnectionStatusLabel("Disconnected!");
            _soundRecorder?.Stop();
        }

        private void AudioQualityChanged(int sampleRate, int bitsPerSecond)
        {
            _soundRecorder.ChangeQuality(sampleRate, bitsPerSecond);
        }

    }
}
