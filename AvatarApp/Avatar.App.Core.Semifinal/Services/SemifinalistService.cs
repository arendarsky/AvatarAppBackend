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

        private long UserId { get; set; }
        private Semifinalist Semifinalist { get; set; }

        public SemifinalistService(IRepository<Semifinalist> semifinalistRepository, IRepository<User> userRepository,
            IRatingService ratingService)
        {
            _semifinalistRepository = semifinalistRepository;
            _ratingService = ratingService;
        }

        public Semifinalist CreateFromUserId(long? userId)
        {

            SetupUserId(userId);
            Semifinalist = CreateFromUserId();
            return Semifinalist;
        }

        private void SetupUserId(long? userId)
        {
            if (userId == null)
            {
                SetupUserIdFromFirstRatingPosition();
                return;
            }
            UserId = userId.Value;
        }

        private void SetupUserIdFromFirstRatingPosition()
        {
            UserId = GetRatingFirstUserId();
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

        private Semifinalist CreateFromUserId()
        {
            return new Semifinalist
            {
                UserId = UserId,
                Date = DateTime.Now
            };
        }

        public async Task InsertSemifinalist(Semifinalist semifinalist)
        {
            await _semifinalistRepository.InsertAsync(semifinalist);
        }
    }
}
