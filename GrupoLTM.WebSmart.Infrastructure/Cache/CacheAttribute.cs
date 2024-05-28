using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Aspects;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace GrupoLTM.WebSmart.Infrastructure.Cache
{
    [Serializable]
    public sealed class CacheAttribute : OnMethodBoundaryAspect
    {
        private static ConnectionMultiplexer _redis;
        private static ConnectionMultiplexer _redisExtrato;
        private string _methodName;
        private static JsonSerializerSettings _jsonSettings = GetJsonSettings();
        private int timeSpan;

        public CacheAttribute(int time = 5)
        {
            timeSpan = time;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["Cache.Enabled"]) == true)
                    return;

                var keyCache = GenerateKey(args.Instance, args.Arguments);

                OpenConnection();

                var db = _redis.GetDatabase();
                var result = db.StringGet(keyCache);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    var obj = JsonConvert.DeserializeObject(result, ((MethodInfo)(args.Method)).ReturnType);
                    args.ReturnValue = obj;
                    args.FlowBehavior = FlowBehavior.Return;
                }
                else
                {
                    args.MethodExecutionTag = keyCache;
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["Cache.Enabled"]) == true)
                    return;

                var keyCache = GenerateKey(args.Instance, args.Arguments);

                OpenConnection();
                var db = _redis.GetDatabase();

                var obj = JsonConvert.SerializeObject(args.ReturnValue, _jsonSettings);

                db.StringSet(keyCache, obj, TimeSpan.FromMinutes(timeSpan));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            this._methodName = method.DeclaringType.FullName + "." + method.Name;
        }

        private string GenerateKey(object instance, Arguments arguments)
        {
            var campaign = ConfigurationManager.AppSettings["Campaing"];

            if (instance == null && arguments.Count == 0)
                return campaign + "." + this._methodName;

            var stringBuilder = new StringBuilder(_methodName);

            stringBuilder.Append('(');

            if (instance != null)
            {
                stringBuilder.Append(instance);
                stringBuilder.Append("; ");
            }

            for (int i = 0; i < arguments.Count; i++)
            {
                stringBuilder.Append(arguments.GetArgument(i) ?? "null");
                stringBuilder.Append(", ");
            }

            return string.Format("{0}.{1}", campaign, stringBuilder.ToString());

        }

        private static void OpenConnection(bool verifyEnabled = true)
        {
            if (verifyEnabled && !Convert.ToBoolean(ConfigurationManager.AppSettings["Cache.Enabled"]))
                return;

            if (_redis != null && _redis.IsConnected)
                return;

            var server = ConfigurationManager.AppSettings["Cache.Server"];
            var password = ConfigurationManager.AppSettings["Cache.Pwd"];

            var config = new ConfigurationOptions
            {
                Password = password,
                AbortOnConnectFail = false,
                AllowAdmin = true,
                ConnectTimeout = 10000,
                ResponseTimeout = 10000,
                ConnectRetry = 5,
                SyncTimeout = 3000
            };

            config.EndPoints.Add(server);

            _redis = ConnectionMultiplexer.Connect(config);
        }

        private static JsonSerializerSettings GetJsonSettings()
        {
            var resolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
            {
                DefaultMembersSearchFlags =
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance
            };

            var settings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = resolver
            };

            return settings;
        }

        public T GetHashObjectExtrato<T>(string key)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["CacheExtrato.Enabled"]))
                    return default(T);

                string json = GetHashRedisExtrato(key);

                return ConvertObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        public void SetHashsObjectsExtrato(string key, Dictionary<string, string> hashValues)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["CacheExtrato.Enabled"]))
                    return;

                var db = _redisExtrato.GetDatabase(Convert.ToInt32(ConfigurationManager.AppSettings["CacheExtrato.Base"]));

                var keys = hashValues.Select(x => new HashEntry(x.Key, x.Value));
                db.HashSetAsync(key, keys.ToArray());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public void StoreObjectExtrato(string key, object value, string CacheName)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["CacheExtrato.Enabled"]))
                    return;

                OpenConnectionExtrato();

                TimeSpan expiry = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings[CacheName + ".TimeSpanMinutos"]));

                if (value == null)
                    return;

                var db = _redisExtrato.GetDatabase(Convert.ToInt32(ConfigurationManager.AppSettings["CacheExtrato.Base"]));
                var obj = JsonConvert.SerializeObject(value);
                db.StringSet(key, obj, expiry, flags: CommandFlags.FireAndForget);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private T ConvertObject<T>(string result)
        {
            if (string.IsNullOrEmpty(result))
                return default(T);

            return JsonConvert.DeserializeObject<T>(result, _jsonSettings);
        }

        private string GetHashRedisExtrato(string key)
        {
            try
            {
                OpenConnectionExtrato();

                var db = _redisExtrato.GetDatabase(Convert.ToInt32(ConfigurationManager.AppSettings["CacheExtrato.Base"]));
                var retorno = db.StringGet(key, flags: CommandFlags.FireAndForget);

                return retorno.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        private string GenerateKeyExtrato(object instance, Arguments arguments)
        {
            var campaign = ConfigurationManager.AppSettings["CampaingExtrato"];

            if (instance == null && arguments.Count == 0)
                return campaign + "." + this._methodName;

            StringBuilder stringBuilder = new StringBuilder(this._methodName);
            stringBuilder.Append('(');
            if (instance != null)
            {
                stringBuilder.Append(instance);
                stringBuilder.Append("; ");
            }

            for (int i = 0; i < arguments.Count; i++)
            {
                stringBuilder.Append(arguments.GetArgument(i) ?? "null");
                stringBuilder.Append(", ");
            }

            return string.Format("{0}.{1}", campaign, stringBuilder.ToString());

        }

        private static void OpenConnectionExtrato()
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["CacheExtrato.Enabled"]))
                return;

            if (_redisExtrato != null && _redisExtrato.IsConnected)
                return;

            var server = ConfigurationManager.AppSettings["CacheExtrato.Server"];
            var password = ConfigurationManager.AppSettings["CacheExtrato.Pwd"];
            var ssl = ConfigurationManager.AppSettings["CacheExtrato.Ssl"];

            var config = new ConfigurationOptions
            {
                Password = password,
                AbortOnConnectFail = false,
                AllowAdmin = true,
                ConnectTimeout = 1000,
                ResponseTimeout = 1000,
                ConnectRetry = 5,
                SyncTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CacheExtrato.SyncTimeout"]),
                Ssl = Convert.ToBoolean(ssl)
            };

            config.EndPoints.Add(server);
            _redisExtrato = ConnectionMultiplexer.Connect(config);
        }

        public void KeyDeleteAsync(string key)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["CacheExtrato.Enabled"]))
                    return;
                OpenConnectionExtrato();
                var db = _redisExtrato.GetDatabase(Convert.ToInt32(ConfigurationManager.AppSettings["CacheExtrato.Base"]));
                db.KeyDelete(key);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}