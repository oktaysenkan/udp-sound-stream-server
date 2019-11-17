using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace udp_sound_stream_server
{
    class RequestHandler
    {
        private Request _request;

        public delegate void StreamStartedEventHandler(int sampleRate, int bitsPerSecond);
        public event StreamStartedEventHandler StreamStarted;

        public delegate void StreamStoppedEventHandler();
        public event StreamStoppedEventHandler StreamStopped;

        public delegate void AudioQualityChangedEventHandler(int sampleRate, int bitsPerSecond);
        public event AudioQualityChangedEventHandler AudioQualityChanged;

        public RequestHandler(Request request)
        {
            _request = request;
        }

        public void Start()
        {
            if (StreamStarted == null)
                throw new Exception("StreamStartedEventHandler not implemented");

            if (StreamStopped == null)
                throw new Exception("StreamStoppedEventHandler not implemented");

            if (AudioQualityChanged == null)
                throw new Exception("AudioQualityChangedEventHandler not implemented");

            if (_request.StartStream)
            {
                if (_request.BitPerSecond != 0 && _request.SampleRate != 0)
                    StreamStarted(_request.SampleRate, _request.BitPerSecond);
                else
                    StreamStarted(44100, 16);
            }
            else if (_request.StopStream)
            {
                StreamStopped();
            }
            else if (_request.ChangeAudioQuality)
            {
                if (_request.BitPerSecond == 0 && _request.SampleRate == 0)
                    throw new Exception("Bit per second or sample rate must not be null or empty.");

                AudioQualityChanged(_request.SampleRate, _request.BitPerSecond);
            }
        }
    }
}
