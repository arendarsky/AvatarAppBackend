using Avatar.App.SharedKernel.Models;
using MediatR;

namespace Avatar.App.Casting.Commands
{
    public class GetBestContestant: IRequest<BaseContestant>
    {
    }
}
