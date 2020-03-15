using System.Linq;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel.Interfaces;

namespace Avatar.App.Core.Services.Impl
{
    public abstract  class BaseServiceWithRating: BaseServiceWithAuthorization
    {
        protected readonly IRepository<LikedVideo> LikedVideoRepository;

        protected BaseServiceWithRating(IRepository<LikedVideo> likedVideoRepository, IRepository<User> userRepository) : base(userRepository)
        {
            LikedVideoRepository = likedVideoRepository;
        }

        protected int GetLikesNumber(User user)
        {
            return user.LoadedVideos.Where(v => v.IsApproved.HasValue && v.IsApproved == true)
                .Sum(v => LikedVideoRepository.Count(c => c.VideoId == v.Id));
        }
    }
}
