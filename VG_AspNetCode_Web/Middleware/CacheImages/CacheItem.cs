namespace VG_AspNetCore_Web.Middleware.CacheImages
{
    public class CacheItem
    {
        public bool Exist { get; set; }
        public byte[] Image { get; set; }
        public string ContentType { get; set; }
    }
}