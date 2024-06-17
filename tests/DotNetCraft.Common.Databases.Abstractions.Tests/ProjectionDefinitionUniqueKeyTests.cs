using DotNetCraft.Common.Databases.Abstractions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCraft.Common.Databases.Abstractions.Tests
{
    [TestClass]
    public class ProjectionDefinitionUniqueKeyTests
    {
        [TestClass]
        public class ProjectionDefinitionTests
        {
            [TestMethod]
            public void GetUniqueKey_WithNoIncludesAndExcludes_ReturnsEntityTypeOnly()
            {
                // Arrange
                var projection = new ProjectionDefinition<MyEntity>();

                // Act
                var key = projection.GetUniqueKey();

                // Assert
                Assert.AreEqual("DotNetCraft.Common.Databases.Abstractions.Tests.ProjectionDefinitionUniqueKeyTests+MyEntity|Includes:|Excludes:", key);
            }

            [TestMethod]
            public void GetUniqueKey_WithIncludesAndNoExcludes_ReturnsCorrectKey()
            {
                // Arrange
                var projection = new ProjectionDefinition<MyEntity>();
                projection.Include(x => x.Property1);

                // Act
                var key = projection.GetUniqueKey();

                // Assert
                Assert.AreEqual("DotNetCraft.Common.Databases.Abstractions.Tests.ProjectionDefinitionUniqueKeyTests+MyEntity|Includes:Property1|Excludes:", key);
            }

            [TestMethod]
            public void GetUniqueKey_WithNoIncludesAndExcludes_ReturnsCorrectKey()
            {
                // Arrange
                var projection = new ProjectionDefinition<MyEntity>();
                projection.Exclude(x => x.Property2);

                // Act
                var key = projection.GetUniqueKey();

                // Assert
                Assert.AreEqual("DotNetCraft.Common.Databases.Abstractions.Tests.ProjectionDefinitionUniqueKeyTests+MyEntity|Includes:|Excludes:Property2", key);
            }

            [TestMethod]
            public void GetUniqueKey_WithIncludesAndExcludes_ReturnsCorrectKey()
            {
                // Arrange
                var projection = new ProjectionDefinition<MyEntity>();
                projection.Include(x => x.Property1);
                projection.Exclude(x => x.Property2);

                // Act
                var key = projection.GetUniqueKey();

                // Assert
                Assert.AreEqual("DotNetCraft.Common.Databases.Abstractions.Tests.ProjectionDefinitionUniqueKeyTests+MyEntity|Includes:Property1|Excludes:Property2", key);
            }
        }

        public class MyEntity
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }
        }
    }
}
