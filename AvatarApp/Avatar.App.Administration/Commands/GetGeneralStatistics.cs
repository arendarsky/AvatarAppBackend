using Avatar.App.Administration.Models;
using MediatR;

namespace Avatar.App.Administration.Commands
{
    public class GetGeneralStatistics: IRequest<GeneralStatistics>
    {
    }
}
