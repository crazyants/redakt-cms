using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redakt.Model;

namespace Redakt.Core.Cache
{
    public interface ICache
    {
        T GetOrDefault<T>(string id, T defaultValue = default(T)) where T : IEntity;
        IEnumerable<T> Get<T>(IEnumerable<string> ids) where T : IEntity;
        Task<T> AddOrGetExistingAsync<T>(string id, Func<string, Task<T>> getMethod) where T : IEntity;
        void Set<T>(T content) where T : IEntity;
        void Set<T>(IEnumerable<T> contents) where T : IEntity;
        void Remove(string key);
        bool Contains(string id);
    }
}
