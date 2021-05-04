using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net;
using System.Net.Http;
using MailSystemDesktopApp.Models;

namespace MailSystemDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var content = API.checkUser(loginTextBox.Text, passwordTextBox.Text);
            if(content.ToString() != "NotFound")
            {
                this.Hide();
                MailSystem mailSystem = new MailSystem(loginTextBox.Text, passwordTextBox.Text, this);
                mailSystem.Show();
            }
        }
    }
}
