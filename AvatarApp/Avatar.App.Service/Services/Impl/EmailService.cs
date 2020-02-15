using System.Threading.Tasks;
using Avatar.App.Entities.Settings;
using Avatar.App.Service.Constants;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Avatar.App.Service.Services.Impl
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendConfirmCodeAsync(string email, string confirmCode)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailSettings.Name, _emailSettings.Email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = EmailMessages.ConfirmSubject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = confirmCode
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port,
                    _emailSettings.UseSsl);
                await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
