using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models
{
    public class SemifinalistModel
    {
        public long Id { get; set; }
        public string VideoName { get; set; }
        public int VotesNumber { get; set; }
        public bool IsLikedByUser { get; set; }

        public static SemifinalistModel FromSemifinalistWithUserLikeInfo(Semifinalist semifinalist, Guid userGuid)
        {
            return new SemifinalistModel(semifinalist, userGuid);
        }

        private SemifinalistModel(Semifinalist semifinalist, Guid userGuid): this(semifinalist)
        {
            IsLikedByUser = semifinalist.Votes?.Any(vote => vote.User.Guid == userGuid) ?? false;
        }

        private SemifinalistModel(Semifinalist semifinalist)
        {
            Id = semifinalist.Id;
            VideoName = semifinalist.VideoName;
            VotesNumber = semifinalist.Votes?.Count() ?? 0;
        }
    }
}
