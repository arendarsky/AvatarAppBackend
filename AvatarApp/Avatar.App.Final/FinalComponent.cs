using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Final.Commands;
using Avatar.App.Final.CreationData;
using Avatar.App.Final.Models;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Commands;
using MediatR;

namespace Avatar.App.Final
{
    public interface IFinalComponent
    {
        Task<bool> VoteToAsync(FinalVoteCreation voteCreation);
        Task<Models.Final> GetFinalAsync();
    }

    internal class FinalComponent: AvatarAppComponent, IFinalComponent
    {
        public FinalComponent(IMediator mediator, IQueryManager queryManager) : base(mediator, queryManager)
        {
        }

        public async Task<bool> VoteToAsync(FinalVoteCreation voteCreation)
        {
            return await Mediator.Send(new VoteInFinal(voteCreation));
        }

        public async Task<Models.Final> GetFinalAsync()
        {
            return await Mediator.Send(new GetActiveFinal());
        }
    }
}
