using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace VG_AspNetCore_Web.Middleware.CacheImages
{
    public class CacheImages
    {
        private readonly CacheImageOptions _options;
        private readonly ConcurrentDictionary<string, CacheItem> _cacheDictionary;
        private Timer _timer;

        public CacheImages(CacheImageOptions options)
        {
            _options = options;
            _cacheDictionary = new ConcurrentDictionary<string, CacheItem>();
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

        public bool TryToGetFromCache(ref HttpContext context, out byte[] response)
        {
            response = new byte[0];
            if (_cacheDictionary.TryGetValue(context.Request.Path, out var cacheItem))
            {
                _timer?.Dispose();
                _timer = new Timer(CleanUpCache, null, _options.CacheExpirationInMilliseconds, -1);
                context.Response.Clear();
                context.Response.ContentType = cacheItem.ContentType;
                response = File.ReadAllBytes(cacheItem.FileName);
                return true;
            }
            return false;
        }

        public void AddToCache(string contentType, string url, byte[] responseBody)
        {
            var fileName = Path.Combine(_options.CachePath, $"{Guid.NewGuid().ToString()}.cache");
            DirectoryInfo info = new DirectoryInfo(_options.CachePath);
            if (!info.Exists)
            {
                info.Create();
            }
            File.WriteAllBytes(fileName, responseBody);
            _cacheDictionary.GetOrAdd(url, new CacheItem { FileName = fileName, ContentType = contentType, AddedTime = DateTime.Now });
            RemoveMoreThan(_options.MaxImageCount);
        }
    }
}
