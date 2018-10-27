using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Airlock.AspNetCore.RateLimit.PolicyCounter
{
    static class DistributedCacheExtensions
    {
        private static readonly JsonSerializer Serializer = new JsonSerializer();

        public static async Task<T> GetJsonAsync<T>(this IDistributedCache cache, string key)
        {
            var value = await cache.GetAsync(key);
            if (value == null)
                return default(T);

            var reader = new JsonTextReader(new StreamReader(new MemoryStream(value)));
            return Serializer.Deserialize<T>(reader);
        }

        public static Task SetJsonAsync(this IDistributedCache cache, string key, object value, DistributedCacheEntryOptions options)
        {
            var ms = new MemoryStream();
            var jsonWriter = new StreamWriter(ms);
            Serializer.Serialize(jsonWriter, value);
            jsonWriter.Flush();
            return cache.SetAsync(key, ms.ToArray(), options);
        }
    }
}
