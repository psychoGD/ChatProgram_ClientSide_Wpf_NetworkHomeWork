using System;
using System.Collections.Generic;
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
    /// Interaction logic for LoginUC.xaml
    /// </summary>
    public partial class LoginUC : UserControl, INotifyPropertyChanged
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
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value;OnPropertyChanged(); }
        }

        public LoginUC()
        {
            InitializeComponent();
            this.DataContext= this;
            username = "testName";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Username != null && Username != "" && Username != " ")
                {
                    App.CurrentUser = new User();
                    App.CurrentUser.Username = Username;
                    App.MainGrid.Children.RemoveAt(0);
                    ChatUC chatUC = new ChatUC();
                    App.chatUC = chatUC; 
                    App.MainGrid.Children.Add(App.chatUC);
                }
                else
                {
                    MessageBox.Show("Fill The Name");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
