using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Models;

namespace Avatar.App.Api.Models.UserModels
{
    public class SemifinalistUserModel: BaseUserModel
    {
        public SemifinalistUserModel(UserProfile userProfile) : base(userProfile.User)
        {
            LikesNumber = userProfile.LikesNumber;
        }

        public int LikesNumber { get; set; }
    }
}
