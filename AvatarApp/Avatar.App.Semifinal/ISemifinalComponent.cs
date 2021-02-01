using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Semifinal
{
    public interface ISemifinalComponent
    {
        Task CloseBattlesAsync();
        Task<IEnumerable<Battle>> GetBattles();
    }
}
