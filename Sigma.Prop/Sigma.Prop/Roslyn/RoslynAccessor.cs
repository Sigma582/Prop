using System;

namespace Sigma.Prop.Roslyn
{
    public class RoslynAccessor : IAccessor
    {
        public object Get(object target, string propertyName)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            Func<object, object> getter = GetGetter(target.GetType(), propertyName);
            return getter(target);
        }

        public TProperty Get<TProperty>(object target, string propertyName)
        {
            //todo: consider typed getters

            var value = Get(target, propertyName);
            if(value is TProperty result)
            {
                return result;
            }
            return default;
        }

        public void Set(object target, string propertyName, object value)
        {

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            Action<object, object> setter = GetSetter(target.GetType(), propertyName);
            setter(target, value);
        }

        private Func<object, object> GetGetter(Type type, string propertyName)
        {
            return RoslynDelegateFactory.Instance.GetGetter(type, propertyName);
        }

        private Action<object, object> GetSetter(Type type, string propertyName)
        {
            return RoslynDelegateFactory.Instance.GetSetter(type, propertyName);
        }
    }
}
