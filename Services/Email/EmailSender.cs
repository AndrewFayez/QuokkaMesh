using System.Net.Mail;
using System.Net;

namespace QuokkaMesh.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "petfriends411@gmail.com";
            var pw = "cblf tmjk lfun nioc";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };
            return client.SendMailAsync(
                new MailMessage(
                    from: mail,
                    to: email,
                    subject,
                    message
                    )
                );
        }

    }
}
