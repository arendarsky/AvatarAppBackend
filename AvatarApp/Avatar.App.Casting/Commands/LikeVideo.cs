using System;
using MediatR;

namespace Avatar.App.Casting.Commands
{
    public class LikeVideo: IRequest
    {
        public LikeVideo(Guid userGuid, string videoName, bool isLiked)
        {
            UserGuid = userGuid;
            VideoName = videoName;
            IsLiked = isLiked;
        }

        public Guid UserGuid { get; }
        public string VideoName { get; }
        public bool IsLiked { get; }
    }
}
