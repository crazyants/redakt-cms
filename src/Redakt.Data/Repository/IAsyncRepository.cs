using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Redakt.Model;

namespace Redakt.Data.Repository
{
    public interface IAsyncRepository<T> where T: class, IEntity
    {
        Task<T> GetAsync(string id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<IList<T>> GetAsync(IEnumerable<string> ids);
        Task<IList<T>> FindAsync(Expression<Func<T, bool>> filter);
        Task<bool> ExistsAsync(string id);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        Task DeleteAsync(string id);
        Task SaveAsync(T entity, bool isUpsert = true);
        Task SaveAsync(IEnumerable<T> entities, bool isUpsert = true);
        //Task UpsertAsync(IEnumerable<T> entities, params Expression<Func<T, object>>[] fieldExpressions);
        //Task UpdateAsync(Expression<Func<T, string>> filterField, IEnumerable<T> entities, params Expression<Func<T, object>>[] fieldExpressions);
    }
}
