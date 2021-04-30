using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace MailSystemDesktopApp
{
    /// <summary>
    /// Логика взаимодействия для MailSystem.xaml
    /// </summary>
    public partial class MailSystem : Window
    {
        Window loginWindow;
        public MailSystem(string userName, string userPassword, Window LoginWindow)
        {
            InitializeComponent();
            this.loginWindow = LoginWindow;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                this.Hide();
                this.loginWindow.Show();
                
            }
            finally
            {
                base.OnClosing(e);
            }
           
        }
    }
}
