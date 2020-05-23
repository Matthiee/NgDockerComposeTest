using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan ttl);
        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
