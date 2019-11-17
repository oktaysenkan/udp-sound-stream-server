using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.SoundIn;
using CSCore.Streams;

namespace udp_sound_stream_server
{
    class SoundRecorder
    {
        private WasapiLoopbackCapture _soundIn;
        private int _sampleRate;
        private int _bitPerSecond;

        public delegate void SoundCapturedEventHandler(byte[] buffer, int bytes);
        public event SoundCapturedEventHandler SoundCaptured;

        public SoundRecorder(int sampleRate = 44100, int bitsPerSecond = 16)
        {
            _soundIn = new WasapiLoopbackCapture();
            _soundIn.Initialize();
            _sampleRate = sampleRate;
            _bitPerSecond = bitsPerSecond;

            SoundInSource soundInSource = new SoundInSource(_soundIn) { FillWithZeros = false };
            IWaveSource convertedSource = soundInSource
                .ChangeSampleRate(sampleRate)
                .ToSampleSource()
                .ToStereo()
                .ToWaveSource(bitsPerSecond);

            soundInSource.DataAvailable += (s, e) =>
            {
                byte[] buffer = new byte[convertedSource.WaveFormat.BytesPerSecond / 2];
                int bytes;

                while ((bytes = convertedSource.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (SoundCaptured == null)
                        throw new Exception("SoundCapturedEventHandler not implemented");

                    SoundCaptured(buffer, bytes);
                }
            };

        }

        ~SoundRecorder()
        {
            Stop();
        }

        public void Start()
        {
            _soundIn.Start();
        }

        public void Stop()
        {
            if (_soundIn.RecordingState == RecordingState.Recording)
            {
                _soundIn?.Stop();
                SoundCaptured = null;
            }
        }
    }
}
