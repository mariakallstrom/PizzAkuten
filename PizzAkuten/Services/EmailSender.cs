using PizzAkuten.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PizzAkuten.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }

        public Task SendOrderConfirmToUser(Order order, ApplicationUser user)
        {

           
            string from = "mariakallstrom@outlook.com";
            string to = "maria@siso.se";
            string subject = "Orderbekräftelse";
            string htmlBody = "Hej! <br> <br> Tack för att du lagt en order hos oss på PizzAkuten! <br><br> Din beställning är på väg! <br><br>";

            htmlBody += "<table>"; 
            htmlBody += "<tr><td> Namn </td><td>Pris</td><td>Antal</td></tr>";
            foreach (var item in order.OrderDish.OrderItems)
            {
                htmlBody += "<tr><td>" + item.OrderDish.Name + "</td><td>" + item.OrderDish.Price + "</td><td>" + item.Quantity + "</td></tr>";
            }
            
            htmlBody += "</table>";
            htmlBody += "Summa: " + order.TotalPrice;

            htmlBody += "<br> Order kommer sändas till följande adress: " +
                "<br>" + user.FirstName + "<br>" + user.LastName + "<br>" + user.Street + "<br>" + user.ZipCode + "<br>" +user.City + "<br>";

            htmlBody += "Har du några frågor kontakta: PizzAkuten Tel: 123456789";


          MailMessage mailMessage = new MailMessage(from, to, subject, htmlBody);
            mailMessage.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com");

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("mariakallstrom@outlook.com", "Skylight542");
            client.EnableSsl = true;
            client.Credentials = credentials;
            client.Send(mailMessage);
            return Task.CompletedTask;
        }

        public Task SendOrderConfirmToUser(Order order, NonAccountUser user)
        {
            string from = "mariakallstrom@outlook.com";
            string to = "maria@siso.se";
            string subject = "Orderbekräftelse";
            string htmlBody = "Hej! <br> <br> Tack för att du lagt en order hos oss på PizzAkuten! <br><br> Din beställning är på väg! <br><br>";

            htmlBody += "<table>";
            htmlBody += "<tr><td> Namn </td><td>Pris</td><td>Antal</td></tr>";
            foreach (var item in order.OrderDish.OrderItems)
            {
                htmlBody += "<tr><td>" + item.OrderDish.Name + "</td><td>" + item.OrderDish.Price + "</td><td>" + item.Quantity + "</td></tr>";
            }

            htmlBody += "</table>";
            htmlBody += "Summa: " + order.TotalPrice;

            htmlBody += "<br> Order kommer sändas till följande adress: " +
                "<br>" + user.FirstName + "<br>" + user.LastName + "<br>" + user.Street + "<br>" + user.ZipCode + "<br>" + user.City + "<br>";

            htmlBody += "Har du några frågor kontakta: PizzAkuten Tel: 123456789";


            MailMessage mailMessage = new MailMessage(from, to, subject, htmlBody);
            mailMessage.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com");

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("mariakallstrom@outlook.com", "Skylight542");
            client.EnableSsl = true;
            client.Credentials = credentials;
            client.Send(mailMessage);
            return Task.CompletedTask;
        }
    }
}
