using Avatar.App.Infrastructure.Models;

namespace Avatar.App.Infrastructure.Handlers.Abstract
{
    internal interface IGenericInsertHandler : IGenericHandler<object, BaseEntity>
    {

    }

    internal interface IGenericHandler: IGenericHandler<BaseEntity>
    {
    }

    internal interface IGenericHandler<TBaseSource> : IGenericHandler<TBaseSource, object>
    {
    }

    internal interface IGenericHandler<TBaseSource, TBaseDestination>
    {

    }
}
