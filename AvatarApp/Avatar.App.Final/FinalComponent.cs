using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Core.Models;
using Avatar.App.Final.Commands;
using Avatar.App.Semifinal;
using Avatar.App.Semifinal.Models;
using MediatR;

namespace Avatar.App.Final
{
    public interface IFinalComponent: IFinalistsSetter
    {
        Task<IEnumerable<UserProfile>> GetFinalists();
    }

    internal class FinalComponent: IFinalComponent
    {
        private readonly IMediator _mediator;

        public FinalComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task AddFinalists(IEnumerable<Semifinalist> semifinalists)
        {
            return _mediator.Send(new AddFinalists(semifinalists));
        }

        public async Task<IEnumerable<UserProfile>> GetFinalists()
        {
            return await _mediator.Send(new GetFinalists());
        }
    }
}
