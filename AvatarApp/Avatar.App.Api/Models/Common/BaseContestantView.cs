using Avatar.App.Casting.Models;

namespace Avatar.App.Api.Models.Common
{
    public abstract class BaseContestantView
    {
        protected BaseContestantView(BaseContestant user)
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
