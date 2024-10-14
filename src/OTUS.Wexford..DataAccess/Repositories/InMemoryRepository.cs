using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OTUS.Wexford.Core.Abstractions.Repositories;
using OTUS.Wexford.Core.Domain;
using static System.Reflection.Metadata.BlobBuilder;

namespace OTUS.Wexford.DataAccess.Repositories
{
    public class InMemoryRepository<T>(IList<T> data) : IRepository<T> where T : BaseEntity
    {
        protected IList<T> Data { get; set; } = data;

        public Task<IList<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Data);
        }

        public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
            return result;
        }

        public Task<T> AddAsync(T entity)
        {
            Data.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<bool> RemoveByIdAsync(Guid id)
        {
            var item = Data.FirstOrDefault(u => u.Id == id);
            if (item != null)
            {
                Data.RemoveAt(Data.IndexOf(item));
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<T> UpdateByIdAsync(Guid id, T employee)
        {
            Data.RemoveAt(Data.IndexOf(Data.FirstOrDefault(u => u.Id == id)));
            Data.Add(employee);
            return Task.FromResult(employee);
        }
    }
}