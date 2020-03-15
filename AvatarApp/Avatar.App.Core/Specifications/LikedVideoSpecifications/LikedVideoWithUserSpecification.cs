using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Specifications.LikedVideoSpecifications
{
    public sealed class LikedVideoWithUserSpecification: BaseSpecification<LikedVideo>
    {
        public LikedVideoWithUserSpecification(Video video) : base(likedVideo => likedVideo.VideoId == video.Id)
        {
            AddInclude(l => l.User);
        }

        public LikedVideoWithUserSpecification(User user) : base(likedVideo => likedVideo.UserId == user.Id)
        {
            AddInclude(l => l.User);
        }
    }
}
