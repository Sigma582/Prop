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

        protected override AccessorMethodsCollection CreateAccessorMethods(Type type)
        {
            var assemblyName = GetAssemblyName(type);

            var builder = new StringBuilder();

            foreach (var property in type.GetProperties(BindingFlags.GetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetGetMethod() != null)
                {
                    var getterCode = GenerateGetterCode(property);
                    builder.AppendLine(getterCode);
                }
            }

            foreach (var property in type.GetProperties(BindingFlags.SetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetSetMethod() != null)
                {
                    var setterCode = GenerateSetterCode(property);
                    builder.AppendLine(setterCode);
                }
            }

            Debug.WriteLine($"* RoslynDelegateFactory generated accessors code for type '{GetTypeName(type)}':\r\n{builder}");


            return default;
        }

        private string GetAssemblyName(Type type)
        {
            return $"{GetNamespaceName(type)}.dll";
        }

        private string GetNamespaceName(Type type)
        {
            return $"Sigma.Prop.Generated_{GetTypeName(type)}"
                .Replace('<', '_')
                .Replace('>', '_')
                .Replace("[]", "_Array");
        }

        private string GetConverterTypeName(Type type)
        {
            return $"Accessors";
        }

        private string GenerateGetterCode(PropertyInfo property)
        {
            return $"  public static {GetTypeName(property.PropertyType)} Get{property.Name}({GetTypeName(property.DeclaringType)} target)" +
                $"\r\n  {{" +
                $"\r\n    if (target == null) return default;" +
                $"\r\n    return target.{property.Name};" +
                $"\r\n  }}";
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
