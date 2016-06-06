using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Redakt.Data.Repository;
using Redakt.Model;

namespace Redakt.Data.Test.Repository
{
    public class Repository<T>: IAsyncRepository<T> where T: class, IEntity
    {
        #region [ Constructors ]
        public Repository()
        {
        }
        #endregion

        #region [ Properties ]
        internal static List<T> Collection = new List<T>();
        #endregion

        #region [ Async Methods ]
        public Task<T> GetAsync(string id)
        {
            return Task.FromResult(Collection.FirstOrDefault(x => x.Id == id));
        }

        public Task<IList<T>> GetAsync(IEnumerable<string> ids)
        {
            return Task.FromResult((IList<T>)Collection.Where(x => ids.Contains(x.Id)).ToList());
        }

        public Task DeleteAsync(string id)
        {
            Collection.RemoveAll(x => x.Id == id);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string id)
        {
            return this.AnyAsync(x => x.Id == id);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return Task.FromResult(Collection.Count(filter.Compile()) > 0);
        }

        public Task SaveAsync(T entity, bool isUpsert = true)
        {
            if (entity.Id == null) entity.Id = Guid.NewGuid().ToString("N");
            entity.DbUpdated = DateTime.UtcNow;
            if (isUpsert || Collection.Any(x => x.Id == entity.Id))
            {
                Collection.RemoveAll(x => x.Id == entity.Id);
                Collection.Add(entity);
            }
            return Task.CompletedTask;
        }

        public Task SaveAsync(IEnumerable<T> entities, bool isUpsert = true)
        {
            return Task.WhenAll(entities.Select(x => SaveAsync(x)));
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return Task.FromResult(Collection.FirstOrDefault(filter.Compile()));
        }

        public Task<IList<T>> FindAsync(Expression<Func<T, bool>> filter)
        {
            return Task.FromResult((IList<T>)Collection.Where(filter.Compile()).ToList());
        }
        #endregion
    }
}
