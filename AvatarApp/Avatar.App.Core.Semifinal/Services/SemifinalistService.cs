using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Semifinal.DTO;
using Avatar.App.Core.Semifinal.Exceptions;
using Avatar.App.Core.Semifinal.Interfaces;
using Avatar.App.Core.Services;
using Avatar.App.Core.Services.Impl;
using Avatar.App.Core.Specifications.UserSpecifications;
using Avatar.App.SharedKernel.Interfaces;

namespace Avatar.App.Core.Semifinal.Services
{
    public class SemifinalistService: ISemifinalistService
    {
        private readonly IRepository<Semifinalist> _semifinalistRepository;
        private readonly IRatingService _ratingService;

        public SemifinalistService(IRepository<Semifinalist> semifinalistRepository, IRepository<User> userRepository,
            IRatingService ratingService)
        {
            _semifinalistRepository = semifinalistRepository;
            _ratingService = ratingService;
        }

        public Semifinalist CreateFromUserId(long? userId)
        {
            var userIdNotNull = SetupUserId(userId);
            var semifinalist = Create(userIdNotNull);
            return semifinalist;
        }

        private long SetupUserId(long? userId)
        {
            return userId ?? SetupUserIdFromFirstRatingPosition();
        }

        private long SetupUserIdFromFirstRatingPosition()
        {
            return GetRatingFirstUserId();
        }
        private long GetRatingFirstUserId()
        {
            var userProfile = _ratingService.GetCommonRating(1).FirstOrDefault();

            if (userProfile == null)
            {
                throw new SemifinalistException();
            }

            return userProfile.User.Id;
        }

        private static Semifinalist Create(long userId)
        {
            return new Semifinalist
            {
                UserId = userId,
                Date = DateTime.Now
            };
        }

        public async Task InsertSemifinalist(Semifinalist semifinalist)
        {
            await _semifinalistRepository.InsertAsync(semifinalist);
        }
    }
}
