using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetCraft.Common.Databases.Abstractions.Options;

namespace DotNetCraft.Common.Databases.Abstractions.Interfaces
{
    public interface IRepository<TEntity>
    where TEntity : class
    {
        Task<TEntity> FindOneAsync(ISpecification<TEntity> specification, CancellationToken token = default);
        IAsyncEnumerable<TEntity> FindManyAsync(ISpecification<TEntity> specification, CancellationToken token = default);
    }
}
