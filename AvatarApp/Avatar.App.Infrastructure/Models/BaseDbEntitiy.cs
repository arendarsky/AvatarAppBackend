using System;
using System.Linq.Expressions;

namespace Avatar.App.Infrastructure.Models
{
    internal abstract class BaseEntity
    {
        public long Id { get; set; }

        public void UpdateProperty<TValue>(AvatarAppContext dbContext, string propertyName, TValue value)
        {
            var property = dbContext.Entry(this).Property(propertyName);
            property.CurrentValue = value;
            property.IsModified = true;
        }

        public void UpdateProperty<TEntity, TProperty>(AvatarAppContext dbContext, Expression<Func<TEntity, TProperty>> expression, TProperty value) where TEntity : class, new()
        {
            var source = new TEntity();
            var idProperty = source.GetType().GetProperty("Id");

            if (idProperty == null)
            {
                return;
            }

            idProperty.SetValue(source, Id);
            var property = dbContext.Entry(source).Property(expression);
            property.CurrentValue = value;
            property.IsModified = true;
        }
    }
}
