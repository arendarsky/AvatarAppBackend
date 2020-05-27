using System.Linq;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Specifications.UserSpecifications
{
    public sealed class UserCommonRatingSpecification: BaseSpecification<User>
    {
        public UserCommonRatingSpecification() : base(u =>
            u.Semifinalist == null && u.LoadedVideos.Any(video => video.IsApproved.HasValue && video.IsApproved.Value))
        {
            AddInclude(user => user.LoadedVideos);
        }
    }
}
