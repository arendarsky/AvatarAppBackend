using Avatar.App.Authentication.Models;
using MediatR;

namespace Avatar.App.Authentication.Commands
{
    public class UpdateUserConfirmation : IRequest
    {
        public User User { get; }

        public UpdateUserConfirmation(User user)
        {
            User = user;
        }
    }
}
