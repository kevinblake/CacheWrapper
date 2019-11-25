using System;
using System.Web.Caching;

namespace Puzzlebox.Caching
{
    public interface ICacheService
    {
        T Get<T>(string cacheId, Func<T> getItemCallback) where T : class;

        T Get<T>(
            string cacheId, 
            CacheDependency cacheDependency, 
            DateTime absoluteExpiration, 
            TimeSpan slidingExpiration, 
            Func<T> getItemCallback) where T : class;

        void Write<T>(string cacheId, T item) where T : class;

        T Get<T>(
            string cacheId) where T : class;

        void Remove(string cacheId);

        void Write<T>(string cacheId, T item, CacheDependency cacheDependency, DateTime absoluteExpiration, TimeSpan slidingExpiration) where T : class;
    }
}