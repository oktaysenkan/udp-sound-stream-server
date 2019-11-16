using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace udp_sound_stream_server
{
    class RequestHandler
    {
        public string Request { get; }
        public bool StartStream { get; set; }
        public bool PauseStream { get; }
        public bool StopStream { get; }
        public bool ChangeAudioQuality { get; }
        public int BitPerSecond { get; }
        public int SampleRate { get; }

        public RequestHandler(byte[] buffer)
        {
            Request = Encoding.UTF8.GetString(buffer);

            StartStream = GetValueOfParameter("StartStream", new bool());
            PauseStream = GetValueOfParameter("PauseStream", new bool());
            StopStream = GetValueOfParameter("StopStream", new bool());
            ChangeAudioQuality = GetValueOfParameter("ChangeAudioQuality", new bool());
            BitPerSecond = GetValueOfParameter("BitPerSecond", new int());
            SampleRate = GetValueOfParameter("SampleRate", new int());
        }

        private T GetValueOfParameter<T>(string requestParamater, T type)
        {
            var startIndex = Request.IndexOf(requestParamater, StringComparison.Ordinal);
            if (startIndex < 0)
                return default(T);

            var equalsIndex = startIndex + requestParamater.Length;
            var commaIndex = Request.IndexOf(",", equalsIndex, StringComparison.Ordinal);
            var value = Request.Substring(equalsIndex + 1, commaIndex - equalsIndex - 1);

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
