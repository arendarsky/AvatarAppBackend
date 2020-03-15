using System;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Specifications.UserSpecifications
{
    public sealed class UserWithWatchedVideosSpecification: BaseSpecification<User>
    {
        public UserWithWatchedVideosSpecification(Guid userGuid) : base(user => user.Guid == userGuid)
        {
            AddInclude(user => user.WatchedVideos);
        }
    }
}
