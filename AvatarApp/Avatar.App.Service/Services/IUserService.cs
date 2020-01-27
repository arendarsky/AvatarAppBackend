using System.Threading.Tasks;

namespace Avatar.App.Service.Services
{
    public interface IUserService
    {
        Task<bool> SetName(string userName);
    }
}