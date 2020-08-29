using Avatar.App.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Avatar.App.Core.Services
{
    public interface ISemifinalService
    {
        Task<bool> CreateBattleAsync(BattleCreatingDto battleCreatingDto);
    }
}
