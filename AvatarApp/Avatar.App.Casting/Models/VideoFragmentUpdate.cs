namespace Avatar.App.Casting.Models
{
    public class VideoFragmentUpdate
    {
        private readonly double _fragmentMaxLength; 

        public VideoFragmentUpdate(string fileName, double startTime, double endTime, double fragmentMaxLength)
        {
            FileName = fileName;
            StartTime = startTime;
            EndTime = endTime;
            _fragmentMaxLength = fragmentMaxLength;
        }

        public string FileName { get; }
        public double StartTime { get; }
        public double EndTime { get; }

        public bool IsIntervalCorrect => StartTime >= 0 && EndTime > 0 && EndTime > StartTime &&
                                          EndTime - StartTime <= _fragmentMaxLength;
    }
}
