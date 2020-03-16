using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Models;
using Avatar.App.Core.Specifications.UserSpecifications;
using Avatar.App.SharedKernel.Interfaces;

namespace Avatar.App.Core.Services.Impl
{
    public class RatingService : BaseServiceWithRating, IRatingService
    {
        public RatingService(IRepository<User> userRepository, IRepository<LikedVideo> likedVideoRepository) : base(likedVideoRepository, userRepository)
        {

        }


        public async Task<ICollection<UserProfile>> GetCommonRatingAsync(int number)
        {
            var users = GetUsers(new UserWithLoadedVideosSpecification()).ToList();

            var userProfiles = await GetUsersWithLikesNumberAsync(users);

            return TakeUsersOrderedByLikesNumber(userProfiles, number).ToList();
        }

        public async Task<int> GetUserRating(Guid userGuid)
        {
            var user = await GetUserAsync(new UserSpecification(userGuid));

            return GetLikesNumber(user);
        }


        #region Private Methods

        private IEnumerable<User> GetUsers(ISpecification<User> specification)
        {
            return UserRepository.List(specification);
        }

        private async Task<IEnumerable<UserProfile>> GetUsersWithLikesNumberAsync(IEnumerable<User> users)
        {
            var userProfiles = await Task.Run(() =>
            {
                var profiles = new List<UserProfile>();
                profiles.AddRange(from user in users
                    select new UserProfile { LikesNumber = GetLikesNumber(user), User = user });
                return profiles;
            });

            return userProfiles;
        }

        private static IEnumerable<UserProfile> TakeUsersOrderedByLikesNumber(IEnumerable<UserProfile> userProfiles,
            int number)
        {
            return userProfiles.OrderByDescending(r => r.LikesNumber).Take(number).ToList();
        }

        #endregion
    }
}
