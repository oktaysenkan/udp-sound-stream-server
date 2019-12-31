using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udp_sound_stream_server
{
    class Request
    {
        public string Body { get; }
        public IDictionary<string, string> Parameters { get; }
        public bool StartStream { get; }
        public bool StopStream { get; }
        public bool ChangeAudioQuality { get; }
        public int BitPerSecond { get; }
        public int SampleRate { get; }

        public Request(byte[] buffer)
        {
            Body = Encoding.UTF8.GetString(buffer);
            Parameters = GetParameters(buffer);

            if (Parameters.ContainsKey("StartStream"))
                StartStream = Convert.ToBoolean(Parameters["StartStream"]);
            if (Parameters.ContainsKey("StopStream"))
                StopStream = Convert.ToBoolean(Parameters["StopStream"]);
            if (Parameters.ContainsKey("ChangeAudioQuality"))
                ChangeAudioQuality = Convert.ToBoolean(Parameters["ChangeAudioQuality"]);
            if (Parameters.ContainsKey("BitPerSecond"))
                BitPerSecond = Convert.ToInt32(Parameters["BitPerSecond"]);
            if (Parameters.ContainsKey("SampleRate"))
                SampleRate = Convert.ToInt32(Parameters["SampleRate"]);
        }

        private IDictionary<string, string> GetParameters(byte[] buffer)
        {
            string[] parametersArray = Body.Split('\n');
            Dictionary<string, string> requestParameters = new Dictionary<string, string>();
            foreach (string line in parametersArray)
            {
                if (String.IsNullOrEmpty(line))
                    continue;

                var colonIndex = line.IndexOf(":");
                var parameterName = line.Substring(0, colonIndex);
                var parameterValue = line.Substring(colonIndex + 2, line.Length - colonIndex - 2);

                requestParameters[parameterName] = parameterValue;
            }
            return requestParameters;
        }
    }
}
