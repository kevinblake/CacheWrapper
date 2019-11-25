using System;
using System.IO;
using System.Web;
using System.Web.Caching;

using Puzzlebox.Caching.Helpers;

namespace Puzzlebox.Caching
{
    public class FileSystemCache : ICacheService
    {
        public FileSystemCache(string saveDirectory)
        {
            this.SaveDirectory = saveDirectory;
        }

        private string SaveDirectory { get; set; }

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
            var item = HttpRuntime.Cache.Get(cacheId) as T;
            if (item == null)
            {
                item = getItemCallback();
                this.Write(cacheId, item);
            }

            return item;
        }

        public void Write<T>(string cacheId, T item) where T : class
        {
            var path = string.Format("{0}{1}.xml", this.SaveDirectory, cacheId);

            SerializationHelper.SaveToDisk(path, item);
        }

        public T Get<T>(string cacheId) where T : class
        {
            var path = string.Format("{0}{1}.xml", this.SaveDirectory, cacheId);

            return SerializationHelper.LoadFromDisk<T>(path);
        }

        public void Remove(string cacheId)
        {
            var path = string.Format("{0}{1}.xml", this.SaveDirectory, cacheId);

            File.Delete(path);
        }

        public void Write<T>(string cacheId, T item, CacheDependency cacheDependency, DateTime absoluteExpiration,
            TimeSpan slidingExpiration) where T : class
        {
            var path = string.Format("{0}{1}.xml", this.SaveDirectory, cacheId);

            SerializationHelper.SaveToDisk(path, item);
        }
    }
}