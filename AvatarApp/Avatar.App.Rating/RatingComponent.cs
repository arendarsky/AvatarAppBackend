using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Rating.Commands;
using Avatar.App.Rating.Models;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Rating
{
    public interface IRatingComponent
    {
        Task<IEnumerable<RatingContestantPerformance>> GetCommonRating(int number);
        Task<int> GetUserRating(Guid userGuid);
        Task<ICollection<RatingSemifinalist>> GetSemifinalists();
        Task<ICollection<RatingFinalist>> GetFinalists();
    }

    internal class RatingComponent: AvatarAppComponent, IRatingComponent
    {
        public RatingComponent(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<RatingContestantPerformance>> GetCommonRating(int number)
        {
            var ratingQuery = await Mediator.Send(new GetQuery<RatingContestantPerformance>());
            var rating = await ratingQuery.Where(r => !r.IsSemifinalist).ToListAsync();
            return rating.OrderByDescending(r => r.LikesNumber).Take(number);
        }

        public async Task<int> GetUserRating(Guid userGuid)
        {
            return await Mediator.Send(new GetLikesNumber(userGuid));
        }

        public async Task<ICollection<RatingSemifinalist>> GetSemifinalists()
        {
            return await (await Mediator.Send(new GetQuery<RatingSemifinalist>())).ToListAsync();
        }

        public async Task<ICollection<RatingFinalist>> GetFinalists()
        {
            return await (await Mediator.Send(new GetQuery<RatingFinalist>())).ToListAsync();
        }
    }
}
