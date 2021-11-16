using System;
using System.Collections.Concurrent;

namespace Sigma.Prop
{
    public class AccessorMethodsCollection
    {
        public AccessorMethodsCollection(ConcurrentDictionary<string, Func<object, object>> getters, ConcurrentDictionary<string, Action<object, object>> setters)
        {
            Getters = getters;
            Setters = setters;
        }

        public ConcurrentDictionary<string, Func<object, object>> Getters { get; }
        public ConcurrentDictionary<string, Action<object, object>> Setters { get; }
    }
}
