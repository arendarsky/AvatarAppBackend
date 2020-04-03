using System;
using System.Collections.Generic;
using System.Linq;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Specifications.VideoSpecifications
{
    public sealed class VideoWithUserSpecification: BaseSpecification<Video>
    {
        public VideoWithUserSpecification(string fileName) : base(video => video.Name == fileName)
        {
            AddInclude(video => video.User);
        }

        public VideoWithUserSpecification(string fileName, Guid userGuid) : base(video => video.Name == fileName && video.User.Guid == userGuid)
        {
            AddInclude(video => video.User);
        }

        public VideoWithUserSpecification(IEnumerable<long> watchedVideos) : base(video =>
            video.IsApproved.HasValue && video.IsApproved == true && video.IsActive &&
            !watchedVideos.Contains(video.Id))
        {
            AddInclude(video => video.User);
        }

        //TODO Rename this
        public VideoWithUserSpecification() : base(video => !video.IsApproved.HasValue)
        {
            AddInclude(video => video.User);
        }
    }
}
