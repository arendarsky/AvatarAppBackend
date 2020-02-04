using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Avatar.App.Service.Helpers
{
    public static class VideoFileHelper
    {
        public static string NameGenerator(string videoStoreDirection)
        {
            string name ="";
            int file_count = Directory.GetFiles(videoStoreDirection).Length;
            for (int i = file_count.ToString().Length; i < 12; i++)
            {
                name += "0";
            }
            name += file_count.ToString()+".mp4";
            
            return name;
        }
    }
}
