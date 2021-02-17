using MediatR;

namespace Avatar.App.Administration.Commands
{
    public class ApproveVideo: IRequest
    {
        public ApproveVideo(string videoName, bool isApproved)
        {
            VideoName = videoName;
            IsApproved = isApproved;
        }

        public string VideoName { get; }
        public bool IsApproved { get; }
    }
}
