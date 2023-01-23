using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using ChatProgram_ClientSide_Wpf.JsonHelper;

namespace ChatProgram_ClientSide_Wpf
{
    /// <summary>
    /// Interaction logic for ChatUC.xaml
    /// </summary>
    public partial class ChatUC : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        private ObservableCollection<Message> messages;

        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged(); }
        }

        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged(); }
        }
        public JsonSerializerSettings settings { get; set; }

        public ChatUC()
        {
            InitializeComponent();
            this.DataContext = this;
            Task.Run(() =>
            {
                try
                {

                    NetworkService.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            });
            Task.Run(() =>
            {
                while (true)
                {
                    if (App.CurrentClient.Connected)
                    {
                        App.CurrentUser.EndPoint = App.CurrentClient.Client.RemoteEndPoint;
                        App.CurrentUser.RemoteEndPoint = App.CurrentClient.Client.RemoteEndPoint.ToString();
                        MessageBox.Show("Remote End Point\n" + App.CurrentClient.Client.RemoteEndPoint.ToString());

                        settings = new JsonSerializerSettings();
                        settings.Converters.Add(new IPAddressConverter());
                        settings.Converters.Add(new IPEndPointConverter());
                        settings.Formatting = Formatting.Indented;
                        string jsonString = JsonConvert.SerializeObject(App.CurrentUser, settings);
                        NetworkService.SendMessageToServer(jsonString);
                        break;
                    }
                }
            });
            Messages = new ObservableCollection<Message>();

            //For test message
            InitializeTestMessages();


            foreach (var item in Messages)
            {
                AddMessageToUI(item);
            }



        }
        public void AddMessageToUI(Message message)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                MessageUC messageUC = new MessageUC();
                string bgColor = "";
                if (message.FromClient)
                {
                    bgColor = "LightBlue";
                    messageUC.HorizontalAlignment = HorizontalAlignment.Right;
                }
                else
                {
                    bgColor = "LightGray";
                    string usernameColor = "orange";
                    messageUC.UsernameColor = usernameColor;
                    messageUC.HorizontalAlignment = HorizontalAlignment.Left;
                }
                messageUC.BackGroundColor = bgColor;
                messageUC.ShortTime = message.dateTime.ToShortTimeString();
                messageUC.message = message;
                MainStack.Children.Add(messageUC);
            });
        }

        public Message CreateMessageClass(string text)
        {
            Message message = new Message();
            message.FromClient = true;
            message.message = text;
            message.User = App.CurrentUser;
            message.dateTime = DateTime.Now;
            return message;
        }
        public Message CreateMessageClass(string text, bool FromClient)
        {
            Message message = new Message();
            message.FromClient = FromClient;
            message.message = text;
            message.User = App.CurrentUser;
            message.dateTime = DateTime.Now;
            return message;
        }
        public void GetNewMessage(Message message)
        {
            var msg = CreateMessageClass(message.message, false);
            AddMessageToUI(msg);
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var msg = CreateMessageClass(text);
                var json = JsonConvert.SerializeObject(msg, settings);
                NetworkService.SendMessageToServer(json);
                AddMessageToUI(msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show("From Client Send Button\n" + ex.Message);
            }
            Text = string.Empty;


        }


        #region Test Region
        public void InitializeTestMessages()
        {
            Message message = new Message();
            message.message = "Hello";
            message.User = App.CurrentUser;
            message.FromClient = true;
            message.dateTime = DateTime.Now;
            Messages.Add(message);

            Message message2 = new Message();
            message2.message = "how are you";
            message2.User = App.CurrentUser;
            message2.FromClient = false;
            message2.dateTime = DateTime.Now;
            Messages.Add(message2);
        }


        //foreach (var item in messages)
        //{
        //    TextBlock textBlock = new TextBlock();
        //    textBlock.VerticalAlignment = VerticalAlignment.Top;
        //    textBlock.FontSize = 16;
        //    textBlock.HorizontalAlignment = HorizontalAlignment.Right;
        //    if (message.FromClient)
        //    {
        //        textBlock.Foreground = Brushes.Black;
        //    }
        //    else
        //    {
        //        textBlock.Foreground = Brushes.Red;
        //    }
        //    textBlock.Text = item.message;


        //    MainGrid.Children.Add(textBlock);
        //}
        #endregion
    }
}
