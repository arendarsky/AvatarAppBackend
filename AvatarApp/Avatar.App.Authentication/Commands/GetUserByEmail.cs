using Avatar.App.Authentication.Models;
using MediatR;

namespace Avatar.App.Authentication.Commands
{
    public class GetUserByEmail: IRequest<User>
    {
        public GetUserByEmail(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
