using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DotNetCraft.Common.Databases.Abstractions.Options
{
    public enum SortDirection
    {
        None,
        Ascending,
        Descending
    }

    public class OrderDefinition<TEntity>
    where TEntity : class
    {
        public Expression<Func<TEntity, object>> OrderBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.None;
    }
}
