using Avatar.App.Semifinal.Models;
using MediatR;

namespace Avatar.App.Semifinal.Commands
{
    public class UpdateVotes: IRequest
    {
        public UpdateVotes(VotingRoom votingRoom)
        {
            VotingRoom = votingRoom;
        }

        public VotingRoom VotingRoom { get; }
    }
}
