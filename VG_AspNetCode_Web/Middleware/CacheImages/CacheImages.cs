using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace VG_AspNetCore_Web.Middleware.CacheImages
{
    public class CacheImages
    {
        private readonly CacheImageOptions _options;
        private readonly Dictionary<string, CacheItem> _cacheDictionary;
        private Timer _timer;

        public CacheImages(CacheImageOptions options)
        {
            _options = options;
            _cacheDictionary = new Dictionary<string, CacheItem>();
            DirectoryInfo info = new DirectoryInfo(_options.CachePath);
            if (info.Exists)
            {
                info.Delete(true);
            }
        }

        private void CleanUpCache(object input)
        {
            while (_cacheDictionary.Count > 0)
            {
                var cacheItem = _cacheDictionary.First();
                File.Delete(cacheItem.Value.FileName);
                _cacheDictionary.Remove(cacheItem.Key);
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
            _cacheDictionary.Add(url, new CacheItem { FileName = fileName, ContentType = contentType, AddedTime = DateTime.Now });
            if (_cacheDictionary.Count > _options.MaxImageCount)
            {
                var latest = _cacheDictionary.OrderBy(p => p.Value.AddedTime).First();
                File.Delete(latest.Value.FileName);
                _cacheDictionary.Remove(latest.Key);
            }
        }
    }
}
