using MongoDB.Driver;

namespace DotNetCraft.Common.Databases.Mongo.Tests
{
    internal class MockAsyncCursor<TEntity> : IAsyncCursor<TEntity>
    {
        private readonly IEnumerable<TEntity> _entities;

        public MockAsyncCursor(IEnumerable<TEntity> entities)
        {
            _entities = entities ?? throw new ArgumentNullException(nameof(entities));
        }

        public IEnumerable<TEntity> Current => _entities;

        public bool MoveNext(CancellationToken cancellationToken = default)
        {
            using (var enumerator = _entities.GetEnumerator())
            {
                return enumerator.MoveNext();
            }
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken = default)
        {
            using (var enumerator = _entities.GetEnumerator())
            {
                return Task.FromResult(enumerator.MoveNext());
            }
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}
