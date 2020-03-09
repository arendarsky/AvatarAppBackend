using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Entities.Models;
using Avatar.App.Service.Models;

namespace Avatar.App.Service.Services
{
    public interface IRatingService
    {
        Task<ICollection<UserProfile>> GetRatingAsync(int number);
        Task<ICollection<LikedVideo>> GetLikesAsync(Guid userGuid, int number, int skip);
    }
}
