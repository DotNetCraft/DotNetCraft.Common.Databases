using DotNetCraft.Common.Databases.Abstractions.Implementations;
using DotNetCraft.Common.Databases.Abstractions.Interfaces;
using EphemeralMongo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
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
            var logger = new NullLogger<MongoRepository<SimpleEntity>>();
            var specification = Substitute.For<ISpecification<SimpleEntity>>();
            var projectionBuilder = Substitute.For<IProjectionBuilder<SimpleEntity>>();

            var entity = new SimpleEntity
            {
                Id = "123",
                Name = "Test"
            };
            var asyncCursor = new MockAsyncCursor<SimpleEntity>(new List<SimpleEntity> { entity });

            // Arrange
            var sut = new MongoRepository<SimpleEntity>(collection, projectionBuilder, logger);

            collection.FindAsync(Arg.Any<FilterDefinition<SimpleEntity>>(), Arg.Any<FindOptions<SimpleEntity>>(),
                    Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(asyncCursor);

            // Act
            var result = await sut.FindOneAsync(specification);

            // Assert
            Assert.AreEqual(entity, result);

        }

        [TestMethod]
        public async Task OnFindOneAsync_ReturnsEntity_WhenEntityExists2()
        {
            var logger = new NullLogger<MongoRepository<SimpleEntity>>();
            var projectionBuilder = Substitute.For<IProjectionBuilder<SimpleEntity>>();

            var entity = new SimpleEntity
            {
                Id = "123",
                Name = "Test"
            };

            var specification = new Specification<SimpleEntity>
            {
                Filter = x => x.Id == entity.Id
            };

            var options = new MongoRunnerOptions();
            using (var runner = MongoRunner.Run(options))
            {
                var database = new MongoClient(runner.ConnectionString).GetDatabase("default");

                // Do something with the database
                await database.CreateCollectionAsync("SimpleEntity");
                var collection = database.GetCollection<SimpleEntity>("SimpleEntity");

                await collection.InsertOneAsync(entity);

                var repository = new MongoRepository<SimpleEntity>(collection, projectionBuilder, logger);

                var actual = await repository.FindOneAsync(specification);

                Assert.AreEqual(entity.Id, actual.Id);
                Assert.AreEqual(entity.Name, actual.Name);
            }
        }
    }
}