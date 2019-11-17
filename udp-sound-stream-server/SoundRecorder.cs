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
        private IWaveSource _targetSoundSource;
        private SoundInSource _defaultSoundSource;

        public int SampleRate { get; private set; }
        public int BitPerSecond { get; private set; }

        public bool IsRecording => _soundIn?.RecordingState == RecordingState.Recording;

        public delegate void SoundCapturedEventHandler(byte[] buffer, int bytes);
        public event SoundCapturedEventHandler SoundCaptured;

        public SoundRecorder (int sampleRate = 44100, int bitsPerSecond = 16)
        {
            SampleRate = sampleRate;
            BitPerSecond = bitsPerSecond;

            _soundIn = new WasapiLoopbackCapture();
            _soundIn.Initialize();
            SampleRate = sampleRate;
            BitPerSecond = bitsPerSecond;

            _defaultSoundSource = new SoundInSource(_soundIn) { FillWithZeros = false };
            ChangeQuality(SampleRate, BitPerSecond);

            _defaultSoundSource.DataAvailable += (s, e) =>
            {
                byte[] buffer = new byte[_targetSoundSource.WaveFormat.BytesPerSecond / 2];
                int bytes;

                while ((bytes = _targetSoundSource.Read(buffer, 0, buffer.Length)) > 0)
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
            if (IsRecording)
                _soundIn.Start();
        }

        public void Stop()
        {
            _soundIn?.Stop();
        }

        public void ChangeQuality(int sampleRate, int bitsPerSecond)
        {
            SampleRate = sampleRate;
            BitPerSecond = bitsPerSecond;

            _soundIn.Stop();
            _targetSoundSource = _defaultSoundSource
                .ChangeSampleRate(SampleRate)
                .ToSampleSource()
                .ToStereo()
                .ToWaveSource(BitPerSecond);
            _soundIn.Start();
        }

    }
}
