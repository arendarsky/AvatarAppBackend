using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Service.Models;

namespace Avatar.App.Service.Services
{
    public interface IRatingService
    {
        Task<ICollection<RatingItem>> GetAsync(int number);
    }
}
