using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Sigma.Prop
{
    internal abstract class DelegateFactoryBase
    {
        private readonly ConcurrentDictionary<Type, Lazy<ConcurrentDictionary<string, Func<object, object>>>> _gettersCache = new ConcurrentDictionary<Type, Lazy<ConcurrentDictionary<string, Func<object, object>>>>();
        private readonly ConcurrentDictionary<Type, Lazy<ConcurrentDictionary<string, Action<object, object>>>> _settersCache = new ConcurrentDictionary<Type, Lazy<ConcurrentDictionary<string, Action<object, object>>>>();
        
        public Func<object, object> GetGetter(Type type, string propertyName)
        {
            var getters = GetGetters(type);

            if(getters.TryGetValue(propertyName, out Func<object, object> getter))
            {
                return getter;
            }

            throw new KeyNotFoundException($"Getter for property '{propertyName}' of type '{type.FullName}' was not found.");
        }

        public Action<object, object> GetSetter(Type type, string propertyName)
        {
            var setters = GetSetters(type);

            if(setters.TryGetValue(propertyName, out Action<object, object> setter))
            {
                return setter;
            }

            throw new KeyNotFoundException($"Setter for property '{propertyName}' of type '{type.FullName}' was not found.");
        }

        private ConcurrentDictionary<string, Func<object, object>> GetGetters(Type type)
        {
            var gettersExist = _gettersCache.TryGetValue(type, out Lazy<ConcurrentDictionary<string, Func<object, object>>> getterLazy);

            if (!gettersExist)
            {
                getterLazy = new Lazy<ConcurrentDictionary<string, Func<object, object>>>(() => CreateGetters(type));
                getterLazy = _gettersCache.GetOrAdd(type, getterLazy);
            }

            return getterLazy.Value;
        }

        private ConcurrentDictionary<string, Action<object, object>> GetSetters(Type type)
        {
            var gettersExist = _settersCache.TryGetValue(type, out Lazy<ConcurrentDictionary<string, Action<object, object>>> setterLazy);

            if (!gettersExist)
            {
                setterLazy = new Lazy<ConcurrentDictionary<string, Action<object, object>>>(() => CreateSetters(type));
                setterLazy = _settersCache.GetOrAdd(type, setterLazy);
            }

            return setterLazy.Value;
        }

        protected abstract ConcurrentDictionary<string, Func<object, object>> CreateGetters(Type type);

        protected abstract ConcurrentDictionary<string, Action<object, object>> CreateSetters(Type type);
    }
}
