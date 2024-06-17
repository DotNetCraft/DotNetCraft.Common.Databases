using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetCraft.Common.Databases.Abstractions.Implementations;
using DotNetCraft.Common.Databases.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace DotNetCraft.Common.Databases.Mongo
{
    internal class MongoRepository<TEntity> : BaseRepository<TEntity>
    where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly IProjectionBuilder<TEntity> _projectionBuilder;

        public MongoRepository(IMongoCollection<TEntity> collection, IProjectionBuilder<TEntity> projectionBuilder, ILogger<MongoRepository<TEntity>> logger) : base(logger)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _projectionBuilder = projectionBuilder ?? throw new ArgumentNullException(nameof(projectionBuilder));
        }

        #region Overrides of BaseRepository<TEntity>

        protected override async Task<TEntity> OnFindOneAsync(ISpecification<TEntity> specification, CancellationToken token = default)
        {
            var filter = specification.Filter == null
                ? FilterDefinition<TEntity>.Empty
                : Builders<TEntity>.Filter.Where(specification.Filter);

            var projectionDefinition = specification.ProjectionDefinition;

            var find = _collection.Find(filter);
            if (projectionDefinition != null)
            {
                var projection = _projectionBuilder.Build(projectionDefinition);
                find = find.Project<TEntity>(projection);
            }

            var entity = await find.FirstOrDefaultAsync(token);
            return entity;
        }

        protected override IAsyncEnumerable<TEntity> OnFindManyAsync(ISpecification<TEntity> specification, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
