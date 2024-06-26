﻿using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ShredStorePresentation.Extensions.Cache
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
           string recordId,
           T data,
           TimeSpan? absoluteExpireTime = null,
           TimeSpan? unusedExpiredTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(15);
            options.SlidingExpiration = unusedExpiredTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            try
            {

                var jsonData = await cache.GetStringAsync(recordId);

                if (jsonData is null)
                {
                    return default;
                }

                return JsonSerializer.Deserialize<T>(jsonData);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static async Task DeleteRecordsAsync(this IDistributedCache cache, string recordId)
        {
            try
            {
                var jsonData = await cache.GetStringAsync(recordId);

                if (jsonData is not null)
                {
                    await cache.DeleteRecordsAsync(recordId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
