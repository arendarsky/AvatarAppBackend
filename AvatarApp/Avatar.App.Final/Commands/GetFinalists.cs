using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Core.Models;
using MediatR;

namespace Avatar.App.Final.Commands
{
    public class GetFinalists: IRequest<IEnumerable<UserProfile>>
    {
    }
}
