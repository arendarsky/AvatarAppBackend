using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Authentication.Commands;
using Avatar.App.Authentication.Models;
using Avatar.App.SharedKernel;
using MediatR;

namespace Avatar.App.Authentication
{
    public interface IAuthenticationComponent
    {
        Task<User> GetUser(Guid guid);
    }

    internal class AuthenticationComponent: AvatarAppComponent, IAuthenticationComponent
    {
        public AuthenticationComponent(IMediator mediator) : base(mediator)
        {
        }

        public async Task<User> GetUser(Guid guid)
        {
            return await Mediator.Send(new GetUserByGuid(guid));
        }
    }
}
