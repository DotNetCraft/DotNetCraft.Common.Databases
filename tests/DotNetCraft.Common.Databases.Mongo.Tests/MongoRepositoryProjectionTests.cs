using DotNetCraft.Common.Databases.Abstractions.Implementations;
using EphemeralMongo;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Driver;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCraft.Common.Databases.Mongo.Tests
{
    [TestClass]
    public class MongoRepositoryProjectionTests
    {
        [TestMethod]
        public async Task SimpleProjectionTest()
        {
            var logger = new NullLogger<MongoRepository<SimpleEntity>>();
            var projectionBuilder =
                new ProjectionBuilder<SimpleEntity>(new NullLogger<ProjectionBuilder<SimpleEntity>>());

            var entity = new SimpleEntity
            {
                Id = "123",
                Name = "Test"
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

                var specification = new Specification<SimpleEntity>
                {
                    Filter = x => x.Id == entity.Id,
                    ProjectionDefinition = new Abstractions.Options.ProjectionDefinition<SimpleEntity>()
                };
                specification.ProjectionDefinition.Include(x=>x.Id);

                var actual = await repository.FindOneAsync(specification);

                Assert.AreEqual(entity.Id, actual.Id);
                Assert.IsNull(actual.Name);
            }
        }
    }
}
