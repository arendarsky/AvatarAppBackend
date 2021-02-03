using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Avatar.App.SharedKernel
{
    public abstract class AvatarAppComponent
    {
        protected IMediator Mediator;

        protected AvatarAppComponent(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}
