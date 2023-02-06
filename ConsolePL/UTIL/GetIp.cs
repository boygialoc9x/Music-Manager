using System;
using System.Net;
using System.Net.Sockets;

namespace ConsolePL.UTIL
{    public class GetIP
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public static bool CheckConnect()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
//System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();