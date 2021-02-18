using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Avatar.App.SharedKernel
{
    public interface IQueryManager
    {
        Task<IList<TDestination>> ToListAsync<TDestination>(IQueryable<TDestination> query, CancellationToken cancellationToken = default);
        Task<TDestination> FirstOrDefaultAsync<TDestination>(IQueryable<TDestination> query, CancellationToken cancellationToken = default);
        Task<int> CountAsync<TDestination>(IQueryable<TDestination> query, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync<TDestination>(IQueryable<TDestination> query, CancellationToken cancellationToken = default);
    }
}
