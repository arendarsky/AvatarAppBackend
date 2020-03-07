using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avatar.App.Api.Models
{
    public class RatingItemModel
    {
        public int LikesNumber { get; set; }
        public UserModel User { get; set; }
    }
}
