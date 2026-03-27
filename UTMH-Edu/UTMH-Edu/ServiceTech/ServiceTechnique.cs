using System;
using System.Configuration;
using System.Net.Mail;

namespace UTMH_Edu.ServiceTech
{
    public class ServiceTechnique
    {
        public static void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                // ✅ Utiliser ton email et mot de passe d'application depuis Web.config
                string emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                string emailPass = ConfigurationManager.AppSettings["EmailPassword"];

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true; // obligatoire pour les liens cliquables

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(emailFrom, emailPass);
                smtp.EnableSsl = true;

                smtp.Send(mail); // envoi
            }
            catch (SmtpException ex)
            {
                throw new Exception("Erreur SMTP : " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'envoi de l'e-mail : " + ex.Message);
            }
        }


    }
}
