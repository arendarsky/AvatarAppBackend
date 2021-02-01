using AutoMapper;

namespace Avatar.App.Infrastructure.CommandHandlers.Semifinal
{
    internal abstract class SemifinalHandler
    {
        protected AvatarAppContext DbContext;

        protected SemifinalHandler(AvatarAppContext dbContext)
        {
            DbContext = dbContext;
        }
    }

    internal abstract class AutoMapperSemifinalHandler: SemifinalHandler
    {
        protected IMapper Mapper;

        protected AutoMapperSemifinalHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext)
        {
            Mapper = mapper;
        }
    }
}
