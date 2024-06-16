using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DotNetCraft.Common.Databases.Abstractions.Implementations;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace DotNetCraft.Common.Databases.MsSql.Tests
{
    [TestClass]
    public class SqlRepositoryFindOneTests
    {
        [TestMethod]
        public async Task OnFindOneAsync_ReturnsEntity_WhenEntityExists()
        {
            // create substitution with async
            var data = new List<SimpleEntity>
            {
                new SimpleEntity
                {
                    Id = "123",
                    Name = "Test"
                }
            };

            var mockSet = Substitute.For<IDbSet<SimpleEntity>, IDbAsyncEnumerable<SimpleEntity>>().Initialize(data.AsQueryable());

            var logger = new NullLogger<SqlRepository<SimpleEntity>>();

            var specification = new Specification<SimpleEntity>
            {
                Filter = x => x.Id == "123"
            };

            var repository = new SqlRepository<SimpleEntity>(mockSet, logger);
            
            var result = await repository.FindOneAsync(specification);

            // Assert
            Assert.AreEqual("123", result.Id);
        }
    }
}