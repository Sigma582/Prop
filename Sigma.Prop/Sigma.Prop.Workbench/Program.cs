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

            MeasureDuration(new MeasureSingle());
            MeasureDuration(new MeasureBatch());
        }

        private static void MeasureDuration(IMeasureMethods measureMethods, int iterations = 100000000)
        {
            IAccessor accessor;
            //accessor = new ReflectionAccessor();
            //var reflectionDuration = measureMethods.MeasureDuration(iterations, accessor);
            //
            accessor = new RoslynAccessor(typeof(TestClass), Implementation.Roslyn, "");
            var roslynDuration = measureMethods.MeasureDuration(iterations, accessor);
            //
            //var roslynDurationDirectMethod = measureMethods.MeasureDurationDirectMethod(iterations);
            //var roslynDurationDirectMethod2 = measureMethods.MeasureDurationDirectMethod2(iterations);
            //var roslynDurationDirectMethod3 = measureMethods.MeasureDurationDirectMethod3(iterations);
            //
            //accessor = new RoslynAccessor_Switch(typeof(TestClass), Implementation.Roslyn, "");
            //var roslynDurationSwitch = measureMethods.MeasureDuration(iterations, accessor);
            //
            //accessor = new RoslynAccessor_If(typeof(TestClass), Implementation.Roslyn, "");
            //var roslynDurationIf = measureMethods.MeasureDuration(iterations, accessor);
            //
            //accessor = new RoslynAccessor_Tree(typeof(TestClass), Implementation.Roslyn, "");
            //var roslynDurationTree = measureMethods.MeasureDuration(iterations, accessor);
            //
            //var directDuration = measureMethods.MeasureDurationDirectAccess(iterations);
            //
            //Debug.WriteLine($"Execution time at {iterations} iterations:");
            //Debug.WriteLine($"* Reflection                  : {reflectionDuration.TotalSeconds} sec.");
            Debug.WriteLine($"* Roslyn_Dictionary           : {roslynDuration.TotalSeconds} sec.");
            //Debug.WriteLine($"* Roslyn_Switch               : {roslynDurationSwitch.TotalSeconds} sec.");
            //Debug.WriteLine($"* Roslyn_If                   : {roslynDurationIf.TotalSeconds} sec.");
            //Debug.WriteLine($"* Roslyn_Tree                 : {roslynDurationTree.TotalSeconds} sec.");
            //Debug.WriteLine($"* Direct method call          : {roslynDurationDirectMethod.TotalSeconds} sec.");
            //Debug.WriteLine($"* Direct method call 2        : {roslynDurationDirectMethod2.TotalSeconds} sec.");
            //Debug.WriteLine($"* Direct method call 3        : {roslynDurationDirectMethod3.TotalSeconds} sec.");
            //Debug.WriteLine($"* Direct value access         : {directDuration.TotalSeconds} sec.");
        }
    }

    public interface IMeasureMethods
    {
        TimeSpan MeasureDuration(int iterations, IAccessor accessor);
        TimeSpan MeasureDurationDirectAccess(int iterations);
        TimeSpan MeasureDurationDirectMethod(int iterations);
        TimeSpan MeasureDurationDirectMethod2(int iterations);
        TimeSpan MeasureDurationDirectMethod3(int iterations);
    }

    public class MeasureSingle : IMeasureMethods
    {
        public TimeSpan MeasureDuration(int iterations, IAccessor accessor)
        {
            var targets = new TestClassBatch(iterations);

            var startTime = DateTime.Now;
            var e = targets.GetEnumerator();
            while (e.MoveNext())
            {
                _ = accessor.Get(e.Current, "Item");
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectAccess(int iterations)
        {
            var targets = new TestClassBatch(iterations);

            var startTime = DateTime.Now;
            foreach (var target in targets)
            {
                _ = target.Item;
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod(int iterations)
        {
            var targets = new TestClassBatch(iterations);

            var startTime = DateTime.Now;
            foreach (var target in targets)
            {
                _ = RoslynAccessor.GetItem(target);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod2(int iterations)
        {
            var targets = new TestClassBatch(iterations);

            var startTime = DateTime.Now;
            foreach (var target in targets)
            {
                _ = RoslynAccessor.GetItem2(target);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod3(int iterations)
        {
            var targets = new TestClassBatch(iterations);

            var startTime = DateTime.Now;
            foreach (var target in targets)
            {
                _ = RoslynAccessor.GetItem3(target);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }
    }

    public class MeasureBatch : IMeasureMethods
    {
        public TimeSpan MeasureDuration(int iterations, IAccessor accessor)
        {
            var target = new TestClassBatch(iterations);

            var startTime = DateTime.Now;
            //var items = accessor.Get(target, "Item").Cast<object>().ToList();
            while (accessor.Get(target, "Item").GetEnumerator().MoveNext())
            {

            }

            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectAccess(int iterations)
        {
            var target = new TestClass();

            var startTime = DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                _ = target.Item;
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod(int iterations)
        {
            var targets = new TestClassBatch(iterations);

            var startTime = DateTime.Now;
            foreach (var target in targets)
            {
                _ = RoslynAccessor.GetItem(target);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod2(int iterations)
        {
            var targets = new TestClassBatch(iterations);

            var startTime = DateTime.Now;
            foreach (var target in targets)
            {
                _ = RoslynAccessor.GetItem2(target);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        public TimeSpan MeasureDurationDirectMethod3(int iterations)
        {
            var targets = new TestClassBatch(iterations);

            var startTime = DateTime.Now;
            foreach (var target in targets)
            {
                _ = RoslynAccessor.GetItem3(target);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }
    }

    public class TestClassBase
    {
        public TestClassBase()
        {
            var r = new Random();

            Id = r.Next(0, int.MaxValue);
            Value = r.Next(0, int.MaxValue);
            Text = Path.GetRandomFileName();
            Comments = new List<string> 
            {
                  Path.GetRandomFileName()
                , Path.GetRandomFileName()
                , Path.GetRandomFileName()
                , Path.GetRandomFileName() 
            };

            Tidbits = new byte[10];
            r.NextBytes(Tidbits);
        }

        public int Id { get; private set; }

        public string Text { get; set; }

        public decimal? Value { get; set; }

        public List<string> Comments { get; set; }

        public byte[] Tidbits { get; set; }

        public Dictionary<string,List<TestClass[]>>[] SomethingComplex { get; set; }
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

    public class TestClassBatch : IEnumerable<TestClass>, IEnumerator<TestClass>
    {
        private readonly int _count;
        private int _current;
        private TestClass _item = new TestClass();

        public TestClassBatch(int count)
        {
            _count = count;
        }

        public TestClass Current => _item;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            return _current++ < _count;
        }

        public void Reset()
        {
            _current = 0;
        }
        public IEnumerator<TestClass> GetEnumerator()
        {
            return this;
        }

        public void Dispose()
        {
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
