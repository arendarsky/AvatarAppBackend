using Avatar.App.Authentication.Models;
using MediatR;

namespace Avatar.App.Authentication.Commands
{
    public abstract class SendUserConfirmation: IRequest
    {
        public User User { get; }

        protected SendUserConfirmation(User user)
        {
            User = user;
        }
    }
}
