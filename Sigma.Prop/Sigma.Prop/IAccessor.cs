namespace Sigma.Prop
{
    public interface IAccessor
    {
        object Get(object target, string propertyName);
        TProperty Get<TProperty>(object target, string propertyName);
        void Set(object target, string propertyName, object value);
    }
}
