using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace DotNetCraft.Common.Databases.Mongo
{
    public interface IProjectionBuilder<TEntity> where TEntity : class
    {
        ProjectionDefinition<TEntity> Build(Abstractions.Options.ProjectionDefinition<TEntity> projectionDefinition);
    }

    public class ProjectionBuilder<TEntity>: IProjectionBuilder<TEntity> 
        where TEntity : class
    {
        private readonly ILogger<ProjectionBuilder<TEntity>> _logger;
        private readonly ConcurrentDictionary<string, ProjectionDefinition<TEntity>> _cache;

        public ProjectionBuilder(ILogger<ProjectionBuilder<TEntity>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _cache = new ConcurrentDictionary<string, ProjectionDefinition<TEntity>>();
        }

        public ProjectionDefinition<TEntity> Build(Abstractions.Options.ProjectionDefinition<TEntity> projectionDefinition)
        {
            if (projectionDefinition == null)
            {
                return null;
            }

            var key = projectionDefinition.GetUniqueKey();

            if (_cache.TryGetValue(key, out var cachedProjection))
            {
                return cachedProjection;
            }

            var projectionBuilder = Builders<TEntity>.Projection;
            ProjectionDefinition<TEntity> projection = null;

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
                    var exclude = projectionDefinition.Excludes[index];
                    projection = projection.Exclude(exclude);
                }
            }

            _cache[key] = projection;

            return projection;
        }
    }

}
