﻿using PizzAkuten.Models;
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

        public Task SendOrderConfirmToUser(Order order)
        {
            string from = "mariakallstrom@outlook.com";
            string to = "maria@siso.se";
            string subject = "Orderbekräftelse";
            string htmlBody = "Hej! <br> <br> Tack för att du lagt en order hos oss! <br><br>";

            htmlBody += "<table>"; 
            htmlBody += "<tr><td> Namn </td><td>Pris</td><td>Antal</td></tr>";
            foreach (var item in order.OrderDish.OrderItems)
            {
                htmlBody += "<tr><td>" + item.OrderDish.Name + "</td><td>" + item.OrderDish.Price + "</td><td>" + item.Quantity + "</td></tr>";
            }
            
            htmlBody += "</table>";
            htmlBody += "Summa: " + order.TotalPrice;

            MailMessage mailMessage = new MailMessage(from, to, subject, htmlBody);
            mailMessage.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com");

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("mariakallstrom@outlook.com", "");
            client.EnableSsl = true;
            client.Credentials = credentials;
            client.Send(mailMessage);
            return Task.CompletedTask;
        }
    }
}
