using System;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Specifications.UserSpecifications
{
    public sealed class UserWithLoadedVideosSpecification: BaseSpecification<User>
    {
        public UserWithLoadedVideosSpecification(Guid userGuid) : base(user => user.Guid == userGuid)
        {
            AddInclude(user => user.LoadedVideos);
        }

        public UserWithLoadedVideosSpecification() : base(user => true)
        {
            AddInclude(user => user.LoadedVideos);
        }
    }
}
