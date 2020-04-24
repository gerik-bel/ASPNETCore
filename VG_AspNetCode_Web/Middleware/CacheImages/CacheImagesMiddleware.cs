using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace VG_AspNetCore_Web.Middleware.CacheImages
{
    public class CacheImagesMiddleware
    {
        private readonly RequestDelegate _nextDelegate;
        private readonly CacheImages _cacheImages;

        public CacheImagesMiddleware(RequestDelegate next, CacheImageOptions options)
        {
            _nextDelegate = next;
            _cacheImages = new CacheImages(options);
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.ToString().StartsWith("/api/"))
            {
                await _nextDelegate(context);
            }
            else
            {
                Stream originalBody = context.Response.Body;
                try
                {
                    using (var memStream = new MemoryStream())
                    {
                        context.Response.Body = memStream;
                        var cacheItem = await _cacheImages.TryToGetFromCacheAsync(context.Request.Path);
                        if (cacheItem.Exist)
                        {
                            context.Response.Clear();
                            context.Response.ContentType = cacheItem.ContentType;
                            memStream.Position = 0;
                            await memStream.WriteAsync(cacheItem.Image);
                        }
                        else
                        {
                            await _nextDelegate(context);
                            if (!string.IsNullOrEmpty(context.Response.ContentType) &&
                                context.Response.ContentType.StartsWith("image"))
                            {
                                memStream.Position = 0;
                                var responseBody = memStream.ToArray();
                                await _cacheImages.AddToCacheAsync(context.Response.ContentType, context.Request.Path,
                                    responseBody);
                            }
                        }

                        memStream.Position = 0;
                        await memStream.CopyToAsync(originalBody);
                    }
                }
                finally
                {
                    context.Response.Body = originalBody;
                }
            }
        }
    }
}