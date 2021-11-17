using Sigma.Prop.Roslyn;
using Sigma.Prop.Extension;
using System.Collections.Generic;
using System;
using System.IO;

namespace Sigma.Prop.Workbench
{
    class Program
    {
        static void Main(string[] args)
        {
            var accessor = AccessorFactory.Instance.Get<TestClass>();

            var test = new TestClass(10);

            var id = accessor.Get<int>(test, "Id");

            var t = test.Get("Text");
            t = test.Get("Tidbits");

            test.Item = new Item();
        }


    }

    public class TestClassBase
    {
        public TestClassBase(int id)
        {
            Id = id;

            var r = new Random();
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

        public TestClass(int id) : base(id)
        {
        }
    }

    public class ItemBase
    {
        public string Something { get; } = Path.GetRandomFileName();
    }

    public class Item : ItemBase
    {

    }
}
