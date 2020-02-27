using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Threading.Tasks;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using Avatar.App.Service.Helpers;
using Avatar.App.Service.Models;
using Avatar.App.Service.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace Avatar.App.Service.Services.Impl
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly AvatarAppContext _context;
        private readonly IEmailService _mailService;
        private readonly IDistributedCache _distributedCache;
        private readonly IJwtSigningEncodingKey _signingEncodingKey;

        public AuthenticationService(AvatarAppContext context, IEmailService mailService, IDistributedCache distributedCache, IJwtSigningEncodingKey signingEncodingKey)
        {
            _context = context;
            _mailService = mailService;
            _distributedCache = distributedCache;
            _signingEncodingKey = signingEncodingKey;
        }

        public async Task<bool> RegisterAsync(UserDto userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (user != null) return false;
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hashed = HashPassword(userDto.Password);
            var guid = Guid.NewGuid();

            user = new User
            {
                Name = userDto.Name,
                Password = userDto.Password,
                Email = userDto.Email,
                Guid = guid
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetAuthorizationTokenAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;
            var hashed = HashPassword(password);
            return password != user.Password ? null : CreateJwtToken(user);
        }

        public async Task SendEmailAsync(string email)
        {
            var confirmCode = ConfirmCodeHelper.CreateRandomCode();
            var distributedCacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = new TimeSpan(0, 5, 0)
            };
            await _distributedCache.SetStringAsync(email, confirmCode, distributedCacheOptions);
            await _mailService.SendConfirmCodeAsync(email, confirmCode);
        }

        public async Task<bool> ConfirmEmailAsync(string email, string confirmCode)
        {
            var correctCode = await _distributedCache.GetStringAsync(email);
            if (correctCode != confirmCode.ToUpper()) return false;
            await _distributedCache.RemoveAsync(email);
            return true;
        }

        public async Task<Guid> GetUserGuidAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null) return user.Guid;
            var guid = Guid.NewGuid();
            user = new User()
            {
                Email = email,
                Guid = guid
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
            return guid;
        }

        #region Private Methods

        private static string HashPassword(string password)
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));
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

        #endregion
    }
}
