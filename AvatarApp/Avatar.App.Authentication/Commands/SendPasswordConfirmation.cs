using Avatar.App.Authentication.Models;

namespace Avatar.App.Authentication.Commands
{
    public class SendPasswordConfirmation: SendUserConfirmation
    {
        public SendPasswordConfirmation(User user) : base(user)
        {
        }
    }
}
