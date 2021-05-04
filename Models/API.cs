using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MailSystemDesktopApp.Models
{
    public static class API
    {
        private static readonly Uri baseUri = new Uri("https://localhost:44371");
        static API()
        {
        }
        public static IList<Mail> getMailsByUserId(int userId)
        {
            using (HttpClient client = new HttpClient() { BaseAddress = baseUri })
            {
                Task<IList<Mail>> task = client.GetFromJsonAsync<IList<Mail>>("/mails/" + userId);
                return task.Result;
            }
        }
        public static User getUserByUserId(int userId)
        {
            using (HttpClient client = new HttpClient() { BaseAddress = baseUri })
            {
                Task<User> task = client.GetFromJsonAsync<User>("/getUserByUserId?userId=" + userId);
                return task.Result;
            }
        }
        public static int getUserIdByUserName(string userName)
        {
            using (HttpClient client = new HttpClient() { BaseAddress = baseUri })
            {
                Task<int> task = client.GetFromJsonAsync<int>("/getUserId?userName=" + userName);
                return task.Result;
            }
        }
        public static async void createMail(Mail mail)
        {
            using (HttpClient client = new HttpClient() { BaseAddress = baseUri })
            {
                string jsonMail = JsonConvert.SerializeObject(mail);
                var httpContent = new StringContent(jsonMail, Encoding.UTF8, "application/json");
                var task = await client.PostAsync("/api/Mail", httpContent);
            }
        }
        public static List<string> getAllUsers()
        {
            using(HttpClient client = new HttpClient() { BaseAddress = baseUri })
            {
                Task<List<string>> task = client.GetFromJsonAsync<List<string>>("/getAllUserNames");
                return task.Result;
            }
        }
        public static async Task<string> checkUser(string login, string password)
        {
            using(HttpClient client = new HttpClient() { BaseAddress = baseUri })
            {
                client.DefaultRequestHeaders.Add("auth", login + " " + password);
                var response = client.GetAsync("api/User?login=" + login + "&password=" + password);
                return response.Result.StatusCode.ToString();
            }
        }
        public static async Task<Mail> deleteMail(int mailId)
        {
            using(HttpClient client = new HttpClient() { BaseAddress = baseUri })
            {
                var result = client.GetAsync("/api/Mail?mailID=" + mailId).Result;
                var response = client.DeleteAsync("/api/Mail?mailID=" + mailId);
                return await response.Result.Content.ReadFromJsonAsync<Mail>();
            }
        }
    }
}
