using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace udp_sound_stream_server
{
    class StreamController : IRequestHandler
    {
        public SoundStreamer _stream { get; set; }
        public StreamController(SoundStreamer stream)
        {
            _stream = stream;
        }

        public void OnRequestRecieved(Request request)
        {
            if (request.StartStream)
            {
                if (request.BitPerSecond != 0 && request.SampleRate != 0)
                    _stream.StartStream(request.SampleRate, request.BitPerSecond);
                else
                    _stream.StartStream(44100, 16);
            }
            else if (request.StopStream)
            {
                _stream.StopStream();
            }
            else if (request.ChangeAudioQuality)
            {
                if (request.BitPerSecond == 0 && request.SampleRate == 0)
                    throw new Exception("Bit per second or sample rate must not be null or empty.");

                _stream.ChangeAudioQuality(request.SampleRate, request.BitPerSecond);
            }
        }
    }
}
