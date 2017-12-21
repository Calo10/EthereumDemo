using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EthereumWeb.Models
{
    public class SentMail
    {

        public void SentEmail(string mensaje, string destinatario,string From,string subject)
        {

            EmailMasagge emailMesagge = new EmailMasagge();
            string htmlTemplade = "<table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td style='padding: 10px 0 30px 0;'><table align='center' border='0' cellpadding='0' cellspacing='0' width='600' style='border: 1px solid #cccccc; border-collapse: collapse;'>" +
"<tr><td align='center' bgcolor='#70bbd9' style='padding: 40px 0 30px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif;'>" +
"</td></tr><tr Style='font-family: Calibri'><td bgcolor='#ffffff' style='padding: 40px 30px 40px 30px; font-family: Calibri'>{0}" +
"</td></tr><tr><td bgcolor='#0a4871' style='padding: 30px 30px 30px 30px;'><table border='0' cellpadding='0' cellspacing='0' width='100%'><tr>" +
"<td style='color: #ffffff; font-family: Calibri; font-size: 14px;' width='75%'>Copyright 2017 #YoAsumo | Todos los Derechos Reservados<br/>" +
"<a href=' http://raykel.eastus.cloudapp.azure.com/EthereumWeb/' style='color: #ffffff;'><font color='#ffffff'>Ingreso</font></a> </td><td align='right' width='25%'></td></tr></table></td></tr></table></td></tr></table>";
            emailMesagge.AdressSMTP = "smtp.gmail.com";
            emailMesagge.Body = string.Format(htmlTemplade, mensaje);
            emailMesagge.CredencialPass = "prosoft123";
            emailMesagge.CredencialUser = "testprosofter@gmail.com";
            emailMesagge.Emails = new List<string> { destinatario };
            emailMesagge.EnableCredencial = true;
            emailMesagge.EnableSSL = true;
            emailMesagge.PortSMTP = 587;
            emailMesagge.From = From;
            emailMesagge.Subject = subject;

            Email(emailMesagge);
        }
        public void Email(EmailMasagge emailMesagge) { 
             try
            {
                //emailMesagge.Emails = new List<string>() { "melendezaj@prosoft.cr" };
                //TODO: Eliminar siguiente linea
                //
                
        MailMessage mailMessage = new MailMessage();
        SmtpClient client = new SmtpClient();

                foreach (var destinatario in emailMesagge.Emails)
                {
                    mailMessage.To.Add(destinatario);
                }
                
                mailMessage.Subject = emailMesagge.Subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = emailMesagge.Body;
                mailMessage.From = new MailAddress(emailMesagge.CredencialUser, emailMesagge.From);
                client.UseDefaultCredentials = false;

                if (emailMesagge.EnableCredencial) {
                 NetworkCredential cred = new NetworkCredential(emailMesagge.CredencialUser, emailMesagge.CredencialPass);
                 client.Credentials = cred;
                }
                client.Port = emailMesagge.PortSMTP ;
                client.Host = emailMesagge.AdressSMTP ;
                client.EnableSsl = emailMesagge.EnableSSL;
             
                client.Send(mailMessage);
              
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}
