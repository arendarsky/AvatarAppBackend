using System;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Specifications.VideoSpecifications
{
    public class VideoSpecification: BaseSpecification<Video>
    {
        public VideoSpecification(string fileName) : base(video => video.Name == fileName)
        {
        }

        public VideoSpecification(Guid userGuid) : base(video => video.User.Guid == userGuid && video.IsApproved.HasValue && video.IsApproved == true)
        {
        }
    }
}
