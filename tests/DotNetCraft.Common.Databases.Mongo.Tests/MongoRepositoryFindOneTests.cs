using DotNetCraft.Common.Databases.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NSubstitute;

namespace DotNetCraft.Common.Databases.Mongo.Tests
{
    [TestClass]
    public class MongoRepositoryTests
    {
        [TestMethod]
        public async Task OnFindOneAsync_ReturnsEntity_WhenEntityExists()
        {
            var collection = Substitute.For<IMongoCollection<SimpleEntity>>();
            var logger = Substitute.For<ILogger<MongoRepository<SimpleEntity>>>();
            var specification = Substitute.For<ISpecification<SimpleEntity>>();

            var entity = new SimpleEntity
            {
                Id = "123",
                Name = "Test"
            };
            var asyncCursor = new MockAsyncCursor<SimpleEntity>(new List<SimpleEntity> { entity });

            // Arrange
            var sut = new MongoRepository<SimpleEntity>(collection, logger);
            
            collection.FindAsync(Arg.Any<FilterDefinition<SimpleEntity>>(), Arg.Any<FindOptions<SimpleEntity>>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(asyncCursor);

            // Act
            var result = await sut.FindOneAsync(specification);

            // Assert
            Assert.AreEqual(entity, result);

        }
    }
}