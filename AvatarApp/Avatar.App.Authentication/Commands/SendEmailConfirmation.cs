using Avatar.App.Authentication.Models;

namespace Avatar.App.Authentication.Commands
{
    public class SendEmailConfirmation: SendUserConfirmation
    {
        public SendEmailConfirmation(User user) : base(user)
        {
        }
    }
}
