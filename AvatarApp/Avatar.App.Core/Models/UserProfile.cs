using Avatar.App.Core.Entities;

namespace Avatar.App.Core.Models
{
    public class UserProfile
    {
        public int LikesNumber { get; set; }
        public User User { get; set; }
    }
}
