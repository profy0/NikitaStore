using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{

    public class EmailSettings
    {
        public string MailToAddress = "nikita-yurchik@mail.ru";
        public string MailFromAddress = "nikitayurchik123@gmail.com";
        public bool UseSsl = true;
        public string Username = "nikitayurchik123@gmail.com";
        public string Password = "ixgrwiinnsfrurho";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\nikita_store_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {

          /*  try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("nikitayurchik123@gmail.com", "ixgrwiinnsfrurho"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("nikitayurchik123@gmail.com"),
                    Subject = "Тестовое сообщение",
                    Body = "Это тестовое сообщение",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add("nikita-yurchik@mail.ru");

                smtpClient.Send(mailMessage);

                Console.WriteLine("Email успешно отправлено!");

                System.Diagnostics.Debug.WriteLine("EVERYTHING IS OKAY <3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EVERYTHING IS BAD <3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3<3");
                System.Diagnostics.Debug.WriteLine("An error occurred while sending the email: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    System.Diagnostics.Debug.WriteLine("Inner Exception Stack Trace: " + ex.InnerException.StackTrace);
                }
            }*/

             using (var smtpClient = new SmtpClient())
             {
                 smtpClient.EnableSsl = emailSettings.UseSsl;
                 smtpClient.Host = emailSettings.ServerName;
                 smtpClient.Port = emailSettings.ServerPort;
                 smtpClient.UseDefaultCredentials = false;
                 smtpClient.Credentials
                     = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                 if (emailSettings.WriteAsFile)
                 {
                     smtpClient.DeliveryMethod
                         = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                     smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                     smtpClient.EnableSsl = false;
                 }

                 StringBuilder body = new StringBuilder()
                     .AppendLine("Новый заказ обработан")
                     .AppendLine("---")
                     .AppendLine("Товары:");

                 foreach (var line in cart.Lines)
                 {
                     var subtotal = line.Product.Price * line.Quantity;
                     body.AppendFormat("{0} x {1} (итого: {2:c}",
                         line.Quantity, line.Product.Name, subtotal);
                 }

                 body.AppendFormat("Общая стоимость: {0:c}", cart.ComputeTotalValue())
                     .AppendLine("---")
                     .AppendLine("Доставка:")
                     .AppendLine(shippingInfo.Name)
                     .AppendLine(shippingInfo.Line)
                     .AppendLine(shippingInfo.City)
                     .AppendLine(shippingInfo.Country)
                     .AppendLine("---")
                     .AppendFormat("Срочная доставка: {0}",
                         shippingInfo.FastDelivery ? "Да" : "Нет");

                 MailMessage mailMessage = new MailMessage(
                                        emailSettings.MailFromAddress,	// От кого
                                        emailSettings.MailToAddress,		// Кому
                                        "Новый заказ отправлен!",		// Тема
                                        body.ToString()); 				// Тело письма

                 if (emailSettings.WriteAsFile)
                 {
                     mailMessage.BodyEncoding = Encoding.UTF8;
                 }

                 try
                 {
                     smtpClient.Send(mailMessage);
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine("An error occurred while sending the email: " + ex.Message);
                 }

               //  smtpClient.Send(mailMessage);
             }
        }
    }
}
