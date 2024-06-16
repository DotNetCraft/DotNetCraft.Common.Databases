using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetCraft.Common.Databases.Abstractions.Implementations;
using DotNetCraft.Common.Databases.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data.Entity;
using System.Linq;
using DotNetCraft.Common.Databases.Abstractions.Options;

namespace DotNetCraft.Common.Databases.MsSql
{
    internal class SqlRepository<TEntity>: BaseRepository<TEntity> 
        where TEntity : class
    {
        private readonly IDbSet<TEntity> _dbSet;

        public SqlRepository(IDbSet<TEntity> dbSet, ILogger<BaseRepository<TEntity>> logger) : base(logger)
        {
            _dbSet = dbSet ?? throw new ArgumentNullException(nameof(dbSet));
        }

        #region Overrides of BaseRepository<TEntity>

        protected override async Task<TEntity> OnFindOneAsync(ISpecification<TEntity> specification, CancellationToken token = default)
        {
            var query = specification.Filter == null ? _dbSet : _dbSet.Where(specification.Filter);
            if (specification.OrderDefinition != null)
            {
                if (specification.OrderDefinition.SortDirection == SortDirection.Ascending)
                    query = query.OrderBy(specification.OrderDefinition.OrderBy);
                else if (specification.OrderDefinition.SortDirection == SortDirection.Descending)
                    query = query.OrderByDescending(specification.OrderDefinition.OrderBy);
            }

            if (specification.SearchDefinition != null)
            {
                if (specification.SearchDefinition.Skip.HasValue)
                    query = query.Skip(specification.SearchDefinition.Skip.Value);
                if (specification.SearchDefinition.Take.HasValue)
                    query = query.Take(specification.SearchDefinition.Take.Value);
                else
                    query = query.Take(1);
            }

            if (specification.ProjectionDefinition != null)
            {
                var includes = specification.ProjectionDefinition.Includes;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                //TODO: Excludes
            }

            var item = await query.FirstOrDefaultAsync(token);
            return item;
        }

        protected override IAsyncEnumerable<TEntity> OnFindManyAsync(ISpecification<TEntity> specification, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
