using System;
using CacheManager.Core;

namespace JiaSyuanLibrary.Helper
{
    public class CacheHelper
    {
        protected class CacherInner<TCacheValueObject> : LazySingletonWrapBase<CacherInner<TCacheValueObject>>
        {
            private readonly ICacheManager<TCacheValueObject> cacheManger;

            /// <summary>
            /// CacheManager 的內部功能設置
            /// </summary>
            public CacherInner()
            {
                cacheManger = CacheFactory.Build<TCacheValueObject>(settings =>
                {
                    settings
                        .WithSystemRuntimeCacheHandle();
                });
            }

            /// <summary>
            /// 取得 Cache的實做操作封裝
            /// </summary>
            /// <param name="key"></param>
            /// <param name="dataAccessProvideer"></param>
            /// <param name="region"></param>
            /// <param name="expireTime"></param>
            /// <param name="offsetTime"></param>
            /// <returns></returns>
            public TCacheValueObject Get(string key, Func<TCacheValueObject> dataAccessProvideer, string region = default(string), TimeSpan expireTime = default(TimeSpan), TimeSpan offsetTime = default(TimeSpan))
            {
                TCacheValueObject result;
                if (region != default(string))
                {
                    result = cacheManger.Get<TCacheValueObject>(key, region);
                }
                else
                {
                    result = cacheManger.Get<TCacheValueObject>(key);
                }

                if (result == null)
                {
                    result = dataAccessProvideer();
                    if (result == null) { return result; }

                    cacheManger.Put(new CacheItem<TCacheValueObject>(key, result, ExpirationMode.Absolute, expireTime));
                }

                return result;
            }

            /// <summary>
            /// Update Cache Value
            /// </summary>
            /// <param name="key"></param>
            /// <param name="UpdateSource"></param>
            public void Update(string key, object UpdateSource)
            {
                cacheManger.AddOrUpdate(key, (TCacheValueObject)UpdateSource, v => (TCacheValueObject)UpdateSource);

            }
        }

        /// <summary>
        /// 從 Cache層取值, 若不存在就用提供的 Func<TCacheValueObject> 參數做 DataAccess
        /// </summary>
        /// <typeparam name="TCacheValueObject"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataAccessProvideer"></param>
        /// <param name="region"></param>
        /// <param name="expireTime"></param>
        /// <param name="offsetTime"></param>
        /// <returns></returns>
        public static TCacheValueObject Get<TCacheValueObject>(string key, Func<TCacheValueObject> dataAccessProvideer, string region = default(string), TimeSpan expireTime = default(TimeSpan), TimeSpan offsetTime = default(TimeSpan))
        {
            return CacherInner<TCacheValueObject>.Instance.Get(key, dataAccessProvideer, region, expireTime);
        }

        public static void Update<TCacheValueObject>(string key, TCacheValueObject UpdateSource)
        {
            CacherInner<TCacheValueObject>.Instance.Update(key, UpdateSource);
        }
    }

    public class LazySingletonWrapBase<T>
    {
        private static object _Instance;
        static readonly object padlock = new object(); //用來LOCK建立instance的程
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (padlock) //lock此區段程式碼，讓其它thread無法進入。
                    {
                        if (_Instance == null)
                        {
                            _Instance = Activator.CreateInstance(typeof(T));
                        }
                    }
                }
                return (T)_Instance;
            }

        }
    }
}
