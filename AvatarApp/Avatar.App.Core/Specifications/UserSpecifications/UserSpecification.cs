using System;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Specifications.UserSpecifications
{
    public class UserSpecification: BaseSpecification<User>
    {
        public UserSpecification(string email) : base(user => user.Email == email)
        {
        }

        public UserSpecification(Guid guid) : base(user => user.Guid == guid)
        {
        }
    }
}
