using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using Redakt.Model;

namespace Redakt.Core.Cache
{
    public class InMemoryCache: ICache
    {
        private readonly ConcurrentDictionary<string, CacheObject> _cache = new ConcurrentDictionary<string, CacheObject>();
        private readonly Timer _pruningTimer = new Timer(300 * 100);
        private int _maxAgeSeconds = 300;

        public InMemoryCache()
        {
            _pruningTimer.Elapsed += (sender, args) => Prune();
            _pruningTimer.Start();
        }

        public T GetOrDefault<T>(string id, T defaultValue = default(T)) where T : IEntity
        {
            CacheObject val;

            if (_cache.TryGetValue(id, out val))
            {
                if (val.LastHit < DateTime.UtcNow.AddSeconds(-_maxAgeSeconds)) return defaultValue;
                val.LastHit = DateTime.UtcNow;
            }
            return (T)val?.Value;
        }

        public T GetOrSet<T>(string id, Func<string, T> getMethod) where T: IEntity
        {
            CacheObject val;

            if (!_cache.TryGetValue(id, out val) || val.LastHit < DateTime.UtcNow.AddSeconds(-_maxAgeSeconds))
            {
                var obj = getMethod(id);
                if (obj != null) Set(obj);
                return obj;
            }

            val.LastHit = DateTime.UtcNow;
            return (T)val.Value;
        }

        public void Set<T>(T obj) where T : IEntity
        {
            _cache[obj.Id] = new CacheObject(obj);
        }

        public void Remove(string id)
        {
            CacheObject val;
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
                _pruningTimer.Interval = value * 100;
                _maxAgeSeconds = value;
            }
        }

        private void Prune()
        {
            foreach (var key in _cache.Where(kv => kv.Value.LastHit < DateTime.UtcNow.AddSeconds(-_maxAgeSeconds)).Select(kv => kv.Key))
            {
                CacheObject val;
                _cache.TryRemove(key, out val);
            }
        }
    }
}
