using System;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Semifinal.DTO;
using Avatar.App.Core.Semifinal.Exceptions;
using Avatar.App.Core.Semifinal.Interfaces;
using Avatar.App.Core.Services.Impl;
using Avatar.App.Core.Specifications.UserSpecifications;
using Avatar.App.SharedKernel.Interfaces;

namespace Avatar.App.Core.Semifinal.Services
{
    public class BattleVoteService: BaseServiceWithAuthorization, IBattleVoteService
    {
        private readonly IBattleRepository _battleRepository;
        private readonly IRepository<BattleVote> _battleVoteRepository;

        private User User { get; set; }
        private BattleVoteDTO BattleVoteCreationDTO { get; set; }
        private BattleVote BattleVote { get; set; }

        public BattleVoteService(IBattleRepository battleRepository, IRepository<BattleVote> battleVoteRepository,
            IRepository<User> userRepository) : base(userRepository)
        {
            _battleRepository = battleRepository;
            _battleVoteRepository = battleVoteRepository;
        }

        public async Task VoteToAsync(Guid userGuid, BattleVoteDTO battleVoteDTO)
        {
            await SetupUserAndBattleVoteCreationDTO(userGuid, battleVoteDTO);

            if (CheckRepetitiveVote() || !CheckUserVotesNumber())
            {
                return;
            }

            BattleVote = CreateBattleVote();
            await _battleVoteRepository.InsertAsync(BattleVote);
        }

        private async Task SetupUserAndBattleVoteCreationDTO(Guid userGuid, BattleVoteDTO battleVoteDTO)
        {
            BattleVoteCreationDTO = battleVoteDTO;
            User = await GetUserAsync(new UserSpecification(userGuid));
        }

        private BattleVote GetVoteByBattleVoteDTOAndUser()
        {
            return _battleVoteRepository.Get(vote =>
                vote.BattleId == BattleVoteCreationDTO.BattleId && vote.SemifinalistId == BattleVoteCreationDTO.SemifinalistId &&
                vote.UserId == User.Id);
        }

        private bool CheckRepetitiveVote()
        {
            var exactSameVote = GetVoteByBattleVoteDTOAndUser();
            return exactSameVote != null;
        }

        private bool CheckUserVotesNumber()
        {
            var battle = GetBattle();
            var userVotesNumber = _battleVoteRepository.Count(vote =>
                vote.BattleId == BattleVoteCreationDTO.BattleId && vote.UserId == User.Id);

            return battle.WinnersNumber > userVotesNumber;
        }

        private Battle GetBattle()
        {
            var battle = _battleRepository.GetById(BattleVoteCreationDTO.BattleId);

            if (battle == null)
            {
                throw new BattleVoteServiceException();
            }

            return battle;
        }

        private BattleVote CreateBattleVote()
        {
            return new BattleVote()
            {
                SemifinalistId = BattleVoteCreationDTO.SemifinalistId,
                BattleId = BattleVoteCreationDTO.BattleId,
                UserId = User.Id,
                Date = DateTime.Now
            };
        }

        public async Task CancelVoteAsync(Guid userGuid, BattleVoteDTO battleVoteDTO)
        {
            await SetupUserAndBattleVoteCreationDTO(userGuid, battleVoteDTO);
            var existedVote = GetVoteByBattleVoteDTOAndUser();

            if (existedVote == null)
            {
                return;
            }

            RemoveVote(existedVote);
        }

        private void RemoveVote(BattleVote vote)
        {
            _battleVoteRepository.Delete(vote);
        }
    }
}
