using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Specifications.UserSpecifications
{
    public sealed class UserSemifinalistsSpecification: BaseSpecification<User>
    {
        public UserSemifinalistsSpecification() : base(user => user.Semifinalist != null)
        {
            AddInclude(user => user.Semifinalist);
            AddInclude(user => user.LoadedVideos);
        }
    }
}
