using System;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Casting.Commands;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Semifinal;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Casting
{
    internal class AddSemifinalistHandler: EFHandler, IRequestHandler<AddSemifinalist>
    {
        public AddSemifinalistHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(AddSemifinalist request, CancellationToken cancellationToken)
        {
            var semifinalistDb = new SemifinalistDb
            {
                UserId = request.UserId,
                Date = DateTime.Now
            };

            await DbContext.Semifinalists.AddAsync(semifinalistDb, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
