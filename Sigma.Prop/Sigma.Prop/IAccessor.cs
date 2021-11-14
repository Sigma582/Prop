using System.Collections.Generic;
using System.Text;

namespace Sigma.Prop
{
    public interface IAccessor
    {
        object Get(object target, string property);
        TProperty Get<TProperty>(object target, string property);
        void Set(object target, string propertyName, object value);
    }
}
