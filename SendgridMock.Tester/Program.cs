using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SendgridMock.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press ENTER to send test email to sendgrid mock website...");
            Console.ReadLine();

            var client = new SendGridClient("gejrgpeojgpejrpjpejrg", "http://localhost:61853");
            var message = new SendGridMessage { From = new EmailAddress("johndoe@example.com", "John Doe") };
            message.AddTo(new EmailAddress("janedoe@example.com", "Jane Doe"));
            message.Subject = "This is awesome";
            message.AddCategory("ListingReply");
            message.HtmlContent = "<h1>This is so cool</h1>";
            var task = client.SendEmailAsync(message);
            task.Wait();
            var result = task.Result;
        }
    }
}
