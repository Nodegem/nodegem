using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Nodester.WebApi.Extensions
{
    public static class DistributedCacheExtension
    {
        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key)
        {
            var value = await cache.GetAsync(key);
            if (value == null) return default;

            var stringValue = Encoding.UTF8.GetString(value);
            return JsonConvert.DeserializeObject<T>(stringValue);
        }
        
        public static async Task<T> GetAsync<T>(this IDistributedCache cache, Guid key)
        {
            return await cache.GetAsync<T>(key.ToString());
        }

        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value,
            TimeSpan expiration = default)
        {
            if (expiration.Equals(default))
            {
                expiration = TimeSpan.FromMinutes(30);
            }

            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(expiration);

            await cache.SetAsync(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), options);
        }
        
        public static async Task SetAsync<T>(this IDistributedCache cache, Guid key, T value,
            TimeSpan expiration = default)
        {
            await cache.SetAsync(key.ToString(), value, expiration);
        }

        public static async Task<bool> ContainsKeyAsync(this IDistributedCache cache, string key)
        {
            return (await cache.GetAsync(key)) != null;
        }
        
        public static async Task<bool> ContainsKeyAsync(this IDistributedCache cache, Guid key)
        {
            return await cache.ContainsKeyAsync(key.ToString());
        }
    }
}