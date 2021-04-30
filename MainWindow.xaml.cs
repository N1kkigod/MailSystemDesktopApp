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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //using (SqlConnection sqlConnection = new SqlConnection("Data Source = localhost; Initial Catalog = mailSystem; Integrated Security = True")) 
            //{
            //    string username = loginTextBox.Text;
            //    string userPassword = passwordTextBox.Text;
            //    SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Users WHERE UserName = \'" + username + "\' and Password = \'" + userPassword + "\'", sqlConnection);
            //    try {
            //        sqlConnection.Open();
            //        SqlDataReader reader = sqlCommand.ExecuteReader();
            //        if(reader.Read())
            //        {
            //            this.Hide();
            //            MailSystem mailSystem = new MailSystem(username, userPassword, this);
            //            mailSystem.Show();
            //        }
            //        else
            //        {

            //            MessageBox.Show("Login or password is wrong!", "Error!");
            //        };
            //    }
            //    catch
            //    {
            //        MessageBox.Show("Error with connection!", "Database error!");
            //    }

            //}
            //HttpClientHandler httpClientHandler = new HttpClientHandler();
            //var tasks = new List<Task>();
            using (var httpClient = new HttpClient())
            {
                //httpClient.BaseAddress = new Uri("http://localhost:44371/api");
                //HttpRequestMessage Query = new HttpRequestMessage();
                //Query = "/mails";
                //var response = await httpClient.GetAsync("/mails");// + loginTextBox.Text + "&password=" + passwordTextBox.Text);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("http://localhost:44371/api/mails"),
                };
                var content = await GetHttpAnswer(loginTextBox.Text, passwordTextBox.Text);
                if(content.ToString() != "NotFound")
                {
                    this.Hide();
                    MailSystem mailSystem = new MailSystem(loginTextBox.Text, passwordTextBox.Text, this);
                    mailSystem.Show();
                }
                //var tasks = new List<Task>();
                //tasks.Add(GetHttpAnswer(httpClient,request));
                //task.Wait(GetHttpAnswer(httpClient));
                //MessageBox.Show(response.StatusCode.ToString());
                //var response = client.GetAsync(url);
            }
            //HttpClient httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri("http://localhost:44371/api");
        }
        public static async Task<string> GetHttpAnswer(string login, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("auth", login + " " + password);
                var response = await client.GetAsync("https://localhost:44371/api/User?login=" + login +"&password=" + password);
                string content = response.StatusCode.ToString();
                MessageBox.Show(content);
                return content;
                //Console.ReadLine();
            }
        }
    }
}
