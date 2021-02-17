using MediatR;
using MimeKit;

namespace Avatar.App.Communications.Commands
{
    internal class SendEmail: IRequest
    {
        public SendEmail(string email, string subject, MimeEntity body)
        {
            Email = email;
            Subject = subject;
            Body = body;
        }

        public string Email { get; }
        public string Subject { get; }
        public MimeEntity Body { get; }
    }
}
