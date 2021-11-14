using System;

namespace Sigma.Prop.Extension
{
    public static class AccessorExtension
    {
        public static object Get(this object target, string property)
        {
            var accessor = AccessorFactory.Instance.Get(target.GetType());
            return accessor.Get(target, property);
        }

        public static T Get<T>(this object target, string property)
        {
            var accessor = AccessorFactory.Instance.Get(target.GetType());
            return accessor.Get<T>(target, property);
        }

        public static void Set(this object target, string property, object value)
        {
            var accessor = AccessorFactory.Instance.Get(target.GetType());
            accessor.Set(target, property, value);
        }
    }
}
