using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OTUS.Wexford.Core.Domain;
using OTUS.Wexford.Core.Domain.Administration;

namespace OTUS.Wexford.Core.Abstractions.Repositories
{
    public interface IRepository<T> where T: BaseEntity
    {
        Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);

        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<bool> RemoveByIdAsync(Guid id);

        Task<T> UpdateByIdAsync(Guid id, T employee);

        Task<T> AddAsync(T employee);
    }
}