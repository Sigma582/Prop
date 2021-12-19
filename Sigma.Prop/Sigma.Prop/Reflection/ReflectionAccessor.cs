using System;
using System.Collections;
using System.Collections.Generic;

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

        public IEnumerable Get(IEnumerable targets, string propertyName)
        {
            foreach (var target in targets)
            {
                var property = target.GetType().GetProperty(propertyName);
                var value = property.GetValue(target);

                yield return value;
            }
        }

        public IEnumerable<TProperty> Get<TProperty>(IEnumerable targets, string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Set(IEnumerable targets, string propertyName, IEnumerable values)
        {
            throw new NotImplementedException();
        }
    }
}
