using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Semifinal.DTO;
using Avatar.App.Core.Semifinal.Exceptions;
using Avatar.App.Core.Semifinal.Interfaces;
using Avatar.App.SharedKernel.Interfaces;

namespace Avatar.App.Core.Semifinal.Services
{
    public class BattleService: IBattleService
    {
        private readonly IRepository<Semifinalist> _semifinalistRepository;
        private readonly IBattleRepository _battleRepository;

        private BattleCreationDTO BattleCreationDTO { get; set; }
        private Battle Battle { get; set; }

        public BattleService(IRepository<Semifinalist> semifinalistRepository, IBattleRepository battleRepository)
        {
            _semifinalistRepository = semifinalistRepository;
            _battleRepository = battleRepository;
        }

        public async Task<Battle> CreateFromBattleCreationDTOAsync(BattleCreationDTO battleCreationDTO)
        {
            BattleCreationDTO = battleCreationDTO;
            Battle = CreateBattle();
            await AddBattleParticipants();
            return Battle;
        }

        public Battle CreateBattle()
        {
            return new Battle
            {
                CreationDate = DateTime.Now,
                WinnersNumber = BattleCreationDTO.WinnersNumber,
                EndDate = BattleCreationDTO.EndDate,
                BattleSemifinalists = new List<BattleSemifinalist>()
            };
        }

        private async Task AddBattleParticipants()
        {
            foreach (var semifinalistId in BattleCreationDTO.SemifinalistsId)
            {
                var semifinalist = await _semifinalistRepository.GetByIdAsync(semifinalistId);
                if (semifinalist == null)
                {
                    throw new BattleServiceException();
                }
                AddBattleParticipant(semifinalist);
            }
        }

        private void AddBattleParticipant(Semifinalist semifinalist)
        {
            var battleSemifinalist = new BattleSemifinalist
            {
                Battle = Battle,
                Semifinalist = semifinalist
            };

            Battle.BattleSemifinalists.Add(battleSemifinalist);
        }

        public async Task InsertBattleAsync(Battle battle)
        {
            await _battleRepository.InsertAsync(battle);
        }

        public IEnumerable<Battle> GetBattles()
        {
            return _battleRepository.GetWithRelations(battle => true).OrderByDescending(battle => battle.EndDate);
        }

        public BattleParticipantVotesDTO GetVotesInfo(Guid userGuid, BattleVoteDTO battleVoteDTO)
        {
            var battle = _battleRepository.GetWithRelations(b => b.Id == battleVoteDTO.BattleId)
                .FirstOrDefault();

            if (battle == null)
            {
                return null;
            }

            return new BattleParticipantVotesDTO
            {
                VotesNumber = battle.Votes.Count(vote => vote.SemifinalistId == battleVoteDTO.SemifinalistId),
                IsLikedByUser = battle.Votes.Any(vote =>
                    vote.SemifinalistId == battleVoteDTO.SemifinalistId && vote.User.Guid == userGuid)
            };
        }
    }
}
