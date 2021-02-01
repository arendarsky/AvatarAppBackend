using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Casting.Models;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Semifinal
{
    public interface IFinalistsSetter
    {
        Task AddFinalists(IEnumerable<Semifinalist> semifinalists);
    }
}
