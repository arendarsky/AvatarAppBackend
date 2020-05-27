using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Specifications.SemifinalistsSpecifications
{
    public sealed class SemifinalistWithUserSpecification: BaseSpecification<Semifinalist>
    {
        public SemifinalistWithUserSpecification() : base(semi => true)
        {
            AddInclude(semi => semi.User);
        }
    }
}
