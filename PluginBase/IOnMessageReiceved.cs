using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginBase
{
    public interface IOnMessageReiceved
    {
        void OnMessageRecieved(IDictionary<string, string> Parameters);
    }
}
