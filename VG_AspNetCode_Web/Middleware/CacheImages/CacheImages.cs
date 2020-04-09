using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VG_AspNetCore_Web.Middleware.CacheImages
{
    public class CacheImages
    {
        private readonly CacheImageOptions _options;
        private readonly ConcurrentDictionary<string, CacheHeader> _cacheDictionary;
        private Timer _timer;

        public CacheImages(CacheImageOptions options)
        {
            _options = options;
            _cacheDictionary = new ConcurrentDictionary<string, CacheHeader>();
            DirectoryInfo info = new DirectoryInfo(_options.CachePath);
            if (info.Exists)
            {
                info.Delete(true);
            }
        }

        private void CleanUpCache(object input)
        {
            RemoveMoreThan(0);
        }

        private void RemoveMoreThan(int maxNumber)
        {
            while (_cacheDictionary.Count > maxNumber)
            {
                var cacheItem = _cacheDictionary.OrderBy(p => p.Value.AddedTime).First();
                File.Delete(cacheItem.Value.FileName);
                _cacheDictionary.Remove(cacheItem.Key, out _);
            }
        }

        public async Task<CacheItem> TryToGetFromCacheAsync(string path)
        {
            CacheItem result = new CacheItem { Exist = false };
            if (_cacheDictionary.TryGetValue(path, out var cacheItem))
            {
                _timer?.Dispose();
                _timer = new Timer(CleanUpCache, null, _options.CacheExpirationInMilliseconds, -1);
                result.Exist = true;
                result.Image = await File.ReadAllBytesAsync(cacheItem.FileName);
                result.ContentType = cacheItem.ContentType;
            }
            return result;
        }

        public async Task AddToCacheAsync(string contentType, string url, byte[] responseBody)
        {
            var fileName = Path.Combine(_options.CachePath, $"{Guid.NewGuid().ToString()}.cache");
            DirectoryInfo info = new DirectoryInfo(_options.CachePath);
            if (!info.Exists)
            {
                info.Create();
            }
            await File.WriteAllBytesAsync(fileName, responseBody);
            _cacheDictionary.GetOrAdd(url, new CacheHeader { FileName = fileName, ContentType = contentType, AddedTime = DateTime.Now });
            RemoveMoreThan(_options.MaxImageCount);
        }
    }
}