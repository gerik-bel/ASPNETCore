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
            Stream originalBody = context.Response.Body;
            try
            {
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;

                    if (_cacheImages.TryToGetFromCache(ref context, out byte[] response))
                    {
                        memStream.Position = 0;
                        await memStream.WriteAsync(response);
                    }
                    else
                    {
                        await _nextDelegate(context);
                        if (!string.IsNullOrEmpty(context.Response.ContentType) && context.Response.ContentType.StartsWith("image"))
                        {
                            memStream.Position = 0;
                            var responseBody = memStream.ToArray();
                            _cacheImages.AddToCache(context.Response.ContentType, context.Request.Path, responseBody);
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