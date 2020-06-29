using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Models;
using Avatar.App.Core.Specifications.SemifinalistsSpecifications;
using Avatar.App.Core.Specifications.UserSpecifications;
using Avatar.App.SharedKernel.Interfaces;

namespace Avatar.App.Core.Services.Impl
{
    public class RatingService : BaseServiceWithRating, IRatingService
    {
        private readonly IRepository<Semifinalist> _semifinalistsRepository;

        public RatingService(IRepository<User> userRepository, IRepository<LikedVideo> likedVideoRepository,
            IRepository<Semifinalist> semifinalistsRepository) : base(likedVideoRepository, userRepository)
        {
            _semifinalistsRepository = semifinalistsRepository;
        }


        public ICollection<UserProfile> GetCommonRating(int number)
        {
            var users = GetUsers(new UserCommonRatingSpecification()).ToList();

            var userProfiles = GetUsersWithLikesNumberAsync(users);

            return TakeUsersOrderedByLikesNumber(userProfiles, number).ToList();
        }

        public async Task<int> GetUserRating(Guid userGuid)
        {
            var user = await GetUserAsync(new UserSpecification(userGuid));

            return GetLikesNumber(user);
        }

        public ICollection<UserProfile> GetSemifinalists()
        {
            var users = UserRepository.List(new UserSemifinalistsSpecification())
                .OrderByDescending(u => u.Semifinalist.Date);

            return GetUsersWithLikesNumberAsync(users).ToList();
        }


        #region Private Methods

        private IEnumerable<User> GetUsers(ISpecification<User> specification)
        {
            return UserRepository.List(specification);
        }

        private IEnumerable<UserProfile> GetUsersWithLikesNumberAsync(IEnumerable<User> users)
        {
            var profiles = new List<UserProfile>();

            profiles.AddRange(from user in users
                select new UserProfile { LikesNumber = GetLikesNumber(user), User = user });

            return profiles;
        }

        private static IEnumerable<UserProfile> TakeUsersOrderedByLikesNumber(IEnumerable<UserProfile> userProfiles,
            int number)
        {
            return userProfiles.OrderByDescending(r => r.LikesNumber).Take(number).ToList();
        }

        #endregion
    }
}
