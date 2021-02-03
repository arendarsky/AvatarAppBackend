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
