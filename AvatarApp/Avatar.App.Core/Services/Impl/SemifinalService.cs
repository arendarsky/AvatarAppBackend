using Avatar.App.Core.Entities;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Models;
using Avatar.App.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avatar.App.Core.Services.Impl
{
    public class SemifinalService: ISemifinalService
    {
        private readonly IRepository<Battle> _battleRepository;
        private readonly IRepository<BattleSemifinalist> _battleSemifinalistRepository;
        private readonly IRepository<Semifinalist> _semifinalistRepository;
        private readonly IRepository<BattleVote> _battleVoteRepository;

        public SemifinalService(IRepository<Battle> battleRepository, IRepository<BattleSemifinalist> battleSemifinalistRepository, IRepository<Semifinalist> semifinalistRepository, IRepository<BattleVote> battleVoteRepository)
        {
            _battleRepository = battleRepository;
            _battleSemifinalistRepository = battleSemifinalistRepository;
            _semifinalistRepository = semifinalistRepository;
            _battleVoteRepository = battleVoteRepository;
        }

        public async Task<bool> CreateBattleAsync(BattleCreatingDto battleCreatingDto)
        {
            //вынести число участников в appsettings.json
            if (battleCreatingDto.SemifinalistsId.Count() != 5)
                return false;

            if (battleCreatingDto.EndDate < DateTime.Now)
                return false;

            var battle = new Battle()
            {
                CreationDate = DateTime.Now,
                EndDate = battleCreatingDto.EndDate
            };

            await _battleRepository.InsertAsync(battle);

            foreach (var semifinalistId in battleCreatingDto.SemifinalistsId)
            {
                var semifinalist = await _semifinalistRepository.GetByIdAsync(semifinalistId);

                if (semifinalist == null)
                    throw new SemifinalistNotFoundException();

                var battleSemifinalist = new BattleSemifinalist()
                {
                    Battle = battle,
                    Semifinalist = semifinalist
                };

                await _battleSemifinalistRepository.InsertAsync(battleSemifinalist);
            }

            return true;
                
        }

        public IEnumerable<Battle> GetActiveBattles()
        {
            return _battleRepository.List().Where(b => b.EndDate > DateTime.Now);
        }

        public async Task<bool> Vote(long battleId, long semifinalistId)
        {
            var battle = await _battleRepository.GetByIdAsync(battleId);

            var semifinalist = await _semifinalistRepository.GetByIdAsync(semifinalistId);

            if (battle == null || semifinalist == null)
                return false;

            var battleVote = new BattleVote()
            {
                Battle = battle,
                Semifinalist = semifinalist,
                BattleId = battleId,
                SemifinalistId = semifinalistId
            };

            var dbBattleVote = await _battleVoteRepository.GetAsync(b =>
            b.BattleId == battleVote.BattleId && b.SemifinalistId == battleVote.SemifinalistId);

            if (dbBattleVote == null)
                await _battleVoteRepository.InsertAsync(battleVote);
            else
                _battleVoteRepository.Delete(dbBattleVote);

            return true;
        }
    }
}
