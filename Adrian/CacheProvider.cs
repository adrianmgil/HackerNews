using Microsoft.Extensions.Caching.Memory;
using System;

namespace Adrian
{
    public interface ICacheProvider
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
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
    }
}