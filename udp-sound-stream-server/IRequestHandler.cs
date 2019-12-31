using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace udp_sound_stream_server
{
    interface IRequestHandler
    {
        void OnRequestRecieved(Request request);
    }
}
