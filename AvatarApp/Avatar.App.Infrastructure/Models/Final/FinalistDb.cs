using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Infrastructure.Models.Final
{
    public class FinalistDb: BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
