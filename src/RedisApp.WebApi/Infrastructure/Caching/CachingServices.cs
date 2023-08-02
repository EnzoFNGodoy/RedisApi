using Microsoft.Extensions.Caching.Distributed;

namespace RedisApp.WebApi.Infrastructure.Caching;

public sealed class CachingServices : ICachingServices
{
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _options;

    public CachingServices(IDistributedCache cache)
    {
        _cache = cache;
        _options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600), // 1 hour
            SlidingExpiration = TimeSpan.FromSeconds(1200) // 20 min
        };
    }

    public async Task<string> GetAsync(string key)
        => await _cache.GetStringAsync(key) ?? string.Empty;

    public async Task SetAsync(string key, string value) 
        => await _cache.SetStringAsync(key, value);

    public async Task RemoveAsync(string key)
        => await _cache.RemoveAsync(key);
}