using Avatar.App.Casting.Models;
using MediatR;

namespace Avatar.App.Casting.Commands
{
    public class GetBestContestant: IRequest<BaseContestant>
    {
    }
}
