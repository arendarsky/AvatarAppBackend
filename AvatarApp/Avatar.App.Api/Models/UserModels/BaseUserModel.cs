using Avatar.App.Casting.Models;
using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Api.Models.UserModels
{
    public abstract class BaseUserModel
    {
        protected BaseUserModel(UserDb user)
        {
            Id = user.Id;
            Name = user.Name;
            Description = user.Description;
            ProfilePhoto = user.ProfilePhoto;
        }

        protected BaseUserModel(Contestant user)
        {
            Id = user.Id;
            Name = user.Name;
            Description = user.Description;
            ProfilePhoto = user.ProfilePhoto;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfilePhoto { get; set; }
    }
}
