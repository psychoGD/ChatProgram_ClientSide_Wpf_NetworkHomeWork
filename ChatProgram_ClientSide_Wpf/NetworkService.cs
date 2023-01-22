using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace ChatProgram_ClientSide_Wpf
{
    public class NetworkService
    {

        public static string GetLocalIpAddress()
        {
            UnicastIPAddressInformation mostSuitableIp = null;
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;
                var properties = network.GetIPProperties();
                if (properties.GatewayAddresses.Count == 0)
                    continue;
                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;
                    if (IPAddress.IsLoopback(address.Address))
                        continue;
                    if (!address.IsDnsEligible)
                    {
                        if (mostSuitableIp == null)
                            mostSuitableIp = address;
                        continue;
                    }
                    // The best IP is the IP got from DHCP server
                    if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    {
                        if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                            mostSuitableIp = address;
                        continue;
                    }
                    return address.Address.ToString();
                }
            }
            return mostSuitableIp != null
                ? mostSuitableIp.Address.ToString()
                : "";
        }
        public static void SendMessageToServer(string message)
        {
            var writer = Task.Run(() =>
            {
                try
                {
                    var stream = App.CurrentClient.GetStream();
                    var bw = new BinaryWriter(stream);
                    bw.Write(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("From Client SendMessageToServer \n" + ex.Message);
                }
            });

        }
        public static void Start()
        {
            var ip = IPAddress.Parse(GetLocalIpAddress());
            var ep = new IPEndPoint(ip, App.Port);
            try
            {
                App.CurrentClient.Connect(ep);
                if (App.CurrentClient.Connected)
                {
                    //var writer = Task.Run(() =>
                    //{
                    //    while (true)
                    //    {
                    //        var text = Console.ReadLine();
                    //        var stream = App.CurrentClient.GetStream();
                    //        var bw = new BinaryWriter(stream);
                    //        bw.Write(text);
                    //    }
                    //});

                    var reader = Task.Run(() =>
                    {
                        while (true)
                        {
                            var stream = App.CurrentClient.GetStream();
                            var br = new BinaryReader(stream);
                            MessageBox.Show($"From Server : {br.ReadString()}");
                        }
                    });

                    Task.WaitAll(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
