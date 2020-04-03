using System.Threading.Tasks;
using Avatar.App.SharedKernel.Settings;
using Avatar.App.Core.Constants;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Avatar.App.Core.Services.Impl
{
    public class EmailService : IEmailService
    {
        private readonly AvatarAppSettings _avatarAppSettings;
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings, IOptions<AvatarAppSettings> avatarAppOptions)
        {
            _avatarAppSettings = avatarAppOptions.Value;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendConfirmCodeAsync(string email, string confirmCode, string guid)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailSettings.Name, _emailSettings.Email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = EmailMessages.ConfirmSubject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"{_avatarAppSettings.WebUrl}/auth/confirm?guid={guid}&confirmCode={confirmCode}"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port,
                _emailSettings.UseSsl);
            await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
