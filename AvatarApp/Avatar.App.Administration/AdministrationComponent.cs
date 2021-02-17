using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Administration.Commands;
using Avatar.App.Administration.Models;
using Avatar.App.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        public AdministrationComponent(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IList<ModerationContestantPerformance>> GetUncheckedVideosAsync(int number)
        {
            var uncheckedVideosQuery = await Mediator.Send(new GetUncheckedVideos());
            return await uncheckedVideosQuery.Take(number).ToListAsync();
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
