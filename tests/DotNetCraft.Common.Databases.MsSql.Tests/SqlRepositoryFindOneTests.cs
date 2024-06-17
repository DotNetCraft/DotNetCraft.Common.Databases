using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DotNetCraft.Common.Databases.Abstractions.Implementations;
using DotNetCraft.Common.Databases.MsSql.Tests.Utils;
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
            var entity = new SimpleEntity
            {
                Id = "123",
                Name = "Test"
            };


            var data = new List<SimpleEntity> { entity };

            var mockSet = Substitute.For<IDbSet<SimpleEntity>, IDbAsyncEnumerable<SimpleEntity>>().Initialize(data.AsQueryable());

            var logger = new NullLogger<SqlRepository<SimpleEntity>>();

            var specification = new Specification<SimpleEntity>
            {
                Filter = x => x.Id == "123"
            };

            var repository = new SqlRepository<SimpleEntity>(mockSet, logger);
            
            var actual = await repository.FindOneAsync(specification);

            Assert.AreEqual(entity.Id, actual.Id);
            Assert.AreEqual(entity.Name, actual.Name);
        }
    }
}