using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Core.Models
{
    public class GeneralStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int TotalVideos { get; set; }
        public int TotalActiveVideos { get; set; }
        public int TotalViews { get; set; }
        public int TotalLikes { get; set; }
    }
}
