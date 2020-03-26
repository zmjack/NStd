﻿using NStandard.Caching;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NStandard
{
    public class CacheContainer<TKey, TValue> : Dictionary<TKey, Cache<TValue>>
    {
        public Func<TKey, CacheDelegate<TValue>> CacheMethod;
        public UpdateCacheExpirationDelegate UpdateExpirationMethod;

        public new Cache<TValue> this[TKey key]
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                if (!ContainsKey(key))
                {
                    base[key] = new Cache<TValue>
                    {
                        CacheMethod = CacheMethod?.Invoke(key),
                        UpdateExpirationMethod = UpdateExpirationMethod,
                    };
                }
                return base[key];
            }
        }

    }
}
