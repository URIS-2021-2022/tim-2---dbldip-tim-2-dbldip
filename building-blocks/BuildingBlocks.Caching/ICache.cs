using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Caching
{
    public interface ICache
    {
        
        void Add(object objectToCache, string key);

        void Add<T>(object objectToCache, string key);

        void Add<T>(object objectToCache, string key, double cacheDuration);

        T Get<T>(string key);

        object Get(string key);        

        void Remove(string key);

        void ClearAll();

        bool Exists(string key);

        public virtual TResponse FromCacheOrService<TResponse>(Func<TResponse> action, string key)
        {
            var cached = Get(key);
            if (cached == null)
            {
                cached = action();
                Add(cached, key);
            }
            return (TResponse)cached;
        }

        public virtual TResponse FromCacheOrService<TResponse>(Func<TResponse> action, string key, double cacheDuration)
        {
            var cached = Get(key);
            if (cached == null)
            {
                cached = action();
                Add<TResponse>(cached, key, cacheDuration);
            }
            return (TResponse)cached;
        }

        public async Task<TResponse> FromCacheOrServiceAsync<TResponse>(Func<Task<TResponse>> action, string key, double cacheDuration)
        {
            var cached = Get(key);
            if (cached == null)
            {
                cached = await action();
                Add<TResponse>(cached, key, cacheDuration);
            }
            return (TResponse)cached;
        }

        public async Task<TResponse> FromCacheOrServiceAsync<TResponse>(Func<Task<TResponse>> action, string key)
        {
            var cached = Get(key);
            if (cached == null)
            {
                cached = await action();
                Add(cached, key);
            }
            return (TResponse)cached;
        }
    }
}