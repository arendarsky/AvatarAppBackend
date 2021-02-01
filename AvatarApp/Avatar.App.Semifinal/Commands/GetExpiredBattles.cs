using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Semifinal.Models;
using MediatR;

namespace Avatar.App.Semifinal.Commands
{
    public class GetExpiredBattles: IRequest<IEnumerable<Battle>>
    {
    }
}
