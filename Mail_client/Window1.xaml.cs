using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.Collections.ObjectModel;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MailKit;
using MimeKit;

namespace Mail_client
{
    public partial class Window1 : Window
    {
        string Mail { get; set; }
        string Password { get; set; }
        int Num { get; set; }
        List<string> FileName { get; set; }
        ObservableCollection<Folder> folder = new ObservableCollection<Folder>();
        public Window1(string mail, string password, int num)
        {
            InitializeComponent();
            Mail = mail;
            Password = password;
            Num = num;
            FileName = new List<string>();
            Folders.ItemsSource = folder;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

                client.Authenticate(Mail, Password);

                foreach (var item in client.GetFolders(client.PersonalNamespaces[0]))
                {
                    Folder folder = new Folder();
                    folder.FolderName = item.Name;
                    foreach (var uid in client.Inbox.Search(SearchQuery.All))
                    {
                        var m = client.Inbox.GetMessage(uid);
                        Console.WriteLine($"Mail: {m.Subject} - {new string(m.TextBody.Take(10).ToArray())}...");
                    }
                }
            }
        }
    }
    public class Folder
    {
        public Folder()
        {
            this.mail1 = new ObservableCollection<Mail>();
        }
        public string FolderName { get; set; }
        public ObservableCollection<Mail> mail1 { get; set; }
    }
    public class Mail
    {
        public string MailName { get; set; }
    }
}
