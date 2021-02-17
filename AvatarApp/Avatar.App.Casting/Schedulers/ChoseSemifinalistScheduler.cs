using System.Threading.Tasks;
using Avatar.App.Casting.Commands;
using Avatar.App.Schedulers;
using MediatR;

namespace Avatar.App.Casting.Schedulers
{
    internal class ChoseSemifinalistScheduler: ICronInvocable
    {
        private readonly IMediator _mediator;

        public ChoseSemifinalistScheduler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Invoke()
        {
            var user = await _mediator.Send(new GetBestContestant());

            if (user != null)
            {
                await _mediator.Send(new AddSemifinalist(user.Id));
            }
        }
    }
}
