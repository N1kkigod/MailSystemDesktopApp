using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
using MailSystemDesktopApp.Models;

namespace MailSystemDesktopApp
{
    /// <summary>
    /// Логика взаимодействия для MailSystem.xaml
    /// </summary>
    public partial class MailSystem : Window
    {
        Window loginWindow;
        string userName;
        IList<Mail> mails = new List<Mail>();
        User thisUser;
        List<string> allUserNames;
        public MailSystem(string userName, string userPassword, Window LoginWindow)
        {
            InitializeComponent();
            this.loginWindow = LoginWindow;
            LoggedLabel.Content += userName;
            this.userName = userName;
            thisUser = API.getUserByUserId(API.getUserIdByUserName(userName));
            allUserNames = API.getAllUsers();
            foreach(string username in allUserNames)
                if(username!=thisUser.UserName)
                    addresserComboBox.Items.Add(username);
            addresserComboBox.SelectedIndex = 1;
            addresserComboBox.SelectedItem = 1; 
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

        private void mailTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MailUpdate();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MailUpdate();
        }
        private void Button_Click(Mail mail, Grid grid)
        {
            string mailText = "Title: " + mail.Title + "\n Date: " + mail.Date.ToString() + "\n Addressee: " + API.getUserByUserId(mail.AddresseeID).UserName + "\n Addresser: " + API.getUserByUserId(mail.AddresserID).UserName + "\n Content: " + mail.MailContent;
            grid.Children.Clear();
            grid.Children.Add(new TextBlock() { Text = mailText, HorizontalAlignment = HorizontalAlignment.Center });
            Button deleteButton = new Button() { Content = "Delete", HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment=VerticalAlignment.Top };
            deleteButton.Click += delegate { DeleteButton_Click(mail, grid); };
            grid.Children.Add(deleteButton);
        }
        private async void DeleteButton_Click(Mail mail, Grid grid)
        {
            Mail result = await API.deleteMail(mail.MailID);
            if(result!=null)
            {
                MessageBox.Show("Message deleted!", "Success!");
            }
            grid.Children.Clear();
            MailUpdate();
        }
        public void MailUpdate()
        {
            mails = API.getMailsByUserId(thisUser.UserID);
            ItemContainerTemplate mailTemplate = new ItemContainerTemplate();
            IList<StackPanel> mailPanels = new List<StackPanel>();
            IList<Button> mailButtons = new List<Button>();
            ScrollViewer thisScrollInc = new ScrollViewer(), thisScrollOut = new ScrollViewer();
            Grid thisAllMailGridNew = new Grid(), thisAllMailGridOutNew = new Grid();
            int rowNumber = 0;
            int rowNumberInp = 0;
            int rowNumberOut = 0;
            foreach (Mail mail in mails)
            {
                Button mailButton = new Button() { Name = "mailButton" + rowNumber };
                Grid thisAllMailGrid, thisMailGrid;
                if (Convert.ToInt32(thisUser.UserID) == mail.AddresserID)
                {
                    rowNumber = rowNumberInp;
                    thisMailGrid = mailGrid;
                    thisAllMailGrid = thisAllMailGridNew;
                }
                else
                {
                    rowNumber = rowNumberOut;
                    thisMailGrid = mailGridOut;
                    thisAllMailGrid = thisAllMailGridOutNew;
                }
                thisAllMailGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(60) });
                mailButton.Click += delegate { Button_Click(mail, thisMailGrid); };
                mailButton.Content = "Title: " + mail.Title + '\n' + "Addressee: " + API.getUserByUserId(mail.AddresseeID).UserName + '\n' + "Addresser: " + API.getUserByUserId(mail.AddresserID).UserName;
                mailButtons.Add(mailButton);
                thisAllMailGrid.Children.Add(mailButton);
                if (Convert.ToInt32(thisUser.UserID) == mail.AddresserID)
                {
                    Grid.SetRow(mailButton, rowNumberInp++);
                }
                else
                {
                    Grid.SetRow(mailButton, rowNumberOut++);
                }
            }

            thisScrollInc.Content = thisAllMailGridNew;
            thisScrollOut.Content = thisAllMailGridOutNew;
            allMailsGrid.Children.Clear();
            allMailsGridOut.Children.Clear();
            allMailsGrid.Children.Add(thisScrollInc);
            allMailsGridOut.Children.Add(thisScrollOut);
        }   

        private void sendMailButton_Click(object sender, RoutedEventArgs e)
        {
            Mail newMail = new Mail();
            newMail.AddresseeID = thisUser.UserID;
            newMail.AddresserID = API.getUserIdByUserName(addresserComboBox.SelectedValue.ToString());
            newMail.Date = DateTime.Now;
            newMail.Title = titleTextBox.Text;
            newMail.MailContent = contentTextBox.Text;
            newMail.MailID = 0;
            API.createMail(newMail);
            MailUpdate();
            MessageBox.Show("Mail send successfully!", "Success!");
            titleTextBox.Text = "";
            contentTextBox.Text = "";
            addresserComboBox.SelectedItem = 1;
        }
    }
}
