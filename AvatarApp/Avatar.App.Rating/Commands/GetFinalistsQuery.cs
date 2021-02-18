using System.Linq;
using Avatar.App.Rating.Models;
using MediatR;

namespace Avatar.App.Rating.Commands
{
    public class GetFinalistsQuery: IRequest<IQueryable<RatingFinalist>>
    {
    }
}
