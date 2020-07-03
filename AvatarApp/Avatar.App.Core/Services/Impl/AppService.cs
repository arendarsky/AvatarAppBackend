using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Specifications.UserSpecifications;
using Avatar.App.SharedKernel.Interfaces;
using Avatar.App.SharedKernel.Settings;
using Microsoft.Extensions.Options;

namespace Avatar.App.Core.Services.Impl
{
    public class AppService: BaseServiceWithAuthorization, IAppService
    {
        private readonly IRepository<Semifinalist> _semifinalistRepository;
        private readonly IRatingService _ratingService;

        public AppService(IRepository<Semifinalist> semifinalistRepository, IRepository<User> userRepository,
            IRatingService ratingService) : base(
            userRepository)
        {
            _semifinalistRepository = semifinalistRepository;
            _ratingService = ratingService;
        }

        public async Task SetSemifinalistAsync(long? userId)
        {
            var user = userId != null
                ? await GetUserAsync(new UserSpecification(userId.Value))
                : GetRatingFirstUser();

            await SetSemifinalistAsync(user);
        }

        private async Task SetSemifinalistAsync(User user)
        {
            await _semifinalistRepository.InsertAsync(new Semifinalist
            {
                UserId = user.Id,
                Date = DateTime.Now
            });
        }

        private User GetRatingFirstUser()
        {
            return _ratingService.GetCommonRating(1).First().User;
        }
    }
}
