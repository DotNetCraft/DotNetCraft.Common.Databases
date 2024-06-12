using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DotNetCraft.Common.Databases.Abstractions.Interfaces;
using DotNetCraft.Common.Databases.Abstractions.Options;

namespace DotNetCraft.Common.Databases.Abstractions.Implementations
{
    public class Specification<TEntity>: ISpecification<TEntity>
    where TEntity : class
    {
        #region Implementation of ISpecification<TEntity>

        public Expression<Func<TEntity, bool>> Filter { get; set; }
        public OrderDefinition<TEntity> OrderDefinition { get; set; }
        public SearchDefinition SearchDefinition { get; set; }
        public ProjectionDefinition<TEntity> ProjectionDefinition { get; set; }

        #endregion
    }
}
