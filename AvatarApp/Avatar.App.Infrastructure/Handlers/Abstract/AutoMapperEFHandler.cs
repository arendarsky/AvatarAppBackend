using AutoMapper;

namespace Avatar.App.Infrastructure.Handlers.Abstract
{
    internal abstract class AutoMapperEFHandler: EFHandler
    {
        protected IMapper Mapper;

        protected AutoMapperEFHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext)
        {
            Mapper = mapper;
        }
    }
}
