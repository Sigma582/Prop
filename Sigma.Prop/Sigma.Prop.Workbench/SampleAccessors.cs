using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sigma.Prop.Workbench
{
    public abstract class SampleAccessorBase : IAccessor
    {
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

        public abstract object Get(object target, string propertyName);
        public abstract TProperty Get<TProperty>(object target, string propertyName);
        public abstract void Set(object target, string propertyName, object value);
        public abstract IEnumerable Get(IEnumerable targets, string propertyName);
        public abstract IEnumerable<TProperty> Get<TProperty>(IEnumerable targets, string propertyName);
        public abstract void Set(IEnumerable targets, string propertyName, IEnumerable values);
    }

    public class SampleAccessor_Dictionary : SampleAccessorBase
    {
        protected Dictionary<string, Func<object, object>> Getters { get; } = new Dictionary<string, Func<object, object>>();
        protected Dictionary<string, Action<object, object>> Setters { get; } = new Dictionary<string, Action<object, object>>();

        public Type TargetType { get; }
        public Implementation Implementation { get; }

        public override object Get(object target, string propertyName)
        {
            if (!Getters.TryGetValue(propertyName, out Func<object, object> getter))
            {
                throw new NotSupportedException();
            }

            return getter(target);
        }

        public override TProperty Get<TProperty>(object target, string propertyName)
        {
            var value = Get(target, propertyName);
            if (value is TProperty result)
            {
                return result;
            }
            return default;
        }

        public override void Set(object target, string propertyName, object value)
        {
            if (!Setters.TryGetValue(propertyName, out Action<object, object> setter))
            {
                throw new NotSupportedException();
            }

            setter(target, value);
        }

        public override IEnumerable Get(IEnumerable targets, string propertyName)
        {
            if (!Getters.TryGetValue(propertyName, out Func<object, object> getter))
            {
                throw new NotSupportedException();
            }

            object item;
            foreach (var target in targets)
            {
                item = getter(target);
                yield return item;
            }
        }

        public override IEnumerable<TProperty> Get<TProperty>(IEnumerable targets, string propertyName)
        {
            var values = Get(targets, propertyName);
            foreach (var value in values)
            {
                if (value is TProperty result)
                {
                    yield return result;
                }
                yield return default;
            }
        }

        public override void Set(IEnumerable targets, string propertyName, IEnumerable values)
        {
            if (!Setters.TryGetValue(propertyName, out Action<object, object> setter))
            {
                throw new NotSupportedException();
            }

            if (targets is ICollection targetsCollection && values is ICollection valuesCollection)
            {
                if (targetsCollection.Count != valuesCollection.Count)
                {
                    throw new InvalidOperationException($"Number of values ({valuesCollection.Count}) is not equal to the number of target objects ({targetsCollection.Count}).");
                }
            }

            var targetsEnumerator = targets.GetEnumerator();
            var valuesEnumerator = values.GetEnumerator();

            while (targetsEnumerator.MoveNext() && valuesEnumerator.MoveNext())
            {
                setter(targetsEnumerator.Current, valuesEnumerator.Current);
            }
        }

        public SampleAccessor_Dictionary()
        {
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
    }

    public class SampleAccessor_Switch : SampleAccessorBase
    {
        public override object Get(object target, string propertyName)
        {
            return propertyName switch
            {
                "Item" => GetItem(target),
                "Id" => GetId(target),
                "Text" => GetText(target),
                "Value" => GetValue(target),
                "Comments" => GetComments(target),
                "Tidbits" => GetTidbits(target),
                "SomethingComplex" => GetSomethingComplex(target),
                _ => default,
            };
        }

        public override TProperty Get<TProperty>(object target, string propertyName)
        {
            var value = Get(target, propertyName);
            if (value is TProperty result)
            {
                return result;
            }
            return default;
        }

        public override void Set(object target, string propertyName, object value)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable Get(IEnumerable targets, string propertyName)
        {
            switch (propertyName)
            {
                case "Item": foreach (var target in targets) yield return GetItem(target); break;
                case "Id": foreach (var target in targets) yield return GetId(target); break;
                case "Text": foreach (var target in targets) yield return GetText(target); break;
                case "Value": foreach (var target in targets) yield return GetValue(target); break;
                case "Comments": foreach (var target in targets) yield return GetComments(target); break;
                case "Tidbits": foreach (var target in targets) yield return GetTidbits(target); break;
                case "SomethingComplex": foreach (var target in targets) yield return GetSomethingComplex(target); break;
                default: foreach (var target in targets) yield return default; break;
            }
        }

        public override IEnumerable<TProperty> Get<TProperty>(IEnumerable targets, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override void Set(IEnumerable targets, string propertyName, IEnumerable values)
        {
            throw new NotImplementedException();
        }
    }

    public class SampleAccessor_If : SampleAccessorBase
    {
        public override object Get(object target, string propertyName)
        {
            if (target is Sigma.Prop.Workbench.TestClassBase)
                if (propertyName == "Item") return ((Sigma.Prop.Workbench.TestClass)target).Item;
            if (propertyName == "Id") return ((Sigma.Prop.Workbench.TestClass)target).Id;
            if (propertyName == "Text") return ((Sigma.Prop.Workbench.TestClass)target).Text;
            if (propertyName == "Value") return ((Sigma.Prop.Workbench.TestClass)target).Value;
            if (propertyName == "Comments") return ((Sigma.Prop.Workbench.TestClass)target).Comments;
            if (propertyName == "Tidbits") return ((Sigma.Prop.Workbench.TestClass)target).Tidbits;
            if (propertyName == "SomethingComplex") return ((Sigma.Prop.Workbench.TestClass)target).SomethingComplex;
            return default;
        }

        public override TProperty Get<TProperty>(object target, string propertyName)
        {
            var value = Get(target, propertyName);
            if (value is TProperty result)
            {
                return result;
            }
            return default;
        }

        public override void Set(object target, string propertyName, object value)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable Get(IEnumerable targets, string propertyName)
        {
            if (propertyName == "Item") foreach (var target in targets) yield return ((Sigma.Prop.Workbench.TestClass)target).Item;
            if (propertyName == "Id") foreach (var target in targets) yield return ((Sigma.Prop.Workbench.TestClass)target).Id;
            if (propertyName == "Text") foreach (var target in targets) yield return ((Sigma.Prop.Workbench.TestClass)target).Text;
            if (propertyName == "Value") foreach (var target in targets) yield return ((Sigma.Prop.Workbench.TestClass)target).Value;
            if (propertyName == "Comments") foreach (var target in targets) yield return ((Sigma.Prop.Workbench.TestClass)target).Comments;
            if (propertyName == "Tidbits") foreach (var target in targets) yield return ((Sigma.Prop.Workbench.TestClass)target).Tidbits;
            if (propertyName == "SomethingComplex") foreach (var target in targets) yield return ((Sigma.Prop.Workbench.TestClass)target).SomethingComplex;
        }

        public override IEnumerable<TProperty> Get<TProperty>(IEnumerable targets, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override void Set(IEnumerable targets, string propertyName, IEnumerable values)
        {
            throw new NotImplementedException();
        }
    }

    public class SampleAccessor_Tree : SampleAccessorBase
    {
        private SortedDictionary<string, Func<object, object>> Getters { get; } = new SortedDictionary<string, Func<object, object>>();
        private SortedDictionary<string, Action<object, object>> Setters { get; } = new SortedDictionary<string, Action<object, object>>();

        public SampleAccessor_Tree()
        {
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

        public override object Get(object target, string propertyName)
        {
            if (!Getters.TryGetValue(propertyName, out Func<object, object> getter))
            {
                throw new NotSupportedException();
            }

            return getter(target);
        }

        public override TProperty Get<TProperty>(object target, string propertyName)
        {
            var value = Get(target, propertyName);
            if (value is TProperty result)
            {
                return result;
            }
            return default;
        }

        public override void Set(object target, string propertyName, object value)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable Get(IEnumerable targets, string propertyName)
        {
            if (!Getters.TryGetValue(propertyName, out Func<object, object> getter))
            {
                throw new NotSupportedException();
            }

            foreach (var target in targets)
            {
                yield return getter(target);
            }
        }

        public override IEnumerable<TProperty> Get<TProperty>(IEnumerable targets, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override void Set(IEnumerable targets, string propertyName, IEnumerable values)
        {
            throw new NotImplementedException();
        }
    }
}
