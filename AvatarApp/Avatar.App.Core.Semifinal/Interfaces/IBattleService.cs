using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Semifinal.DTO;

namespace Avatar.App.Core.Semifinal.Interfaces
{
    public interface IBattleService
    {
        Task<Battle> CreateFromBattleCreationDTOAsync(BattleCreationDTO battleCreationDTO);
        Task InsertBattleAsync(Battle battle);
        IEnumerable<Battle> GetBattles();
    }
}
