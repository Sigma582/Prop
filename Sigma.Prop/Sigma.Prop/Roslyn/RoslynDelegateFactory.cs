using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sigma.Prop.Roslyn
{
    public class RoslynDelegateFactory : DelegateFactoryBase
    {
        #region singleton
        private static readonly Lazy<RoslynDelegateFactory> _lazy = new Lazy<RoslynDelegateFactory>(() => new RoslynDelegateFactory());

        public static RoslynDelegateFactory Instance => _lazy.Value;

        private RoslynDelegateFactory()
        {

        }
        #endregion

        protected override ConcurrentDictionary<string, Func<object, object>> CreateGetters(Type type)
        {
            var assemblyName = $"Sigma.Prop.{GetTypeName(type)}.Getters.dll";

            var builder = new StringBuilder();

            foreach (var property in type.GetProperties(BindingFlags.GetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetGetMethod() == null) continue;

                var getterCode = GenerateGetterCode(property);
                builder.AppendLine(getterCode);
            }

            Debug.WriteLine($"* RoslynDelegateFactory generated code for getters of type '{GetTypeName(type)}'\r\n{builder}");


            return new ConcurrentDictionary<string, Func<object, object>>();
        }

        private string GenerateGetterCode(PropertyInfo property)
        {
            return $"  public static {GetTypeName(property.PropertyType)} Get{property.Name}({GetTypeName(property.DeclaringType)} target)" +
                $"\r\n  {{" +
                $"\r\n    if (target == null) return default;" +
                $"\r\n    return target.{property.Name};" +
                $"\r\n  }}";
        }

        protected override ConcurrentDictionary<string, Action<object, object>> CreateSetters(Type type)
        {
            var assemblyName = $"Sigma.Prop.{GetTypeName(type)}.Setters.dll";

            var builder = new StringBuilder();

            foreach (var property in type.GetProperties(BindingFlags.SetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetSetMethod() == null) continue;

                var setterCode = GenerateSetterCode(property);
                builder.AppendLine(setterCode);
            }

            Debug.WriteLine($"* RoslynDelegateFactory generated code for setters of type '{GetTypeName(type)}'\r\n{builder}");


            return new ConcurrentDictionary<string, Action<object, object>>();
        }

        private string GenerateSetterCode(PropertyInfo property)
        {
            return $"  public static void Set{property.Name}({GetTypeName(property.DeclaringType)} target, {GetTypeName(property.PropertyType)} value)" +
                $"\r\n  {{" +
                $"\r\n    if (target == null) return;" +
                $"\r\n    target.{property.Name} = value;" +
                $"\r\n  }}";
        }

        private string GetTypeName(Type type)
        {
            //"System.Int32[]"
            if (type.IsArray)
            {
                return $"{GetTypeName(type.GetElementType())}[]";
            }

            //"System.Nullable`1[[System.Decimal, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
            if (type.IsGenericType)
            {
                //"System.Nullable"
                var ownName = type.FullName.Remove(type.FullName.IndexOf('`'));
                //System.Nullable<System.Decimal>
                return $"{ownName}<{string.Join(",", type.GenericTypeArguments.Select(t => GetTypeName(t)))}>";
            }

            return type.FullName;
        }

    }
}
