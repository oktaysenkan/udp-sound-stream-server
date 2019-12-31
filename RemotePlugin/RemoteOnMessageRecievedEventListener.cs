using PluginBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotePlugin
{
    public class RemoteOnMessageRecievedEventListener : IOnMessageReiceved  
    {
        public void OnMessageRecieved(IDictionary<string, string> parameters)
        {
            Debug.WriteLine("Test");
        }
    }
}
