using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace CommonBox
{
    public class MailBox
    {
        //Them email pass vao day nha cac ban
        static readonly string fromEmailAddress = "";
        static readonly string fromEmailPassword = "";
        static readonly string fromEmailDisplayName = "Book Management System";
        static readonly string smtpHost = "smtp.gmail.com";
        static readonly string smtpPort = "587";

        public static void SendMail(string toEmail, string subject, string content)
        {
            bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());
            string body = content;
            StringBuilder sbBody = new StringBuilder(body);
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmail));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = sbBody.ToString();

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enabledSsl;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);
        }
        public static void SendMailAuthentication(string toEmail, string subject, string content,string lang)
        {

            bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

            string body = null;
            if (lang == "vi" || lang == null)
            {
                body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/Assets/vendors/pages/authenticationform_Vi.html"));
            }
            else
            {
                body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/Assets/vendors/pages/authenticationform.html"));
            }
            StringBuilder sbBody = new StringBuilder(body);
            sbBody.Replace("@ViewBag.AuthenticationLink", content);
            sbBody.Replace("@ViewBag.MonthYear", System.DateTime.Now.ToShortDateString());
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmail));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = sbBody.ToString();

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enabledSsl;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);
        }
    }
}
