using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Exceptions;
using Avatar.App.SharedKernel.Interfaces;

namespace Avatar.App.Core.Services.Impl
{
    public abstract class BaseServiceWithAuthorization
    {
        protected readonly IRepository<User> UserRepository;

        protected BaseServiceWithAuthorization(IRepository<User> userRepository)
        {
            UserRepository = userRepository;
        }

        protected virtual async Task<User> GetUserAsync(ISpecification<User> specification)
        {
            var user = await UserRepository.GetAsync(specification);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return user;
        }
    }
}
