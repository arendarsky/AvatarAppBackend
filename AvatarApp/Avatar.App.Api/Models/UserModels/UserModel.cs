using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models.UserModels
{
    public class UserModel: BaseUserModel
    {
        public UserModel(User user) : base(user)
        {
        }
    }
}
