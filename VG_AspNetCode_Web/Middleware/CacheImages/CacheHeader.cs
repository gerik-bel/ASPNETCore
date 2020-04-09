using System;

namespace VG_AspNetCore_Web.Middleware.CacheImages
{
    public class CacheHeader
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime AddedTime { get; set; }
    }
}