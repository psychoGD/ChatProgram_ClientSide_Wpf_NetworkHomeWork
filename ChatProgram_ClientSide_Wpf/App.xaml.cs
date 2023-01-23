using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChatProgram_ClientSide_Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }
        public static TcpClient CurrentClient { get; set; } = new TcpClient();
        public static Grid MainGrid { get; set; }

        public const int Port = 27001;
        //From Client To Server
        public const string SpecialWordForServer = ";FCTS;";
        public static ChatUC chatUC { get; set; }
    }
}
