using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        public ChatUC()
        {
            InitializeComponent();
            this.DataContext = this;
            Messages = new ObservableCollection<Message>();


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
            foreach (var item in Messages)
            {
                if (item.FromClient)
                {
                    string bgColor = "LightBlue";
                    MessageUC messageUC = new MessageUC(item, bgColor);
                    messageUC.HorizontalAlignment = HorizontalAlignment.Right;
                    MainStack.Children.Add(messageUC);
                }
                else
                {
                    string bgColor = "LightGray";
                    string usernameColor = "orange";
                    MessageUC messageUC = new MessageUC(item, bgColor);
                    messageUC.UsernameColor = usernameColor; 
                    messageUC.HorizontalAlignment = HorizontalAlignment.Left;
                    MainStack.Children.Add(messageUC);
                }

            }



        }


        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
