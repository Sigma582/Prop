using Sigma.Prop.Roslyn;
using Sigma.Prop.Extension;
using System.Collections.Generic;
using System;
using System.IO;
using Sigma.Prop.Reflection;
using System.Diagnostics;

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

            var iterations = 100000000;

            IAccessor accessor = new ReflectionAccessor();
            var reflectionDuration = MeasureDuration(iterations, accessor);

            accessor = new RoslynAccessor(typeof(TestClass), Implementation.Roslyn, "");
            var roslynDuration = MeasureDuration(iterations, accessor);

            var roslynDurationDirectMethod = MeasureDurationDirectMethod(iterations);
            var roslynDurationDirectMethod2 = MeasureDurationDirectMethod2(iterations);
            var roslynDurationDirectMethod3 = MeasureDurationDirectMethod3(iterations);

            accessor = new RoslynAccessor_Switch(typeof(TestClass), Implementation.Roslyn, "");
            var roslynDurationSwitch = MeasureDuration(iterations, accessor);

            accessor = new RoslynAccessor_If(typeof(TestClass), Implementation.Roslyn, "");
            var roslynDurationIf = MeasureDuration(iterations, accessor);

            accessor = new RoslynAccessor_Tree(typeof(TestClass), Implementation.Roslyn, "");
            var roslynDurationTree = MeasureDuration(iterations, accessor);

            var directDuration = MeasureDurationDirectAccess(iterations);

            Debug.WriteLine($"Execution time at {iterations} iterations:");
            Debug.WriteLine($"* Reflection                  : {reflectionDuration.TotalSeconds} sec.");
            Debug.WriteLine($"* Roslyn_Dictionary           : {roslynDuration.TotalSeconds} sec.");
            Debug.WriteLine($"* Roslyn_Switch               : {roslynDurationSwitch.TotalSeconds} sec.");
            Debug.WriteLine($"* Roslyn_If                   : {roslynDurationIf.TotalSeconds} sec.");
            Debug.WriteLine($"* Roslyn_Tree                 : {roslynDurationTree.TotalSeconds} sec.");
            Debug.WriteLine($"* Roslyn direct method call   : {roslynDurationDirectMethod.TotalSeconds} sec.");
            Debug.WriteLine($"* Roslyn direct method call 2 : {roslynDurationDirectMethod2.TotalSeconds} sec.");
            Debug.WriteLine($"* Roslyn direct method call 3 : {roslynDurationDirectMethod3.TotalSeconds} sec.");
            Debug.WriteLine($"* Direct value access         : {directDuration.TotalSeconds} sec.");
        }

        private static TimeSpan MeasureDuration(int iterations, IAccessor accessor)
        {
            var target = new TestClass();

            object item;
            var startTime = DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                item = accessor.Get(target, "Item");
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        private static TimeSpan MeasureDurationDirectAccess(int iterations)
        {
            var target = new TestClass();

            object item;
            var startTime = DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                item = target.Item;
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        private static TimeSpan MeasureDurationDirectMethod(int iterations)
        {
            var target = new TestClass();

            object item;
            var startTime = DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                item = RoslynAccessor.GetItem(target);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        private static TimeSpan MeasureDurationDirectMethod2(int iterations)
        {
            var target = new TestClass();

            object item;
            var startTime = DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                item = RoslynAccessor.GetItem2(target);
            }
            var duration = DateTime.Now - startTime;
            return duration;
        }

        private static TimeSpan MeasureDurationDirectMethod3(int iterations)
        {
            var target = new TestClass();
            var accessor = new RoslynAccessor(null, Implementation.Roslyn, "");

            object item;
            var startTime = DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                item = RoslynAccessor.GetItem3(target);
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
}
