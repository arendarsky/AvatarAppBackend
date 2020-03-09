using Avatar.App.Entities.Models;

namespace Avatar.App.Service.Models
{
    public class UserProfile
    {
        public int LikesNumber { get; set; }
        public User User { get; set; }
    }
}
