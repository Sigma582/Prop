using System;
using System.Collections.Generic;

namespace Sigma.Prop
{
    public abstract class AccessorBase : IAccessor
    {
        //We only fill these dictionaries once and only read from them going forward.
        //So it's safe to use Dictionary and not ConcurrentDictionary even if the same instance is used by multiple threads.
        protected Dictionary<string, Func<object, object>> Getters { get; } = new Dictionary<string, Func<object, object>>();
        protected Dictionary<string, Action<object, object>> Setters { get; } = new Dictionary<string, Action<object, object>>();

        public Type TargetType { get; }
        public Implementation Implementation { get; }

        public object Get(object target, string propertyName)
        {
            if (!Getters.TryGetValue(propertyName, out Func<object, object> getter))
            {
                throw new NotSupportedException();
            }

            return getter(target);
        }

        public TProperty Get<TProperty>(object target, string propertyName)
        {
            var value = Get(target, propertyName);
            if (value is TProperty result)
            {
                return result;
            }
            return default;
        }

        public void Set(object target, string propertyName, object value)
        {
            if (!Setters.TryGetValue(propertyName, out Action<object, object> setter))
            {
                throw new NotSupportedException();
            }

            setter(target, value);
        }

        public AccessorBase(Type targetType, Implementation implementation)
        {
            Getters["Test"] = GetTest;
            TargetType = targetType;
            Implementation = implementation;
        }

        private object GetTest(object arg)
        {
            throw new NotImplementedException();
        }
    }
}
