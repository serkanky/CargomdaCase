using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Redis.Cache
{
    public class RedisService
    {
        private readonly IDatabase _cache;

        public RedisService(IDatabase cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            var serializedData = _cache.StringGet(key);
            if (!serializedData.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<T>(serializedData);
            }

            return default;
        }

        public void Set<T>(string key, T data)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            _cache.StringSet(key, serializedData);
        }

        public void Remove(string key)
        {
            _cache.KeyDelete(key);
        }
    }
}

