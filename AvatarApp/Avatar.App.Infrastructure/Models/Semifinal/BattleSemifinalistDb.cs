using Avatar.App.SharedKernel;

namespace Avatar.App.Infrastructure.Models.Semifinal
{
    internal class BattleSemifinalistDb: BaseEntity
    {
        public long BattleId { get; set; }
        public long SemifinalistId { get; set; }

        public BattleDb Battle { get; set; }
        public SemifinalistDb Semifinalist { get; set; }
    }
}
