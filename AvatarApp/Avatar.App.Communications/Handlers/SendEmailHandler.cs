using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Communications.Commands;
using Avatar.App.Communications.Settings;
using MailKit.Net.Smtp;
using MediatR;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Avatar.App.Communications.Handlers
{
    internal class SendEmailHandler: IRequestHandler<SendEmail>
    {
        private readonly SmtpClient _smtpClient;
        private readonly EmailSettings _emailSettings;

        public SendEmailHandler(IOptions<EmailSettings> emailSettingsOptions, SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
            _emailSettings = emailSettingsOptions.Value;
        }

        public async Task<Unit> Handle(SendEmail request, CancellationToken cancellationToken)
        {
            var message = CreateEmailMessage(request.Email, request.Subject, request.Body);
            await SendMessageAsync(message);
            return Unit.Value;
        }

        private MimeMessage CreateEmailMessage(string email, string subject, MimeEntity body)
        {
            var emailMessage = new MimeMessage { Subject = subject, Body = body };
            emailMessage.From.Add(new MailboxAddress(_emailSettings.Name, _emailSettings.Email));
            emailMessage.To.Add(new MailboxAddress("", email));
            return emailMessage;
        }

        private async Task SendMessageAsync(MimeMessage emailMessage)
        {
            await _smtpClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port,
                _emailSettings.UseSsl);
            await _smtpClient.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
            await _smtpClient.SendAsync(emailMessage);

            await _smtpClient.DisconnectAsync(true);
        }
    }
}
