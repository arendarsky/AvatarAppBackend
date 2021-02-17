using System.Linq;
using Avatar.App.Administration.Models;
using MediatR;

namespace Avatar.App.Administration.Commands
{
    public class GetUncheckedVideos: IRequest<IQueryable<ModerationContestantPerformance>>
    {
    }
}
