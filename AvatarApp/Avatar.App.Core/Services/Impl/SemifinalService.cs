using Avatar.App.Core.Entities;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Models;
using Avatar.App.Core.Specifications.UserSpecifications;
using Avatar.App.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avatar.App.Core.Services.Impl
{
    public class SemifinalService: BaseServiceWithAuthorization, ISemifinalService
    {
        private readonly IRepository<Battle> _battleRepository;
        private readonly IRepository<BattleSemifinalist> _battleSemifinalistRepository;
        private readonly IRepository<Semifinalist> _semifinalistRepository;
        private readonly IRepository<LikedVideo> _likedVideoRepository;
        private readonly IRepository<User> _userRepository;


        public SemifinalService(IRepository<Battle> battleRepository, IRepository<BattleSemifinalist> battleSemifinalistRepository, IRepository<Semifinalist> semifinalistRepository, IRepository<LikedVideo> likedVideoRepository, IRepository<User> userRepository) :
            base(userRepository)
        {
            _battleRepository = battleRepository;
            _battleSemifinalistRepository = battleSemifinalistRepository;
            _semifinalistRepository = semifinalistRepository;
            _likedVideoRepository = likedVideoRepository;
            _userRepository = userRepository;
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

        public async Task<bool> Vote(Guid userGuid, long battleId, long semifinalistId)
        {
            var battle = await _battleRepository.GetByIdAsync(battleId);

            var semifinalist = await _semifinalistRepository.GetByIdAsync(semifinalistId);

            if (battle == null || semifinalist == null)
                return false;

            var battleLike = new LikedVideo()
            {
                User = await GetUserAsync(new UserSpecification(userGuid)),
                Date = DateTime.Now,
                Video = semifinalist.Video, 
                Semifinalist = semifinalist,
                Battle = battle
            };

            var dbBattleLike = await _likedVideoRepository.GetAsync(b => 
            b.Semifinalist != null && b.User == battleLike.User && b.Video == battleLike.Video 
            && b.Battle == battleLike.Battle);

            if (dbBattleLike == null)
            {
                //вынести макс. кол-во в appsettings
                if (battle.Likes.Count(like => like.User == battleLike.User) == 2)
                    return false;

                await _likedVideoRepository.InsertAsync(battleLike);
            }
                
            else
                _likedVideoRepository.Delete(dbBattleLike);

            return true;
        }

        //public async Task<IEnumerable<Semifinalist>> GetBattleWinnersAsync(long battleId)
        //{
        //    var battle = await _battleRepository.GetByIdAsync(battleId);
        //    battle.Likes.GroupBy(l => l.Semifinalist).OrderByDescending(x => x.Count()).Select(x => x.FirstOrDefault());
        //}
    }
}
