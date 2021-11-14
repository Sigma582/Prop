using System;

namespace Sigma.Prop.Reflection
{
    /// <summary>
    /// <see cref="IAccessor"/> implementation using <see cref="System.Reflection.PropertyInfo"/> methods. This is the slowest implementation and is present solely for comparison with others. Never use this in production.
    /// </summary>
    public class ReflectionAccessor : IAccessor
    {
        public object Get(object target, string propertyName)
        {
            if(target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            
            if(propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var property = target.GetType().GetProperty(propertyName);
            var value = property.GetValue(target);

            return value;
        }

        public TProperty Get<TProperty>(object target, string propertyName)
        {
            var value = Get(target, propertyName);
            return (TProperty)value;
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

            var property = target.GetType().GetProperty(propertyName);
            property.SetValue(target, value);
        }
    }
}
