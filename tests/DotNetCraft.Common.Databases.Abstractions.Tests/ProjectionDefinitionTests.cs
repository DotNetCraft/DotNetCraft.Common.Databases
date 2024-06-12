using DotNetCraft.Common.Databases.Abstractions.Options;
using System.Linq.Expressions;

namespace DotNetCraft.Common.Databases.Abstractions.Tests
{
    [TestClass]
    public class ProjectionDefinitionTests
    {
        private class SimpleEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [TestMethod]
        public void ProjectionDefinitionIncludeTest()
        {
            var projection = new ProjectionDefinition<SimpleEntity>();
            Expression<Func<SimpleEntity, object>> field = entity => entity.Name;
            projection.Include(field);

            Assert.AreEqual(0, projection.Excludes.Count);
            Assert.AreEqual(1, projection.Includes.Count);
            Assert.AreEqual(field, projection.Includes[0]);
        }

        [TestMethod]
        public void ProjectionDefinitionExcludeTest()
        {
            var projection = new ProjectionDefinition<SimpleEntity>();
            Expression<Func<SimpleEntity, object>> field = entity => entity.Name;
            projection.Exclude(field);

            Assert.AreEqual(0, projection.Includes.Count);
            Assert.AreEqual(1, projection.Excludes.Count);
            Assert.AreEqual(field, projection.Excludes[0]);
        }
    }
}