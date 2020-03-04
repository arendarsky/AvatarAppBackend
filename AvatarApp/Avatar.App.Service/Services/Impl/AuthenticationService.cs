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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Avatar.App.Service.Services.Impl
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly AvatarAppContext _context;
        private readonly IEmailService _mailService;
        private readonly IJwtSigningEncodingKey _signingEncodingKey;
        private readonly IMemoryCache _cache;

        public AuthenticationService(AvatarAppContext context, IEmailService mailService, IJwtSigningEncodingKey signingEncodingKey, IMemoryCache memoryCache)
        {
            _context = context;
            _mailService = mailService;
            _signingEncodingKey = signingEncodingKey;
            _cache = memoryCache;
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
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            _cache.Set(email, confirmCode, cacheEntryOptions);
            await _mailService.SendConfirmCodeAsync(email, confirmCode);
        }

        public async Task<bool> ConfirmEmailAsync(string email, string confirmCode)
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
