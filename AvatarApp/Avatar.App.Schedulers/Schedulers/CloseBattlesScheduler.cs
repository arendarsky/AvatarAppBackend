using System.Threading.Tasks;
using Avatar.App.Semifinal;

namespace Avatar.App.Schedulers.Schedulers
{
    internal class CloseBattlesScheduler: ICronInvocable
    {
        private readonly ISemifinalComponent _semifinalComponent;

        public CloseBattlesScheduler(ISemifinalComponent semifinalComponent)
        {
            _semifinalComponent = semifinalComponent;
        }

        public async Task Invoke()
        {
            await _semifinalComponent.CloseBattlesAsync();
        }
    }
}
