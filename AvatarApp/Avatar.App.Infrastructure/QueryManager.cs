using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure
{
    internal class QueryManager: IQueryManager
    {
        public async Task<IList<TDestination>> ToListAsync<TDestination>(IQueryable<TDestination> query, CancellationToken cancellationToken = default)
        {
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TDestination> FirstOrDefaultAsync<TDestination>(IQueryable<TDestination> query, CancellationToken cancellationToken = default)
        {
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> CountAsync<TDestination>(IQueryable<TDestination> query, CancellationToken cancellationToken = default)
        {
            return await query.CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync<TDestination>(IQueryable<TDestination> query, CancellationToken cancellationToken = default)
        {
            return await query.AnyAsync(cancellationToken);
        }
    }
}
