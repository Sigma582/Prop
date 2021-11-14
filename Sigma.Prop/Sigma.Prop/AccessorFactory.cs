using Sigma.Prop.Reflection;
using Sigma.Prop.Roslyn;
using System;
using System.Collections.Concurrent;

namespace Sigma.Prop
{
    //note:
    //Since accessors end up being lightweight, with all the heavy lifting done and cached by DelegateFactoryBase and its descendants,
    // caching accessors within AccessorFactory may be superfluous.
    //Reconsider if AccessorFactory is needed at all. Possible alternative could be:
    // * create an 'Accessor' class which instantiates one of the IAccessor implementations according to the config or ctor parameter; and
    // * let consumers instantiate the Accessor class whenever needed, possibly specifying desired implementation via ctor.
    public class AccessorFactory
    {
        //todo make configurable
        private Implementation _implementation = Implementation.Roslyn;

        #region singleton
        private static readonly Lazy<AccessorFactory> _lazy = new Lazy<AccessorFactory>(() => new AccessorFactory());

        public static AccessorFactory Instance => _lazy.Value;

        private AccessorFactory()
        {

        }
        #endregion

        private readonly ConcurrentDictionary<Type, Lazy<IAccessor>> _accessors = new ConcurrentDictionary<Type, Lazy<IAccessor>>();

        /// <summary>
        /// Create an instance of IAccessor, using the best implementation available.
        /// </summary>
        /// <typeparam name="T">Type of the object to be accessed.</typeparam>
        /// <returns></returns>
        public IAccessor Get<T>()
        {
            return Get(typeof(T));
        }

        /// <summary>
        /// Create an instance of IAccessor, using the best implementation available.
        /// </summary>
        /// <param name="targetType">Type of the object to be accessed.</param>
        /// <returns></returns>
        public IAccessor Get(Type targetType)
        {
            var exists = _accessors.TryGetValue(targetType, out Lazy<IAccessor> lazy);

            if (!exists)
            {
                lazy = new Lazy<IAccessor>(() => CreateAccessor(targetType), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

                //If another thread did this faster, we'll get that thread's instance of lazy and the one created above is
                // discarded - which is fine since creating Lazy is cheap.
                //What matters is that all concurrent threads end up using the same instance of lazy, 
                // and that instance ensures that the actual value is only initialized once.
                lazy = _accessors.GetOrAdd(targetType, lazy);
            }

            return lazy.Value;
        }

        private IAccessor CreateAccessor(Type targetType)
        {
            switch (_implementation)
            {
                case Implementation.Roslyn:
                    return new RoslynAccessor();
                case Implementation.Reflection:
                    return new ReflectionAccessor();

                default:
                    throw new ArgumentOutOfRangeException("Implementation type not specified.");
            }
        }
    }
}
