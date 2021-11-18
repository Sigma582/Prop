using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sigma.Prop.Workbench
{
    //generated via RoslynAccessorFactory and copied here
    public class RoslynAccessor : AccessorBase
    {
        public System.String SourceCode { get; }

        public RoslynAccessor(System.Type targetType, Sigma.Prop.Implementation implementation, System.String sourceCode)
            : base(targetType, implementation)
        {
            SourceCode = sourceCode;
            Getters["Item"] = GetItem;
            Setters["Item"] = SetItem;
            Getters["Id"] = GetId;
            Getters["Text"] = GetText;
            Setters["Text"] = SetText;
            Getters["Value"] = GetValue;
            Setters["Value"] = SetValue;
            Getters["Comments"] = GetComments;
            Setters["Comments"] = SetComments;
            Getters["Tidbits"] = GetTidbits;
            Setters["Tidbits"] = SetTidbits;
            Getters["SomethingComplex"] = GetSomethingComplex;
            Setters["SomethingComplex"] = SetSomethingComplex;

        }

        public static object GetItem(object target)
        {
                return ((Sigma.Prop.Workbench.TestClass)target).Item;
        }
        public static void SetItem(object target, object value)
        {
                ((Sigma.Prop.Workbench.TestClass)target).Item = (Sigma.Prop.Workbench.ItemBase)value;
        }
        public static object GetId(object target)
        {
                return ((Sigma.Prop.Workbench.TestClassBase)target).Id;
        }
        public static object GetText(object target)
        {
                return ((Sigma.Prop.Workbench.TestClassBase)target).Text;
        }
        public static void SetText(object target, object value)
        {
                ((Sigma.Prop.Workbench.TestClassBase)target).Text = (System.String)value;
        }
        public static object GetValue(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).Value;
        }
        public static void SetValue(object target, object value)
        {
                ((Sigma.Prop.Workbench.TestClassBase)target).Value = (System.Nullable<System.Decimal>)value;
        }
        public static object GetComments(object target)
        {
                return ((Sigma.Prop.Workbench.TestClassBase)target).Comments;
        }
        public static void SetComments(object target, object value)
        {
                ((Sigma.Prop.Workbench.TestClassBase)target).Comments = (System.Collections.Generic.List<System.String>)value;
        }
        public static object GetTidbits(object target)
        {
                return ((Sigma.Prop.Workbench.TestClassBase)target).Tidbits;
        }
        public static void SetTidbits(object target, object value)
        {
                ((Sigma.Prop.Workbench.TestClassBase)target).Tidbits = (System.Byte[])value;
        }
        public static object GetSomethingComplex(object target)
        {
                return ((Sigma.Prop.Workbench.TestClassBase)target).SomethingComplex;
        }
        public static void SetSomethingComplex(object target, object value)
        {
                ((Sigma.Prop.Workbench.TestClassBase)target).SomethingComplex = (System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<Sigma.Prop.Workbench.TestClass[]>>[])value;
        }

        public static object GetItem2(object target)
        {
            return ((Sigma.Prop.Workbench.TestClass)target).Item;
        }

        public static object GetItem3(TestClass target)
        {
            return target.Item;
        }
    }

    public class RoslynAccessor_Switch : IAccessor
    {
        public System.String SourceCode { get; }

        public RoslynAccessor_Switch(System.Type targetType, Sigma.Prop.Implementation implementation, System.String sourceCode)
        {
            SourceCode = sourceCode;

        }

        public static object GetItem(object target)
        {
            if (target is Sigma.Prop.Workbench.TestClass)
                return ((Sigma.Prop.Workbench.TestClass)target).Item;
            return default;
        }

        public static void SetItem(object target, object value)
        {
            if (target is Sigma.Prop.Workbench.TestClass
                && value is Sigma.Prop.Workbench.ItemBase)
                ((Sigma.Prop.Workbench.TestClass)target).Item = (Sigma.Prop.Workbench.ItemBase)value;
        }
        public static object GetId(object target)
        {
                return ((Sigma.Prop.Workbench.TestClassBase)target).Id;
        }
        public static object GetText(object target)
        {
                return ((Sigma.Prop.Workbench.TestClassBase)target).Text;
        }
        public static void SetText(object target, object value)
        {
                ((Sigma.Prop.Workbench.TestClassBase)target).Text = (System.String)value;
        }
        public static object GetValue(object target)
        {
                return ((Sigma.Prop.Workbench.TestClassBase)target).Value;
        }
        public static void SetValue(object target, object value)
        {
                ((Sigma.Prop.Workbench.TestClassBase)target).Value = (System.Nullable<System.Decimal>)value;
        }
        public static object GetComments(object target)
        {
                return ((Sigma.Prop.Workbench.TestClassBase)target).Comments;
        }
        public static void SetComments(object target, object value)
        {
                ((Sigma.Prop.Workbench.TestClassBase)target).Comments = (System.Collections.Generic.List<System.String>)value;
        }
        public static object GetTidbits(object target)
        {
                return ((Sigma.Prop.Workbench.TestClassBase)target).Tidbits;
        }
        public static void SetTidbits(object target, object value)
        {
                ((Sigma.Prop.Workbench.TestClassBase)target).Tidbits = (System.Byte[])value;
        }
        public static object GetSomethingComplex(object target)
        {
                return ((Sigma.Prop.Workbench.TestClassBase)target).SomethingComplex;
        }
        public static void SetSomethingComplex(object target, object value)
        {
                ((Sigma.Prop.Workbench.TestClassBase)target).SomethingComplex = (System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<Sigma.Prop.Workbench.TestClass[]>>[])value;
        }

        public object Get(object target, string propertyName)
        {
            switch (propertyName)
            {
                case "Item": return GetItem(target);
                case "Id": return GetId(target);
                case "Text": return GetText(target);
                case "Value": return GetValue(target);
                case "Comments": return GetComments(target);
                case "Tidbits": return GetTidbits(target);
                case "SomethingComplex": return GetSomethingComplex(target);
                default: return default;
            }
        }

        public TProperty Get<TProperty>(object target, string propertyName)
        {
            var value = Get(target, propertyName);
            if (value is TProperty result)
            {
                return result;
            }
            return default;
        }

        public void Set(object target, string propertyName, object value)
        {
            throw new System.NotImplementedException();
        }
    }

    public class RoslynAccessor_If : IAccessor
    {
        public System.String SourceCode { get; }

        public RoslynAccessor_If(System.Type targetType, Sigma.Prop.Implementation implementation, System.String sourceCode)
        {
            SourceCode = sourceCode;

        }

        public static object GetItem(object target)
        {
            return ((Sigma.Prop.Workbench.TestClass)target).Item;
        }
        public static void SetItem(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClass)target).Item = (Sigma.Prop.Workbench.ItemBase)value;
        }
        public static object GetId(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).Id;
        }
        public static object GetText(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).Text;
        }
        public static void SetText(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClassBase)target).Text = (System.String)value;
        }
        public static object GetValue(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).Value;
        }
        public static void SetValue(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClassBase)target).Value = (System.Nullable<System.Decimal>)value;
        }
        public static object GetComments(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).Comments;
        }
        public static void SetComments(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClassBase)target).Comments = (System.Collections.Generic.List<System.String>)value;
        }
        public static object GetTidbits(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).Tidbits;
        }
        public static void SetTidbits(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClassBase)target).Tidbits = (System.Byte[])value;
        }
        public static object GetSomethingComplex(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).SomethingComplex;
        }
        public static void SetSomethingComplex(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClassBase)target).SomethingComplex = (System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<Sigma.Prop.Workbench.TestClass[]>>[])value;
        }

        public object Get(object target, string propertyName)
        {
            //if (target is Sigma.Prop.Workbench.TestClassBase)
            {
                if (propertyName == "Id") return ((Sigma.Prop.Workbench.TestClass)target).Id;
                if (propertyName == "Text") return ((Sigma.Prop.Workbench.TestClass)target).Text;
                if (propertyName == "Value") return ((Sigma.Prop.Workbench.TestClass)target).Value;
                if (propertyName == "Item") return ((Sigma.Prop.Workbench.TestClass)target).Item;
                if (propertyName == "Comments") return ((Sigma.Prop.Workbench.TestClass)target).Comments;
                if (propertyName == "Tidbits") return ((Sigma.Prop.Workbench.TestClass)target).Tidbits;
                if (propertyName == "SomethingComplex") return ((Sigma.Prop.Workbench.TestClass)target).SomethingComplex;
            }
            return default;
        }

        public TProperty Get<TProperty>(object target, string propertyName)
        {
            var value = Get(target, propertyName);
            if (value is TProperty result)
            {
                return result;
            }
            return default;
        }

        public void Set(object target, string propertyName, object value)
        {
            throw new System.NotImplementedException();
        }
    }

    public class RoslynAccessor_Tree : IAccessor
    {
        public System.String SourceCode { get; }

        private SortedDictionary<string, Func<object, object>> Getters { get; } = new SortedDictionary<string, Func<object, object>>();
        private SortedDictionary<string, Action<object, object>> Setters { get; } = new SortedDictionary<string, Action<object, object>>();

        public RoslynAccessor_Tree(System.Type targetType, Sigma.Prop.Implementation implementation, System.String sourceCode)
        {
            SourceCode = sourceCode;

            Getters["Item"] = GetItem;
            Setters["Item"] = SetItem;
            Getters["Id"] = GetId;
            Getters["Text"] = GetText;
            Setters["Text"] = SetText;
            Getters["Value"] = GetValue;
            Setters["Value"] = SetValue;
            Getters["Comments"] = GetComments;
            Setters["Comments"] = SetComments;
            Getters["Tidbits"] = GetTidbits;
            Setters["Tidbits"] = SetTidbits;
            Getters["SomethingComplex"] = GetSomethingComplex;
            Setters["SomethingComplex"] = SetSomethingComplex;
        }

        public static object GetItem(object target)
        {
            return ((Sigma.Prop.Workbench.TestClass)target).Item;
        }
        public static void SetItem(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClass)target).Item = (Sigma.Prop.Workbench.ItemBase)value;
        }
        public static object GetId(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).Id;
        }
        public static object GetText(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).Text;
        }
        public static void SetText(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClassBase)target).Text = (System.String)value;
        }
        public static object GetValue(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).Value;
        }
        public static void SetValue(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClassBase)target).Value = (System.Nullable<System.Decimal>)value;
        }
        public static object GetComments(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).Comments;
        }
        public static void SetComments(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClassBase)target).Comments = (System.Collections.Generic.List<System.String>)value;
        }
        public static object GetTidbits(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).Tidbits;
        }
        public static void SetTidbits(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClassBase)target).Tidbits = (System.Byte[])value;
        }
        public static object GetSomethingComplex(object target)
        {
            return ((Sigma.Prop.Workbench.TestClassBase)target).SomethingComplex;
        }
        public static void SetSomethingComplex(object target, object value)
        {
            ((Sigma.Prop.Workbench.TestClassBase)target).SomethingComplex = (System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<Sigma.Prop.Workbench.TestClass[]>>[])value;
        }

        public object Get(object target, string propertyName)
        {
                if (propertyName == "Id") return GetId(target);
                if (propertyName == "Text") return GetText(target);
                if (propertyName == "Value") return GetValue(target);
                if (propertyName == "Comments") return GetComments(target);
                if (propertyName == "Tidbits") return GetTidbits(target);
                if (propertyName == "SomethingComplex") return GetSomethingComplex(target);
                if (propertyName == "Item") return GetItem(target);
                return default;
        }

        public TProperty Get<TProperty>(object target, string propertyName)
        {
            var value = Get(target, propertyName);
            if (value is TProperty result)
            {
                return result;
            }
            return default;
        }

        public void Set(object target, string propertyName, object value)
        {
            throw new System.NotImplementedException();
        }
    }
}
