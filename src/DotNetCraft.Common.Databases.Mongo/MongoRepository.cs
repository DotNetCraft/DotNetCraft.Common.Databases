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
    public class MongoRepository<TEntity> : BaseRepository<TEntity>
    where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoRepository(IMongoCollection<TEntity> collection, ILogger<MongoRepository<TEntity>> logger) : base(logger)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        #region Overrides of BaseRepository<TEntity>

        protected override async Task<TEntity> OnFindOneAsync(ISpecification<TEntity> specification, CancellationToken token = default)
        {
            var filter = specification.Filter == null
                ? FilterDefinition<TEntity>.Empty
                : Builders<TEntity>.Filter.Where(specification.Filter);

            var findOptions = new FindOptions<TEntity>
            {
                Limit = specification.SearchDefinition == null ? 1 : specification.SearchDefinition.Take,
                Skip = specification.SearchDefinition == null ? 1 : specification.SearchDefinition.Skip,
            };


            var projectionBuilder = Builders<TEntity>.Projection;
            ProjectionDefinition<TEntity> projection = null;

            var projectionDefinition = specification.ProjectionDefinition;
            if (projectionDefinition != null)
            {
                if (projectionDefinition.Includes.Count > 0)
                {
                    projection = projectionBuilder.Include(projectionDefinition.Includes[0]);
                    for (var index = 1; index < projectionDefinition.Includes.Count; index++)
                    {
                        var include = projectionDefinition.Includes[index];
                        projection = projection.Include(include);
                    }
                }

                if (projectionDefinition.Excludes.Count > 0)
                {
                    projection = projection == null
                        ? projectionBuilder.Exclude(projectionDefinition.Excludes[0])
                        : projection.Exclude(projectionDefinition.Excludes[0]);

                    for (var index = 1; index < projectionDefinition.Excludes.Count; index++)
                    {
                        var include = projectionDefinition.Excludes[index];
                        projection = projection.Exclude(include);
                    }
                }
            }

            var findResult = await _collection.FindAsync(filter, findOptions, token);
            var item = await findResult.FirstAsync(token);
            return item;
        }

        protected override IAsyncEnumerable<TEntity> OnFindManyAsync(ISpecification<TEntity> specification, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
