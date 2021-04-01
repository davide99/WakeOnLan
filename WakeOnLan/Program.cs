using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Forms;

namespace WakeOnLan
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static IPAddress GetLocalIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            throw new Exception("No network adapter with IPv4 on this system");
        }

        public static IPAddress GetLocalMask(IPAddress localIP)
        {
            foreach(var adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach(var unicastIpAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if(unicastIpAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (localIP.Equals(unicastIpAddressInformation.Address))
                        {
                            return unicastIpAddressInformation.IPv4Mask;
                        }
                    }
                }
            }

            throw new ArgumentException("Errore");
        }

        public static IPAddress GetNetworkIP(IPAddress ip, IPAddress mask)
        {
            var ipBytes = ip.GetAddressBytes();
            var maskBytes = mask.GetAddressBytes();

            var networkIp = new byte[ipBytes.Length];
            for(int i = 0; i< networkIp.Length; i++)
            {
                networkIp[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }

            return new IPAddress(networkIp);
        }

        public static int GetMaxHostNumber(IPAddress mask)
        {
            var maskBytes = mask.GetAddressBytes();

            for(int i = 0; i<maskBytes.Length; i++)
                maskBytes[i] = (byte)~maskBytes[i];

            if (BitConverter.IsLittleEndian)
                Array.Reverse(maskBytes);

            return BitConverter.ToInt32(maskBytes) - 1;
        }

        public static IPAddress GetNextIp(IPAddress ip)
        {
            var ipBytes = ip.GetAddressBytes();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(ipBytes);

            var nextIpUInt32 = BitConverter.ToUInt32(ipBytes) + 1;
            var nextIpBytes = BitConverter.GetBytes(nextIpUInt32);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(nextIpBytes);

            return new IPAddress(nextIpBytes);
        }

        public static bool IsIpReachable(IPAddress ip)
        {
            Ping ping = new Ping();
            PingReply pingReply = ping.Send(ip, 100);

            return pingReply.Status == IPStatus.Success;
        }
    }
}
