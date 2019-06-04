using System;
using System.Web.Caching;

namespace Puzzlebox.Caching
{
    public class CascadingCache : ICacheService
    {
        private readonly ICacheService[] cacheList;

        public CascadingCache(string defaultSaveDirectory)
        {
            this.cacheList = new ICacheService[] { new InMemoryCache(), new FileSystemCache(defaultSaveDirectory) };
        }

        public CascadingCache(ICacheService[] cacheList)
        {
            this.cacheList = cacheList;
        }

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
            var c = this.cacheList.GetEnumerator();
            c.MoveNext();
            var r = ((ICacheService)c.Current).Get(
                cacheId, 
                () =>
                    {
                        c.MoveNext();
                        var nextProvider = c.Current as ICacheService;
                        if (nextProvider != null)
                        {
                            return nextProvider.Get(
                                cacheId, 
                                cacheDependency, 
                                absoluteExpiration, 
                                slidingExpiration, 
                                getItemCallback);
                        }

                        return getItemCallback();
                    });

            return r;
        }

        public void Write<T>(string cacheId, T item) where T : class
        {
            foreach (var cache in this.cacheList)
            {
                cache.Write(cacheId, item);
            }
        }

        public T Get<T>(string cacheId) where T : class
        {
            foreach (var cache in this.cacheList)
            {
                var i = cache.Get<T>(cacheId);

                if (i != null)
                {
                    return i;
                }
            }

            return null;
        }

        public void Write<T>(string cacheId, T item, CacheDependency cacheDependency, DateTime absoluteExpiration,
            TimeSpan slidingExpiration) where T : class
        {
            foreach (var cache in this.cacheList)
            {
                cache.Write(cacheId, item, cacheDependency, absoluteExpiration, slidingExpiration);
            }
        }
    }
}