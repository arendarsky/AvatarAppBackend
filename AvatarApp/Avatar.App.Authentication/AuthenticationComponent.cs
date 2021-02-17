using System;
using System.Threading.Tasks;
using Avatar.App.Authentication.CData;
using Avatar.App.Authentication.Commands;
using Avatar.App.Authentication.Models;
using Avatar.App.Authentication.Security;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Authentication
{
    public interface IAuthenticationComponent
    {
        Task<User> GetUser(Guid guid);
        Task<User> GetUser(string email);
        Task<User> GetUser(long id);
        Task<bool> TryRegister(UserCData userCData);
        Task<JwtUser> GetAuthorizationToken(string email, string password);
        Task<bool> TrySendEmailConfirmation(User user);
        Task<bool> TrySendPasswordReset(User user);
        Task<bool> TryConfirmEmail(User user, string confirmCode);
        Task<bool> TryResetPassword(User user, string confirmCode, string password);
        Task<bool> TryChangePassword(User user, string oldPassword, string newPassword);
        Task SetFireBaseId(User user, string fireBaseId);
    }

    internal class AuthenticationComponent: AvatarAppComponent, IAuthenticationComponent
    {
        private readonly IJwtSigningEncodingKey _signingEncodingKey;

        public AuthenticationComponent(IMediator mediator, IJwtSigningEncodingKey signingEncodingKey) : base(mediator)
        {
            _signingEncodingKey = signingEncodingKey;
        }

        public async Task<User> GetUser(Guid guid)
        {
            return await Mediator.Send(new GetUserByGuid<User>(guid));
        }

        public async Task<User> GetUser(string email)
        {
            return await Mediator.Send(new GetUserByEmail(email));
        }

        public async Task<User> GetUser(long id)
        {
            return await Mediator.Send(new GetById<User>(id));
        }

        public async Task<bool> TryRegister(UserCData userCData)
        {
            if (!User.IsEmailValid(userCData.Email) || !await CheckUserExistenceAsync(userCData.Email))
            {
                return false;
            }

            await Mediator.Send(new Insert<UserCData>(userCData));
            return true;
        }

        private async Task<bool> CheckUserExistenceAsync(string email)
        {
            var userQuery = await Mediator.Send(new GetQuery<User>());
            return await userQuery.AnyAsync(
                user => string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<JwtUser> GetAuthorizationToken(string email, string password)
        {
            var user = await GetUser(email);
            return new JwtUser(user, password, _signingEncodingKey);
        }

        public async Task<bool> TrySendEmailConfirmation(User user)
        {
            await UpdateUserConfirmation(user);
            await Mediator.Send(new SendEmailConfirmation(user));
            return true;
        }
        public async Task<bool> TrySendPasswordReset(User user)
        {
            await UpdateUserConfirmation(user);
            await Mediator.Send(new SendPasswordConfirmation(user));
            return true;
        }

        private async Task UpdateUserConfirmation(User user)
        {
            user.SetConfirmationCode();
            await Mediator.Send(new UpdateUserConfirmation(user));
        }

        public async Task<bool> TryConfirmEmail(User user, string confirmCode)
        {
            if (!CheckConfirmation(user, confirmCode)) return false;
            user.IsEmailConfirmed = true;
            user.ConfirmationCode = null;
            await Mediator.Send(new UpdateUserConfirmation(user));
            return true;
        }

        public async Task<bool> TryResetPassword(User user, string confirmCode, string password)
        {
            if (!CheckConfirmation(user, confirmCode)) return false;
            user.ConfirmationCode = null;
            user.Password = User.HashPassword(password);
            await Mediator.Send(new ChangePassword(user));
            return true;
        }

        public async Task<bool> TryChangePassword(User user, string oldPassword, string newPassword)
        {
            if (!user.IsPasswordCorrect(oldPassword)) return false;
            user.Password = User.HashPassword(newPassword);
            await Mediator.Send(new ChangePassword(user));
            return true;
        }

        private static bool CheckConfirmation(User user, string confirmCode)
        {
            return string.Equals(user.ConfirmationCode, confirmCode, StringComparison.Ordinal);
        }

        public async Task SetFireBaseId(User user, string fireBaseId)
        {
            user.FireBaseId = fireBaseId;
            await Mediator.Send(new UpdateFireBase(user));
        }
    }
}
