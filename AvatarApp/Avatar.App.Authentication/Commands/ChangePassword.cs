using Avatar.App.Authentication.Models;
using MediatR;

namespace Avatar.App.Authentication.Commands
{
    public class ChangePassword: IRequest
    {
        public User User { get; }

        public ChangePassword(User user)
        {
            User = user;
        }
    }
}
