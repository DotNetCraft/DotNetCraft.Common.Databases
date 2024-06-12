using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNetCraft.Common.Databases.Abstractions.Exceptions;
using DotNetCraft.Common.Databases.Abstractions.Interfaces;
using DotNetCraft.Common.Databases.Abstractions.Options;
using Microsoft.Extensions.Logging;

namespace DotNetCraft.Common.Databases.Abstractions.Implementations
{
    public abstract class BaseRepository<TEntity>: IRepository<TEntity>
    where TEntity : class
    {
        private readonly ILogger<BaseRepository<TEntity>> _logger;

        protected BaseRepository(ILogger<BaseRepository<TEntity>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Implementation of IRepository<TEntity>

        protected abstract Task<TEntity> OnFindOneAsync(ISpecification<TEntity> specification, CancellationToken token = default);
        protected abstract IAsyncEnumerable<TEntity> OnFindManyAsync(ISpecification<TEntity> specification, CancellationToken token = default);

        public async Task<TEntity> FindOneAsync(ISpecification<TEntity> specification, CancellationToken token = default)
        {
            try
            {
                var result = await OnFindOneAsync(specification, token);
                return result;
            }
            catch (Exception exception)
            {
                throw new ReadDatabaseException($"Failed to get a single entity by: {specification}", exception);
            }
        }

        public IAsyncEnumerable<TEntity> FindManyAsync(ISpecification<TEntity> specification, CancellationToken token = default)
        {
            try
            {
                var result = OnFindManyAsync(specification, token);
                return result;
            }
            catch (Exception exception)
            {
                throw new ReadDatabaseException($"Failed to a list of entities by: {specification}", exception);
            }
        }

        #endregion
    }
}
