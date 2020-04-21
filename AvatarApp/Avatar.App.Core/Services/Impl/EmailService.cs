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
        private readonly SmtpClient _smtpClient;
        private readonly AvatarAppSettings _avatarAppSettings;
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings, IOptions<AvatarAppSettings> avatarAppOptions, SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
            _avatarAppSettings = avatarAppOptions.Value;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendConfirmCodeAsync(string email, string confirmCode, string guid)
        {
            var emailBody = CreateConfirmEmailBody(guid, confirmCode);

            var emailMessage = CreateEmailMessage(email, EmailMessages.ConfirmSubject, emailBody);

            await SendMessageAsync(emailMessage);
        }

        public async Task SendPasswordResetAsync(string email, string confirmCode, string guid)
        {
            var emailBody = CreatePasswordResetBody(guid, confirmCode);

            var emailMessage = CreateEmailMessage(email, EmailMessages.ResetPasswordSubject, emailBody);

            await SendMessageAsync(emailMessage);
        }

        #region Private Methods

        private MimeMessage CreateEmailMessage(string email, string subject, MimeEntity body)
        {
            var emailMessage = new MimeMessage { Subject = subject, Body = body };
            emailMessage.From.Add(new MailboxAddress(_emailSettings.Name, _emailSettings.Email));
            emailMessage.To.Add(new MailboxAddress("", email));

            return emailMessage;
        }

        private MimeEntity CreateConfirmEmailBody(string guid, string confirmCode)
        {
            return new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"{_avatarAppSettings.WebUrl}/auth/confirm?guid={guid}&confirmCode={confirmCode}"
            };
        }

        private MimeEntity CreatePasswordResetBody(string guid, string confirmCode)
        {
            return new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"{_avatarAppSettings.WebUrl}/auth/password/change?guid={guid}&confirmCode={confirmCode}"
            };
        }

        private async Task SendMessageAsync(MimeMessage emailMessage)
        {
            await _smtpClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port,
                _emailSettings.UseSsl);
            await _smtpClient.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
            await _smtpClient.SendAsync(emailMessage);

            await _smtpClient.DisconnectAsync(true);
        }

        #endregion
    }
}
