using System;
using System.Collections.Generic;
using Avatar.App.Semifinal.Models;
using MediatR;

namespace Avatar.App.Final.Commands
{
    public class AddFinalists: IRequest
    {
        public IEnumerable<Semifinalist> Semifinalists { get; }

        public AddFinalists(IEnumerable<Semifinalist> semifinalists)
        {
            Semifinalists = semifinalists;
        }
    }
}
