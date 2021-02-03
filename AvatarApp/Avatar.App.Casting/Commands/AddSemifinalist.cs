﻿using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Avatar.App.Casting.Commands
{
    public class AddSemifinalist: IRequest
    {
        public AddSemifinalist(long userId)
        {
            UserId = userId;
        }

        public long UserId { get; }
    }
}
