using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Casting.Commands;
using Avatar.App.Core.Services;
using Avatar.App.Schedulers;
using MediatR;

namespace Avatar.App.Casting.Schedulers
{
    internal class ChoseSemifinalistScheduler: ICronInvocable
    {
        private readonly IRatingService _ratingService;
        private readonly IMediator _mediator;

        public ChoseSemifinalistScheduler(IRatingService ratingService, IMediator mediator)
        {
            _ratingService = ratingService;
            _mediator = mediator;
        }

        public async Task Invoke()
        {
            var userProfile = _ratingService.GetCommonRating(1).FirstOrDefault();

            if (userProfile != null)
            {
                await _mediator.Send(new AddSemifinalist(userProfile.User.Id));
            }
        }
    }
}
