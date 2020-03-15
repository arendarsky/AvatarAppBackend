using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.Core.Models;

namespace Avatar.App.Core.Services
{
    public interface IRatingService
    {
        Task<ICollection<UserProfile>> GetCommonRatingAsync(int number);
        Task<int> GetUserRating(Guid userGuid);
    }
}
