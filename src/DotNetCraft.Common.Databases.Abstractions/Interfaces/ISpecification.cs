using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DotNetCraft.Common.Databases.Abstractions.Options;

namespace DotNetCraft.Common.Databases.Abstractions.Interfaces
{
    public interface ISpecification<TEntity>
    where TEntity : class
    {
        Expression<Func<TEntity, bool>> Filter { get; set; }
        OrderDefinition<TEntity> OrderDefinition { get; set; }
        SearchDefinition SearchDefinition { get; set; }
        ProjectionDefinition<TEntity> ProjectionDefinition { get; set; }
    }
}
