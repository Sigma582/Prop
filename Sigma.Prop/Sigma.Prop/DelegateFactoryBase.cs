using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Sigma.Prop
{
    public abstract class DelegateFactoryBase
    {
        private readonly ConcurrentDictionary<Type, Lazy<AccessorMethodsCollection>> _accessorMethodsCache = new ConcurrentDictionary<Type, Lazy<AccessorMethodsCollection>>();
        
        public Func<object, object> GetGetter(Type type, string propertyName)
        {
            var collection = GetAccessorMethods(type);

            if(collection.Getters.TryGetValue(propertyName, out Func<object, object> getter))
            {
                return getter;
            }

            throw new KeyNotFoundException($"Getter for property '{propertyName}' of type '{type.FullName}' was not found.");
        }

        public Action<object, object> GetSetter(Type type, string propertyName)
        {
            var collection = GetAccessorMethods(type);

            if(collection.Setters.TryGetValue(propertyName, out Action<object, object> setter))
            {
                return setter;
            }

            throw new KeyNotFoundException($"Setter for property '{propertyName}' of type '{type.FullName}' was not found.");
        }

        private AccessorMethodsCollection GetAccessorMethods(Type type)
        {
            var exists = _accessorMethodsCache.TryGetValue(type, out Lazy<AccessorMethodsCollection> lazy);

            if (!exists)
            {
                lazy = new Lazy<AccessorMethodsCollection>(() => CreateAccessorMethods(type));
                lazy = _accessorMethodsCache.GetOrAdd(type, lazy);
            }

            return lazy.Value;
        }

        protected abstract AccessorMethodsCollection CreateAccessorMethods(Type type);
    }
}
