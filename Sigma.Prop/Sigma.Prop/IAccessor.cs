using System.Collections;
using System.Collections.Generic;

namespace Sigma.Prop
{
    public interface IAccessor
    {
        object Get(object target, string propertyName);
        TProperty Get<TProperty>(object target, string propertyName);
        void Set(object target, string propertyName, object value);

        IEnumerable Get(IEnumerable targets, string propertyName);
        IEnumerable<TProperty> Get<TProperty>(IEnumerable targets, string propertyName);
        void Set(IEnumerable targets, string propertyName, IEnumerable values);
    }
}
