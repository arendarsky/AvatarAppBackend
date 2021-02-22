using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Final.CreationData;
using MediatR;

namespace Avatar.App.Final.Commands
{
    public class VoteInFinal: IRequest<bool>
    {
        public FinalVoteCreation VoteCreation { get; }

        public VoteInFinal(FinalVoteCreation voteCreation)
        {
            VoteCreation = voteCreation;
        }
    }
}
