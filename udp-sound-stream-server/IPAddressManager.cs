using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace udp_sound_stream_server
{
    class IPAddressManager
    {
        public static List<IPAddress> GetLocalIPAddress()
        {
            List<IPAddress> ipAddresses = new List<IPAddress>();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddresses.Add(ip);
                }
            }

            if (ipAddresses.Count > 0)
                return ipAddresses;

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
