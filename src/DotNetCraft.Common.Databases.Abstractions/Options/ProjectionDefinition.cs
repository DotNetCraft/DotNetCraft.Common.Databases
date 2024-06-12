using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DotNetCraft.Common.Databases.Abstractions.Options
{
    public class ProjectionDefinition<TEntity>
    where TEntity : class
    {
        private readonly List<Expression<Func<TEntity, object>>> _includes = new List<Expression<Func<TEntity, object>>>();
        private readonly List<Expression<Func<TEntity, object>>> _excludes = new List<Expression<Func<TEntity, object>>>();

        public IReadOnlyList<Expression<Func<TEntity, object>>> Includes => _includes;
        public IReadOnlyList<Expression<Func<TEntity, object>>> Excludes => _excludes;

        public void Include(Expression<Func<TEntity, object>> field)
        {
            _includes.Add(field);
        }

        public void Exclude(Expression<Func<TEntity, object>> field)
        {
            _excludes.Add(field);
        }
    }
}
