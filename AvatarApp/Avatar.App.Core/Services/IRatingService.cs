using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Models;

namespace Avatar.App.Core.Services
{
    public interface IRatingService
    {
        ICollection<UserProfile> GetCommonRating(int number);
        Task<int> GetUserRating(Guid userGuid);
        ICollection<UserProfile> GetSemifinalists();
    }
}
