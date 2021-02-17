using Avatar.App.Authentication.Models;
using MediatR;

namespace Avatar.App.Authentication.Commands
{
    public class UpdateFireBase: IRequest
    {
        public User User { get; }

        public UpdateFireBase(User user)
        {
            User = user;
        }
    }
}
