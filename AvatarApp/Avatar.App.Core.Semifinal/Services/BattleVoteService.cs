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
        private readonly IBattleService _battleService;

        public BattleVoteService(IBattleRepository battleRepository, IRepository<BattleVote> battleVoteRepository,
            IRepository<User> userRepository, IBattleService battleService) : base(userRepository)
        {
            _battleRepository = battleRepository;
            _battleVoteRepository = battleVoteRepository;
            _battleService = battleService;
        }

        public async Task<BattleParticipantVotesDTO> VoteToAsync(Guid userGuid, BattleVoteDTO battleVoteDTO)
        {
            var user = await GetUserAsync(new UserSpecification(userGuid));

            var existedVote = GetVoteByBattleVoteDTOAndUser(user, battleVoteDTO);

            if (existedVote != null)
            {
                RemoveVote(existedVote);
                return _battleService.GetVotesInfo(userGuid, battleVoteDTO);
            }

            if (!CheckUserVotesNumber(user, battleVoteDTO))
            {
                return _battleService.GetVotesInfo(userGuid, battleVoteDTO);
            }

            var battleVote = CreateBattleVote(user, battleVoteDTO);
            _battleVoteRepository.Insert(battleVote);
            return _battleService.GetVotesInfo(userGuid, battleVoteDTO);
        }

        private BattleVote GetVoteByBattleVoteDTOAndUser(User user, BattleVoteDTO battleVoteCreationDTO)
        {
            return _battleVoteRepository.Get(vote =>
                vote.BattleId == battleVoteCreationDTO.BattleId && vote.SemifinalistId == battleVoteCreationDTO.SemifinalistId &&
                vote.UserId == user.Id);
        }

        private void RemoveVote(BattleVote vote)
        {
            _battleVoteRepository.Delete(vote);
        }

        private bool CheckUserVotesNumber(User user, BattleVoteDTO battleVoteDTO)
        {
            var battle = GetBattle(battleVoteDTO);
            var userVotesNumber = _battleVoteRepository.Count(vote =>
                vote.BattleId == battleVoteDTO.BattleId && vote.UserId == user.Id);

            return battle.WinnersNumber > userVotesNumber;
        }

        private Battle GetBattle(BattleVoteDTO battleVoteDTO)
        {
            var battle = _battleRepository.GetById(battleVoteDTO.BattleId);

            if (battle == null)
            {
                throw new BattleVoteServiceException();
            }

            return battle;
        }

        private static BattleVote CreateBattleVote(User user, BattleVoteDTO battleVoteDTO)
        {
            return new BattleVote()
            {
                SemifinalistId = battleVoteDTO.SemifinalistId,
                BattleId = battleVoteDTO.BattleId,
                UserId = user.Id,
                Date = DateTime.Now
            };
        }
    }
}
