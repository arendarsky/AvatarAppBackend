using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avatar.App.Rating.Models;
using MediatR;

namespace Avatar.App.Rating.Commands
{
    public class GetFinalistsQuery: IRequest<IQueryable<RatingFinalist>>
    {
    }
}
