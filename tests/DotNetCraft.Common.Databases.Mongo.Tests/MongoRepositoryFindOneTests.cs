using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCraft.Common.Databases.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NSubstitute;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetCraft.Common.Databases.Mongo.Tests
{
    [TestClass]
    public class MongoRepositoryTests
    {
        private readonly IMongoCollection<SimpleEntity> _collection;
        private readonly ILogger<MongoRepository<SimpleEntity>> _logger;
        private readonly ISpecification<SimpleEntity> _specification;
        private readonly IAsyncCursor<SimpleEntity> _asyncCursor;
        private readonly SimpleEntity _entity;

        public MongoRepositoryTests()
        {
            _collection = Substitute.For<IMongoCollection<SimpleEntity>>();
            _logger = Substitute.For<ILogger<MongoRepository<SimpleEntity>>>();
            _specification = Substitute.For<ISpecification<SimpleEntity>>();
            
            _entity = new SimpleEntity
            {
                Id = "123",
                Name = "Test"
            };
            _asyncCursor = new TestAsyncCursor<SimpleEntity>(_entity);
        }

        public class TestAsyncCursor<TEntity> : IAsyncCursor<TEntity>
        {
            private readonly TEntity _entity;

            public TestAsyncCursor(TEntity entity)
            {
                _entity = entity;
            }

            public IEnumerable<TEntity> Current
            {
                get
                {
                    return new[]
                    {
                        _entity
                    };
                }
            }

            public bool MoveNext(CancellationToken cancellationToken = default)
            {
                return true;
            }

            public Task<bool> MoveNextAsync(CancellationToken cancellationToken = default)
            {
                return Task.FromResult(true);
            }

            public void Dispose()
            {
                // Nothing to dispose
            }
        }


        [TestMethod]
        public async Task OnFindOneAsync_ReturnsEntity_WhenEntityExists()
        {
            // Arrange
            var sut = new MongoRepository<SimpleEntity>(_collection, _logger);
            
            _collection.FindAsync(Arg.Any<FilterDefinition<SimpleEntity>>(), Arg.Any<FindOptions<SimpleEntity>>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(_asyncCursor);

            // Act
            var result = await sut.FindOneAsync(_specification);

            // Assert
            Assert.AreEqual(_entity, result);

        }
    }
}