﻿using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Entities.Models;

namespace Avatar.App.Service.Models
{
    public class RatingItem
    {
        public int LikesNumber { get; set; }
        public User User { get; set; }
    }
}
