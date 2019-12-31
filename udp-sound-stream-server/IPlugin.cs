using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace udp_sound_stream_server
{
    public interface IPlugin
    {
        void OnMessageRecieve(IDictionary<string, string> Parameters);
    }
}
