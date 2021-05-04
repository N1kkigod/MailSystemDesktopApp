using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSystemDesktopApp.Models
{
    public class Mail
    {
        public int MailID { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int AddresseeID { get; set; }
        public int AddresserID { get; set; }
        public string MailContent { get; set; }
    }
}
