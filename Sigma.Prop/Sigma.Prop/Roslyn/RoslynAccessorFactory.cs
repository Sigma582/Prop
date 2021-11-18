using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sigma.Prop.Roslyn
{
    public class RoslynAccessorFactory
    {
        private string BaseClassName = "AccessorBase";

        public IAccessor CreateAccessor(Type targetType)
        {
            var (code, referencedTypes) = GenerateCode(targetType);

            var assemblyName = GetAssemblyName(targetType);
            var tree = CSharpSyntaxTree.ParseText(code);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var compilation = CSharpCompilation.Create(assemblyName
                , syntaxTrees: new[] { tree }
                ////* Loading assemblies based on referenced types doesn't work because dotnetstandard assembly is not loaded.
                ////* So at this point we resort to referencing all assemblies currently loaded, assuming that they contain everything needed.
                ////* todo: see if we can reduce number of references while including all the necessary ones
                //, references: referencedTypes
                //    .Select(type => type.Assembly)
                //    .Distinct()
                //    .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
                , references: assemblies
                    .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
                , options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            
            using (var stream = new MemoryStream())
            {
                var result = compilation.Emit(stream);

                if (!result.Success)
                {
                    //todo: provide details
                    throw new ApplicationException($"Could not generate accessor methods for class {GetTypeName(targetType)}");
                }
                
                stream.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(stream.ToArray());
                var accessorType = assembly.GetType($"{GetNamespaceName(targetType)}.{GetAccessorTypeName(targetType)}");
                var accessor = Activator.CreateInstance(accessorType, targetType, Implementation.Roslyn, code);
                return (IAccessor)accessor;
            }
        }

        private Tuple<string, List<Type>> GenerateCode(Type targetType)
        {
            var referencedTypes = new List<Type> { typeof(object), typeof(AccessorBase), targetType };

            var methodsBuilder = new StringBuilder();
            var ctorBuilder = new StringBuilder();

            foreach (var property in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                AddReferencedTypes(referencedTypes, property.PropertyType);

                if (property.GetGetMethod() != null)
                {
                    var getterCode = GenerateGetterCode(property);
                    methodsBuilder.AppendLine(getterCode);

                    var ctorCode = $"            Getters[\"{property.Name}\"] = Get{property.Name};";
                    ctorBuilder.AppendLine(ctorCode);
                }
            
                if (property.GetSetMethod() != null)
                {
                    var setterCode = GenerateSetterCode(property);
                    methodsBuilder.AppendLine(setterCode);

                    var ctorCode = $"            Setters[\"{property.Name}\"] = Set{property.Name};";
                    ctorBuilder.AppendLine(ctorCode);
                }
            }

            Debug.WriteLine($"* RoslynDelegateFactory generated accessors code for type '{GetTypeName(targetType)}':\r\n{methodsBuilder}");

            var code = $@"namespace {GetNamespaceName(targetType)} " 
                + $"\r\n{{" 
                + $"\r\n    public class {GetAccessorTypeName(targetType)} : {BaseClassName}" 
                + $"\r\n    {{" 
                + $"\r\n        public {typeof(string).FullName} SourceCode {{ get; }}" 
                + $"\r\n" 
                + $"\r\n        public {GetAccessorTypeName(targetType)}({typeof(Type).FullName} targetType, {typeof(Implementation).FullName} implementation, {typeof(string).FullName} sourceCode)" 
                + $"\r\n            : base(targetType, implementation)" 
                + $"\r\n        {{" 
                + $"\r\n            SourceCode = sourceCode;" 
                + $"\r\n{ctorBuilder}" 
                + $"\r\n        }}" 
                + $"\r\n" 
                + $"\r\n{methodsBuilder}" 
                + $"\r\n    }}" 
                + $"\r\n}}";

            return new Tuple<string, List<Type>>(code, referencedTypes);
        }

        private static void AddReferencedTypes(List<Type> referencedTypes, Type type)
        {
            if (!referencedTypes.Contains(type))
            {
                referencedTypes.Add(type);
            }

            if (type.IsGenericType)
            {
                foreach (var genericType in type.GetGenericArguments())
                {
                    AddReferencedTypes(referencedTypes, genericType);
                }
            }
        }

        private string GetAssemblyName(Type targetType)
        {
            return $"{GetNamespaceName(targetType)}.dll";
        }

        private string GetNamespaceName(Type targetType)
        {
            return $"Sigma.Prop.Generated.{GetShortTypeName(targetType)}"
                .Replace('<', '_')
                .Replace('>', '_')
                .Replace("[]", "_Array");
        }

        private string GetAccessorTypeName(Type targetType)
        {
            return $"RoslynAccessor_{GetShortTypeName(targetType)}_{Math.Abs(targetType.FullName.GetHashCode())}";
        }

        private string GenerateGetterCode(PropertyInfo property)
        {
            return  $"        public static object Get{property.Name}(object target)" +
                $"\r\n        {{" +
                $"\r\n            if (target is {GetTypeName(property.DeclaringType)})" +
                $"\r\n            return (({GetTypeName(property.DeclaringType)})target).{property.Name};" +
                $"\r\n            return default;" +
                $"\r\n        }}";
        }

        private string GenerateSetterCode(PropertyInfo property)
        {
            return  $"        public static void Set{property.Name}(object target, object value)" +
                $"\r\n        {{" +
                $"\r\n            if (target is {GetTypeName(property.DeclaringType)}" +
                $"\r\n                && value is {GetTypeName(property.PropertyType)})" +
                $"\r\n            (({GetTypeName(property.DeclaringType)}) target).{property.Name} = ({GetTypeName(property.PropertyType)}) value;" +
                $"\r\n        }}";
        }

        private string GetTypeName(Type type)
        {
            if (type.IsArray)
            {
            //"System.Int32[]"
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

        private string GetShortTypeName(Type type)
        {
            if (type.IsArray)
            {
                return $"{GetShortTypeName(type.GetElementType())}_Array";
            }

            if (type.IsGenericType)
            {
                var ownName = type.Name.Remove(type.FullName.IndexOf('`'));
                return $"{ownName}_{string.Join("_", type.GenericTypeArguments.Select(t => GetTypeName(t)))}";
            }

            return type.Name;
        }
    }
}
