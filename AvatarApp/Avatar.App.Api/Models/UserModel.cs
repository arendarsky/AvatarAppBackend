using System.Collections.Generic;

namespace Avatar.App.Api.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfilePhoto { get; set; }
        public ICollection<VideoModel> Videos { get; set; }
    }
}
