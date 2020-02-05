using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Avatar.App.Entities.Models
{
    public class VideoStream
    {
        public string Name { get; set; }
        public Stream Stream { get; set; }
    }
}
