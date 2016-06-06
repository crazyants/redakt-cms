using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Redakt.Data.Repository;
using Redakt.Model;

namespace Redakt.Data.Mongo.Repository
{
    public class Repository<T>: IAsyncRepository<T> where T: class, IEntity
    {
        #region [ Constructors ]
        public Repository(IConnection connection)
        {
            this.Connection = connection;
        }
        #endregion
    
        #region [ Properties ]
        protected IConnection Connection { get; set; }

        protected IMongoCollection<T> Collection => this.Connection.Database.GetCollection<T>(typeof(T).Name);

        #endregion

        #region [ Async Methods ]
        public Task<T> GetAsync(string id)
        {
            return this.Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IList<T>> GetAsync(IEnumerable<string> ids)
        {
            return await this.Collection.Find(Builders<T>.Filter.In(x => x.Id, ids)).Limit(null).ToListAsync().ConfigureAwait(false);
        }

        public Task DeleteAsync(string id)
        {
            return this.Collection.DeleteOneAsync(x => x.Id == id);
        }

        public Task<bool> ExistsAsync(string id)
        {
            return this.AnyAsync(x => x.Id == id);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return this.Collection.CountAsync(new ExpressionFilterDefinition<T>(filter), new CountOptions {Limit = 1}).ContinueWith(t => t.Result > 0);
        }

        public Task SaveAsync(T entity, bool isUpsert = true)
        {
            entity.DbUpdated = DateTime.UtcNow;
            return this.Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, new UpdateOptions { IsUpsert = isUpsert });
        }

        public Task SaveAsync(IEnumerable<T> entities, bool isUpsert = true)
        {
            var models = new List<WriteModel<T>>();
            foreach (var entity in entities)
            {
                entity.DbUpdated = DateTime.UtcNow;
                models.Add(new ReplaceOneModel<T>(new BsonDocument("_id", entity.Id), entity) { IsUpsert = true });
            }

            return this.Collection.BulkWriteAsync(models);
        }

        //public Task UpdateAsync(T entity, params Expression<Func<T, object>>[] fieldExpressions)
        //{
        //    var fields = fieldExpressions.ToDictionary(k => k.GetMemberPath(), v => v.Compile());
        //    var update = Builders<T>.Update.Combine(fields.Select(f => Builders<T>.Update.Set(f.Key, f.Value.Invoke(entity)))).CurrentDate(x => x.DbUpdated);
        //    return this.Collection.UpdateOneAsync(x => x.Id == entity.Id, update, new UpdateOptions { IsUpsert = true });
        //}

        //public Task UpdateAsync(Expression<Func<T, bool>> filter, T entity, params Expression<Func<T, object>>[] fieldExpressions)
        //{
        //    var fields = fieldExpressions.ToDictionary(GetMemberPath, v => v.Compile());
        //    var update = Builders<T>.Update.Combine(fields.Select(f => Builders<T>.Update.Set(f.Key, f.Value.Invoke(entity)))).CurrentDate(x => x.DbUpdated);
        //    this.Collection.UpdateMany(filter, update);
        //    return this.Collection.UpdateManyAsync(filter, update);
        //}

        //public Task UpsertAsync(IEnumerable<T> entities, params Expression<Func<T, object>>[] fieldExpressions)
        //{
        //    var fields = fieldExpressions.ToDictionary(k => k.GetMemberPath(), v => v.Compile());
        //    var models = new List<WriteModel<T>>();
        //    foreach (var entity in entities)
        //    {
        //        entity.DbUpdated = DateTime.UtcNow;
        //        var updateDefinition = Builders<T>.Update.Combine(fields.Select(f => Builders<T>.Update.Set(f.Key, f.Value.Invoke(entity)))).Set(x => x.DbUpdated, entity.DbUpdated).SetOnInsert(x => x.DbCreated, entity.DbUpdated);
        //        models.Add(new UpdateOneModel<T>(new BsonDocument("_id", entity.Id), updateDefinition) { IsUpsert = true });
        //    }

        //    return this.Collection.BulkWriteAsync(models);
        //}

        //public Task UpdateAsync(Expression<Func<T, string>> filterField, IEnumerable<T> entities, params Expression<Func<T, object>>[] fieldExpressions)
        //{
        //    var filterFunction = filterField.Compile();
        //    var fields = fieldExpressions.ToDictionary(k => k.GetMemberPath(), v => v.Compile());
        //    var models = new List<WriteModel<T>>();
        //    foreach (var entity in entities)
        //    {
        //        entity.DbUpdated = DateTime.UtcNow;
        //        var updateDefinition = Builders<T>.Update.Combine(fields.Select(f => Builders<T>.Update.Set(f.Key, f.Value.Invoke(entity)))).Set(x => x.DbUpdated, entity.DbUpdated);
        //        models.Add(new UpdateManyModel<T>(Builders<T>.Filter.Eq(filterField, filterFunction(entity)), updateDefinition));
        //    }

        //    return this.Collection.BulkWriteAsync(models);
        //}

        public Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return this.Collection.Find(new ExpressionFilterDefinition<T>(filter)).FirstOrDefaultAsync();
        }

        public async Task<IList<T>> FindAsync(Expression<Func<T, bool>> filter)
        {
            return await this.Collection.Find(new ExpressionFilterDefinition<T>(filter)).Limit(null).ToListAsync().ConfigureAwait(false);
        }
        #endregion

        #region [ Helper Methods ]
        protected async Task<IList<T>> FindAsync<TVal>(string field, TVal value)
        {
            return await this.Collection.Find(Builders<T>.Filter.Eq(field, value)).Limit(null).ToListAsync().ConfigureAwait(false);
        }

        protected async Task<IList<T>> FindAsync<TVal>(string field, IEnumerable<TVal> values)
        {
            return await this.Collection.Find(Builders<T>.Filter.In(field, values)).Limit(null).ToListAsync().ConfigureAwait(false);
        }
        #endregion
    }
}
