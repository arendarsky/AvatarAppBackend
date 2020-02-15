using System;
using System.Threading.Tasks;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using Avatar.App.Service.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Avatar.App.Service.Services.Impl
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly AvatarAppContext _context;
        private readonly IEmailService _mailService;
        private readonly IDistributedCache _distributedCache;

        public AuthenticationService(AvatarAppContext context, IEmailService mailService, IDistributedCache distributedCache)
        {
            _context = context;
            _mailService = mailService;
            _distributedCache = distributedCache;
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
    }
}
