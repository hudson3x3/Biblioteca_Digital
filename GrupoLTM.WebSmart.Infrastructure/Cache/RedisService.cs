using System;
using System.Configuration;
using StackExchange.Redis;

namespace GrupoLTM.WebSmart.Infrastructure.Cache
{
    public class RedisService
    {
        private ConnectionMultiplexer _redis;

        private void OpenConnection()
        {
            if (_redis != null && _redis.IsConnected)
                return;

            var server = ConfigurationManager.AppSettings["Cache.Server"];
            var password = ConfigurationManager.AppSettings["Cache.Pwd"];
            var ssl = ConfigurationManager.AppSettings["Cache.Ssl"];

            var config = new ConfigurationOptions
            {
                Password = password,
                AbortOnConnectFail = false,
                ConnectTimeout = 5000,
                ResponseTimeout = 5000,
                ConnectRetry = 5,
                Ssl = Convert.ToBoolean(ssl)
            };

            config.EndPoints.Add(server);

            _redis = ConnectionMultiplexer.Connect(config);

            if (!_redis.IsConnected)
                throw new Exception("Redis não conectado");
        }

        public string GetCache(string key)
        {
            OpenConnection();

            var cache = _redis.GetDatabase();

            var value = cache.StringGet(key);

            return value.ToString();
        }

        public void SetCache(string key, string value, int seconds)
        {
            OpenConnection();

            var cache = _redis.GetDatabase();

            cache.StringSet(key, value, TimeSpan.FromSeconds(seconds));
        }
    }
}
