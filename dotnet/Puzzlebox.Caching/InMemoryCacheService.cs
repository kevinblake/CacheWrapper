using System;
using System.Web;
using System.Web.Caching;

namespace Puzzlebox.Caching
{
    public class InMemoryCache : ICacheService
    {
        public T Get<T>(string cacheId, Func<T> getItemCallback) where T : class
        {
            return this.Get(cacheId, null, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), getItemCallback);
        }

        public T Get<T>(
            string cacheId, 
            CacheDependency cacheDependency, 
            DateTime absoluteExpiration, 
            TimeSpan slidingExpiration, 
            Func<T> getItemCallback) where T : class
        {
            var item = Get<T>(cacheId);
            if (item == null)
            {
                item = getItemCallback();
                HttpRuntime.Cache.Insert(cacheId, item, cacheDependency, absoluteExpiration, slidingExpiration);
            }

            return item;
        }

        public T Get<T>(
            string cacheId) where T : class
        {
            var item = HttpRuntime.Cache.Get(cacheId) as T;

            return item;
        }

        public void Remove(string cacheId)
        {
            HttpRuntime.Cache.Remove(cacheId);
        }

        public void Write<T>(string cacheId, T item) where T : class
        {
            HttpRuntime.Cache.Insert(cacheId, item);
        }

        public void Write<T>(string cacheId, T item, CacheDependency cacheDependency, DateTime absoluteExpiration, TimeSpan slidingExpiration) where T : class
        {
            HttpRuntime.Cache.Insert(cacheId, item, cacheDependency, absoluteExpiration, slidingExpiration);
        }


    }
}