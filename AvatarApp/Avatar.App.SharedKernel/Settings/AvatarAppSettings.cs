namespace Avatar.App.SharedKernel.Settings
{
    public class AvatarAppSettings
    {
        public double ShortVideoMaxLength { get; set; }
        public string VideoStoragePrefix { get; set; }
        public string ImageStoragePrefix { get; set; }
        public int MaxVideoNumber { get; set; }
        public int MaxVideoSize { get; set; }
        public int MaxImageSize { get; set; }
        public string AcceptedVideoExtension { get; set; }
        public string AdminGuid { get; set; }

        public string WebUrl { get; set; }
    }
}
