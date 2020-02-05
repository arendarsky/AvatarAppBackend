using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using Avatar.App.Service.Constants;
using Avatar.App.Service.Helpers;
using MailKit;
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

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null) return true;
            user = new User()
            {
                Email = email
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }
    }
}
