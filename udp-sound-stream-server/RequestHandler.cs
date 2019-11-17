using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udp_sound_stream_server
{
    class RequestHandler
    {
        public string Request { get; }
        public bool StartStream { get; }
        public bool StopStream { get; }
        public bool ChangeAudioQuality { get; }
        public int BitPerSecond { get; }
        public int SampleRate { get; }

        public RequestHandler(byte[] buffer)
        {
            Request = Encoding.UTF8.GetString(buffer);

            StartStream = GetValueOfParameter("StartStream", StartStream);
            StopStream = GetValueOfParameter("StopStream", StopStream);
            ChangeAudioQuality = GetValueOfParameter("ChangeAudioQuality", ChangeAudioQuality);
            BitPerSecond = GetValueOfParameter("BitPerSecond", BitPerSecond);
            SampleRate = GetValueOfParameter("SampleRate", SampleRate);
        }

        private T GetValueOfParameter<T>(string requestParamater, T type)
        {
            var startIndex = Request.IndexOf(requestParamater, StringComparison.Ordinal);
            if (startIndex < 0)
                return default(T);

            var colonSignIndex = startIndex + requestParamater.Length;
            var newLineIndex = Request.IndexOf("\n", colonSignIndex, StringComparison.Ordinal);
            var value = Request.Substring(colonSignIndex + 2, newLineIndex - colonSignIndex - 2);

            if (type is bool)
                return (T)(object)Convert.ToBoolean(value);

            if (type is string)
                return (T)(object)value;

            if (type is int)
                return (T)(object)Convert.ToInt32(value);

            return default(T);
        }

    }
}
