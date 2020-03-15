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

        public AuthenticationService(IEmailService mailService, IJwtSigningEncodingKey signingEncodingKey, IMemoryCache memoryCache, IRepository<User> userRepository) : base(userRepository)
        {
            _mailService = mailService;
            _signingEncodingKey = signingEncodingKey;
            _cache = memoryCache;
        }

        public async Task RegisterAsync(UserDto userDto)
        {
            var user = await GetUserAsync(new UserSpecification(userDto.Email));

            if (user != null) throw new UserAlreadyExistsException();

            await CreateUserAsync(userDto);
        }

        public async Task<string> GetAuthorizationTokenAsync(string email, string password)
        {
            var user = await GetUserAsync(new UserSpecification(email)); ;
            if (user == null) throw new UserNotFoundException();

            if (!CheckUserPassword(user, password)) throw new InvalidPasswordException();

            return CreateJwtToken(user);
        }

        public async Task SendEmailAsync(string email)
        {
            var confirmCode = ConfirmCodeHelper.CreateRandomCode();

            SaveCodeToCache(email, confirmCode);
            
            await _mailService.SendConfirmCodeAsync(email, confirmCode);
        }

        public async Task<bool> ConfirmEmailAsync(string email, string confirmCode)
        {
            if (!await CheckConfirmCode(confirmCode, email)) return false;

            await SetUserEmailConfirmed(email);

            return true;
        }

        #region Private Methods

        protected override async Task<User> GetUserAsync(ISpecification<User> specification)
        {
            var user = await UserRepository.GetAsync(specification);

            return user;
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

        private void SaveCodeToCache(string email, string confirmCode)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            _cache.Set(email, confirmCode, cacheEntryOptions);
        }

        private async Task<bool> CheckConfirmCode(string confirmCode, string email)
        {
            var result = await Task.Run(() =>
            {
                if (!_cache.TryGetValue(email, out string cacheEntry)) return false;

                if (!string.Equals(cacheEntry, confirmCode, StringComparison.CurrentCultureIgnoreCase)) return false;

                _cache.Remove(email);
                return true;
            });
            return result;
        }

        private async Task SetUserEmailConfirmed(string email)
        {
            var user = await GetUserAsync(new UserSpecification(email));
            if (user == null) throw new UserNotFoundException();

            user.IsEmailConfirmed = true;
            UserRepository.Update(user);
        }

        #endregion
    }
}
