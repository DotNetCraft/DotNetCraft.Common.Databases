using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DotNetCraft.Common.Databases.Abstractions.Options
{
    public class ProjectionDefinition<TEntity>
    where TEntity : class
    {
        private readonly List<Expression<Func<TEntity, object>>> _includes = new List<Expression<Func<TEntity, object>>>();
        private readonly List<Expression<Func<TEntity, object>>> _excludes = new List<Expression<Func<TEntity, object>>>();

        public IReadOnlyList<Expression<Func<TEntity, object>>> Includes => _includes;
        public IReadOnlyList<Expression<Func<TEntity, object>>> Excludes => _excludes;

        public void Include(Expression<Func<TEntity, object>> field)
        {
            _includes.Add(field);
        }

        public void Exclude(Expression<Func<TEntity, object>> field)
        {
            _excludes.Add(field);
        }

        public string GetUniqueKey()
        {
            var entityType = typeof(TEntity).FullName;

            var includeKeys = _includes.Select(ExpressionHelper.GetExpressionPath).ToList();
            var excludeKeys = _excludes.Select(ExpressionHelper.GetExpressionPath).ToList();

            var key = $"{entityType}|Includes:{string.Join(",", includeKeys)}|Excludes:{string.Join(",", excludeKeys)}";

            return key;
        }
    }

    public static class ExpressionHelper
    {
        public static string GetExpressionPath<T>(Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression ?? ((UnaryExpression)expression.Body).Operand as MemberExpression;
            var path = memberExpression.ToString();
            return path.Substring(path.IndexOf('.') + 1);
        }
    }
}
