namespace Avatar.App.Casting.CreationData
{
    public class VideoCreation
    {
        public string Name { get; }
        public long UserId { get; }
        public double EndTime { get; }
        public double StartTime { get; }
        public bool IsActive { get;}

        public VideoCreation(string fileName, long userId, double endTime, bool isActive)
        {
            UserId = userId;
            EndTime = endTime;
            Name = fileName;
            UserId = userId;
            StartTime = 0;
            EndTime = endTime;
            IsActive = isActive;
        }
    }
}
