using Microsoft.Extensions.Caching.Memory;
using System;

// It can be very useful with any object type
namespace Adrian
{
    public interface ICacheProvider
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
        void Remove(String key);
    }

    public class CacheProvider : ICacheProvider
    {
        private IMemoryCache cache;

        public CacheProvider(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void Set<T>(string key, T value)
        {
            cache.Set<T>(key, value, TimeSpan.FromMinutes(5));
        }

        public T Get<T>(string key)
        {
            return cache.Get<T>(key);
        }

        public void Remove(String key)
        {
            cache.Remove(key);
        }
    }
}