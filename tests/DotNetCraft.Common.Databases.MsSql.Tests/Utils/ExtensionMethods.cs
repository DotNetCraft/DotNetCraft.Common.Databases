using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace DotNetCraft.Common.Databases.MsSql.Tests.Utils
{
    public static class ExtensionMethods
    {
        public static IDbSet<T> Initialize<T>(this IDbSet<T> dbSet, IQueryable<T> data) where T : class
        {
            dbSet.Provider.Returns(data.Provider);
            dbSet.Expression.Returns(data.Expression);
            dbSet.ElementType.Returns(data.ElementType);
            dbSet.GetEnumerator().Returns(data.GetEnumerator());

            if (dbSet is IDbAsyncEnumerable)
            {
                ((IDbAsyncEnumerable<T>)dbSet).GetAsyncEnumerator()
                    .Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));
                dbSet.Provider.Returns(new TestDbAsyncQueryProvider<T>(data.Provider));
            }

            return dbSet;
        }
    }
}
