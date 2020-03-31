namespace VG_AspNetCore_Web.Middleware.CacheImages
{
    public class CacheImageOptions
    {
        public string CachePath { get; set; }
        public int MaxImageCount { get; set; }
        public int CacheExpirationInMilliseconds { get; set; }
    }
}