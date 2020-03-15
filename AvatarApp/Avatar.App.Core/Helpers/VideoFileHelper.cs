using System;
using System.IO;


namespace Avatar.App.Core.Helpers
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

        public static string GetRandomPath(string videoStoreDirection)
        {
            string name = "\\";
            Random rnd = new Random();
            int numOfVideo = rnd.Next(1, Directory.GetFiles(videoStoreDirection).Length);
            for (int i = numOfVideo.ToString().Length; i < 12; i++)
            {
                name += "0";
            }
            name += numOfVideo.ToString() + ".mp4";
            return name;
        }
    }
}
