using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Administration.Commands;
using Avatar.App.Administration.Models;
using Avatar.App.SharedKernel;
using MediatR;

namespace Avatar.App.Administration
{
    public interface IAdministrationComponent
    {
        Task<IList<ModerationContestantPerformance>> GetUncheckedVideosAsync(int number);
        Task SetApproveStatusAsync(string fileName, bool isApproved);
        Task<GeneralStatistics> GetGeneralStatistics();
    }

    internal class AdministrationComponent: AvatarAppComponent, IAdministrationComponent
    {
        public AdministrationComponent(IMediator mediator, IQueryManager queryManager) : base(mediator, queryManager)
        {
        }

        public async Task<IList<ModerationContestantPerformance>> GetUncheckedVideosAsync(int number)
        {
            var uncheckedVideosQuery = await Mediator.Send(new GetUncheckedVideos());
            return await QueryManager.ToListAsync(uncheckedVideosQuery.Take(number));
        }

        public async Task SetApproveStatusAsync(string fileName, bool isApproved)
        {
            await Mediator.Send(new ApproveVideo(fileName, isApproved));
        }

        public async Task<GeneralStatistics> GetGeneralStatistics()
        {
            return await Mediator.Send(new GetGeneralStatistics());
        }
    }
}
