using Sigma.Prop.Roslyn;
using Sigma.Prop.Extension;
using System.Collections.Generic;
using System;
using System.IO;
using Sigma.Prop.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Linq;

namespace Sigma.Prop.Workbench
{
    class Program
    {
        static void Main(string[] args)
        {
            //var accessor = AccessorFactory.Instance.Get<TestClass>();

            //var test = new TestClass();
            //var id = accessor.Get<int>(test, "Id");
            //var t = test.Get("Text");
            //t = test.Get("Tidbits");
            //test.Item = new Item();

            var iterations = 5 * 1000 * 1000;
            var target = new List<TestClass>(iterations);
            for (int i = 0; i < iterations; i++)
            {
                target.Add(new TestClass());
            }

            MeasureDuration(new MeasureSingle(), target);
            MeasureDuration(new MeasureBatch(), target);
            Console.ReadLine();
        }

        private static void MeasureDuration(IMeasureMethods measureMethods, IList<TestClass> target)
        {
            IAccessor accessor;
            accessor = new ReflectionAccessor();
            var reflectionDuration = measureMethods.MeasureDuration(target, accessor);

            accessor = new SampleAccessor_Dictionary();
            var roslynDuration = measureMethods.MeasureDuration(target, accessor);

            var roslynDurationDirectMethod = measureMethods.MeasureDurationDirectMethod(target);
            var roslynDurationDirectMethod2 = measureMethods.MeasureDurationDirectMethod2(target);
            var roslynDurationDirectMethod3 = measureMethods.MeasureDurationDirectMethod3(target);

            accessor = new SampleAccessor_Switch();
            var roslynDurationSwitch = measureMethods.MeasureDuration(target, accessor);

            accessor = new SampleAccessor_If();
            var roslynDurationIf = measureMethods.MeasureDuration(target, accessor);

            accessor = new SampleAccessor_Tree();
            var roslynDurationTree = measureMethods.MeasureDuration(target, accessor);

            var directDuration = measureMethods.MeasureDurationDirectAccess(target);

            Console.WriteLine($"Execution time at {target.Count} items, using method accepting {measureMethods.ParameterKind}:");
            Console.WriteLine($"* Reflection                  : {reflectionDuration.TotalSeconds} sec.");
            Console.WriteLine($"* Dictionary                  : {roslynDuration.TotalSeconds} sec.");
            Console.WriteLine($"* Switch                      : {roslynDurationSwitch.TotalSeconds} sec.");
            Console.WriteLine($"* If                          : {roslynDurationIf.TotalSeconds} sec.");
            Console.WriteLine($"* Tree                        : {roslynDurationTree.TotalSeconds} sec.");
            Console.WriteLine($"* Direct method call          : {roslynDurationDirectMethod.TotalSeconds} sec.");
            Console.WriteLine($"* Direct method call 2        : {roslynDurationDirectMethod2.TotalSeconds} sec.");
            Console.WriteLine($"* Direct method call 3        : {roslynDurationDirectMethod3.TotalSeconds} sec.");
            Console.WriteLine($"* Direct value access         : {directDuration.TotalSeconds} sec.");
        }
    }

    public interface IMeasureMethods
    {
        string ParameterKind { get; }

        TimeSpan MeasureDuration(IList<TestClass> target, IAccessor accessor);
        TimeSpan MeasureDurationDirectAccess(IList<TestClass> target);
        TimeSpan MeasureDurationDirectMethod(IList<TestClass> target);
        TimeSpan MeasureDurationDirectMethod2(IList<TestClass> target);
        TimeSpan MeasureDurationDirectMethod3(IList<TestClass> target);
    }

    public class MeasureSingle : IMeasureMethods
    {
        public string ParameterKind => "single item";

        public TimeSpan MeasureDuration(IList<TestClass> target, IAccessor accessor)
        {
            var startTime = DateTime.Now;
            var e = target.GetEnumerator();
            while (e.MoveNext())
            {
                _ = accessor.Get(e.Current, "Item");
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectAccess(IList<TestClass> target)
        {
            var startTime = DateTime.Now;
            foreach (var item in target)
            {
                _ = item.Item;
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod(IList<TestClass> target)
        {
            var startTime = DateTime.Now;
            foreach (var item in target)
            {
                _ = SampleAccessor_Dictionary.GetItem(item);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod2(IList<TestClass> target)
        {
            var startTime = DateTime.Now;
            foreach (var item in target)
            {
                _ = SampleAccessor_Dictionary.GetItem2(item);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod3(IList<TestClass> target)
        {
            var startTime = DateTime.Now;
            foreach (var item in target)
            {
                _ = SampleAccessor_Dictionary.GetItem3(item);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }
    }

    public class MeasureBatch : IMeasureMethods
    {
        public string ParameterKind => "IEnumerable<T>";

        public TimeSpan MeasureDuration(IList<TestClass> target, IAccessor accessor)
        {
            var startTime = DateTime.Now;
            //var items = accessor.Get(target, "Item").Cast<object>().ToList();
            var enumerator = accessor.Get(target, "Item").GetEnumerator();
            while (enumerator.MoveNext())
            {
                _ = enumerator.Current;
            }

            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectAccess(IList<TestClass> target)
        {
            var startTime = DateTime.Now;
            foreach (var item in target)
            {
                _ = item.Item;
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod(IList<TestClass> target)
        {
            var startTime = DateTime.Now;
            foreach (var item in target)
            {
                _ = SampleAccessor_Dictionary.GetItem(item);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod2(IList<TestClass> target)
        {
            var startTime = DateTime.Now;
            foreach (var item in target)
            {
                _ = SampleAccessor_Dictionary.GetItem2(item);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod3(IList<TestClass> target)
        {
            var startTime = DateTime.Now;
            foreach (var item in target)
            {
                _ = SampleAccessor_Dictionary.GetItem3(item);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }
    }

    public class TestClassBase
    {
        protected Random Rand = new Random();

        public TestClassBase()
        {
            Id = Rand.Next(0, int.MaxValue);
            Value = Rand.Next(0, int.MaxValue);
            Text = Path.GetRandomFileName();
            Comments = new List<string>
            {
                  Path.GetRandomFileName()
                , Path.GetRandomFileName()
                , Path.GetRandomFileName()
            };

            Tidbits = new byte[4];
            Rand.NextBytes(Tidbits);
        }

        public int Id { get; private set; }

        public string Text { get; set; }

        public decimal? Value { get; set; }

        public List<string> Comments { get; set; }

        public byte[] Tidbits { get; set; }

        public Dictionary<string, List<TestClass[]>>[] SomethingComplex { get; set; }
    }

    public class TestClass : TestClassBase
    {
        public ItemBase Item { get; set; }
    }

    public class ItemBase
    {
        public string Something { get; } = Path.GetRandomFileName();
    }

    public class Item : ItemBase
    {

    }
}
