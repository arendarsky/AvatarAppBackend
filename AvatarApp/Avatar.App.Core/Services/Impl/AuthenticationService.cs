using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Helpers;
using Avatar.App.Core.Models;
using Avatar.App.Core.Security;
using Avatar.App.Core.Specifications.UserSpecifications;
using Avatar.App.SharedKernel.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Avatar.App.Core.Services.Impl
{
    public class AuthenticationService: BaseServiceWithAuthorization, IAuthenticationService
    {
        private readonly IEmailService _mailService;
        private readonly IJwtSigningEncodingKey _signingEncodingKey;
        private readonly IMemoryCache _cache;
        private const string PasswordPrefix = "password_";

        public AuthenticationService(IEmailService mailService, IJwtSigningEncodingKey signingEncodingKey, IMemoryCache memoryCache, IRepository<User> userRepository) : base(userRepository)
        {
            _mailService = mailService;
            _signingEncodingKey = signingEncodingKey;
            _cache = memoryCache;
        }

        public async Task RegisterAsync(UserDto userDto)
        {
            if (!await CheckUserExistenceAsync(new UserSpecification(userDto.Email)))
                throw new UserAlreadyExistsException();

            await CreateUserAsync(userDto);
        }

        public async Task<AuthorizationDto> GetAuthorizationTokenAsync(string email, string password)
        {
            var user = await GetUserAsync(new UserSpecification(email)); ;

            if (!CheckUserPassword(user, password)) throw new InvalidPasswordException();

            return new AuthorizationDto
            {
                Token = user.IsEmailConfirmed ? CreateJwtToken(user) : null,
                ConfirmationRequired = !user.IsEmailConfirmed
            };
        }

        public async Task SendEmailAsync(string email)
        {
            var guid = await GetUserGuidAsync(email);

            var confirmCode = ConfirmCodeHelper.CreateRandomCode();
            SaveCodeToCache(guid, confirmCode);
            
            await _mailService.SendConfirmCodeAsync(email, confirmCode, guid);
        }

        public async Task<bool> ConfirmEmailAsync(string guid, string confirmCode)
        {
            if (!Guid.TryParse(guid, out var userGuid)) return false;

            var user = await GetUserAsync(new UserSpecification(userGuid));

            if (user.IsEmailConfirmed) return true;

            if (!CheckConfirmCode(confirmCode, guid)) return false;

            SetUserEmailConfirmed(user);

            return true;
        }

        public async Task SendPasswordReset(string email)
        {
            var guid = await GetUserGuidAsync(email);
            var passwordGuid = PasswordPrefix + guid;

            var confirmCode = ConfirmCodeHelper.CreateRandomCode();
            SaveCodeToCache(passwordGuid, confirmCode);

            await _mailService.SendPasswordResetAsync(email, confirmCode, guid);
        }

        public async Task<bool> ChangePassword(string guid, string confirmCode, string password)
        {
            if (!Guid.TryParse(guid, out var userGuid)) return false;

            var user = await GetUserAsync(new UserSpecification(userGuid));

            var passwordGuid = PasswordPrefix + guid;

            if (!CheckConfirmCode(confirmCode, passwordGuid)) return false;

            ChangeUserPassword(user, password);

            return true;
        }

        public async Task SetFireBaseId(Guid guid, string fireBaseId)
        {
            var user = await GetUserAsync(new UserSpecification(guid));

            user.FireBaseId = fireBaseId;

            UserRepository.Update(user);
        }

        #region Private Methods

        protected async Task<bool> CheckUserExistenceAsync(ISpecification<User> specification)
        {
            return await UserRepository.GetAsync(specification) == null;
        }

        private string CreateJwtToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Guid.ToString())
            };
            var token = new JwtSecurityToken(
                "AvatarApp",
                "AvatarAppClient",
                claims,
                signingCredentials: new SigningCredentials(
                    _signingEncodingKey.GetKey(),
                    _signingEncodingKey.SigningAlgorithm));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task CreateUserAsync(UserDto userDto)
        {
            var hashed = PasswordHelper.HashPassword(userDto.Password);
            var guid = Guid.NewGuid();

            var user = new User
            {
                Name = userDto.Name,
                Password = hashed,
                Email = userDto.Email,
                Guid = guid
            };

            await UserRepository.InsertAsync(user);
        }

        private static bool CheckUserPassword(User user, string password)
        {
            var hashed = PasswordHelper.HashPassword(password);
            return hashed == user.Password;
        }

        private void SaveCodeToCache(string guid, string confirmCode)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            _cache.Set(guid, confirmCode, cacheEntryOptions);
        }

        private async Task<string> GetUserGuidAsync(string email)
        {
            var user = await GetUserAsync(new UserSpecification(email));

            if (user == null) throw new UserNotFoundException();

            return user.Guid.ToString();
        }

        private bool CheckConfirmCode(string confirmCode, string guid)
        {
            if (!_cache.TryGetValue(guid, out string cacheEntry)) return false;

            if (!string.Equals(cacheEntry, confirmCode, StringComparison.CurrentCultureIgnoreCase)) return false;

            _cache.Remove(guid);

            return true;
        }

        private void SetUserEmailConfirmed(User user)
        {
            user.IsEmailConfirmed = true;
            UserRepository.Update(user);
        }

        private void ChangeUserPassword(User user, string password)
        {
            user.Password = PasswordHelper.HashPassword(password);
            UserRepository.Update(user);
        }

        #endregion
    }
}
