namespace RedisApp.WebApi.Infrastructure.Caching;

public interface ICachingServices
{
    Task SetAsync(string key, string value);
    Task<string> GetAsync(string key);
    Task RemoveAsync(string key);
}