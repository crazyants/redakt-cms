using System;
using System.Collections.Concurrent;
using System.Linq;
using Redakt.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redakt.Core.Cache
{
    public class InMemoryCache: ICache
    {
        private readonly ConcurrentDictionary<string, CacheItem> _cache = new ConcurrentDictionary<string, CacheItem>();
        //private readonly Timer _pruningTimer = new Timer(300 * 100);
        private int _maxAgeSeconds = 300;

        public InMemoryCache()
        {
            //_pruningTimer.Elapsed += (sender, args) => Prune();
            //_pruningTimer.Start();
        }

        public T GetOrDefault<T>(string id, T defaultValue = default(T)) where T : IEntity
        {
            CacheItem item;

            if (_cache.TryGetValue(id, out item))
            {
                if (!IsFresh(item)) return defaultValue;
                item.LastHit = DateTime.UtcNow;
            }
            return (T)item?.Value;
        }

        public IEnumerable<T> Get<T>(IEnumerable<string> ids) where T : IEntity
        {
            foreach (var id in ids)
            {
                CacheItem item;
                if (_cache.TryGetValue(id, out item) && IsFresh(item))
                {
                    item.LastHit = DateTime.UtcNow;
                    yield return (T)item.Value;
                }
            }
        }

        public async Task<T> AddOrGetExistingAsync<T>(string id, Func<string, Task<T>> getMethod) where T : IEntity
        {
            CacheItem item;
            if (!_cache.TryGetValue(id, out item) || !IsFresh(item))
            {
                var obj = await getMethod(id);
                if (obj != null) Set(obj);
                return obj;
            }

            item.LastHit = DateTime.UtcNow;
            return (T)item.Value;
        }

        //public T AddOrGetExisting<T>(string id, Func<string, T> getMethod) where T: IEntity
        //{
        //    CacheItem item;

        //    if (!_cache.TryGetValue(id, out item) || !IsFresh(item))
        //    {
        //        var obj = getMethod(id);
        //        if (obj != null) Set(obj);
        //        return obj;
        //    }

        //    item.LastHit = DateTime.UtcNow;
        //    return (T)item.Value;
        //}

        public void Set<T>(T obj) where T : IEntity
        {
            _cache[obj.Id] = new CacheItem(obj);
        }

        public void Set<T>(IEnumerable<T> objs) where T : IEntity
        {
            foreach (var obj in objs) Set(obj);
        }

        public void Remove(string id)
        {
            CacheItem val;
            _cache.TryRemove(id, out val);
        }

        public bool Contains(string id)
        {
            return _cache.ContainsKey(id);
        }

        public int MaxAgeSeconds
        {
            get { return _maxAgeSeconds; }
            set
            {
               // _pruningTimer.Interval = value * 100;
                _maxAgeSeconds = value;
            }
        }

        private void Prune()
        {
            foreach (var key in _cache.Where(kv => kv.Value.LastHit < DateTime.UtcNow.AddSeconds(-_maxAgeSeconds)).Select(kv => kv.Key))
            {
                CacheItem val;
                _cache.TryRemove(key, out val);
            }
        }

        private bool IsFresh(CacheItem obj)
        {
            return obj.LastHit >= DateTime.UtcNow.AddSeconds(-_maxAgeSeconds);
        }
    }
}
