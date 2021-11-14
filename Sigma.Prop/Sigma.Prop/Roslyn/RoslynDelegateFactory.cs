using System;
using System.Collections.Concurrent;

namespace Sigma.Prop.Roslyn
{
    internal class RoslynDelegateFactory : DelegateFactoryBase
    {
        #region singleton
        private static readonly Lazy<RoslynDelegateFactory> _lazy = new Lazy<RoslynDelegateFactory>(() => new RoslynDelegateFactory());

        public static RoslynDelegateFactory Instance => _lazy.Value;

        private RoslynDelegateFactory()
        {

        }
        #endregion

        protected override ConcurrentDictionary<string, Func<object, object>> CreateGetters(Type type)
        {
            throw new NotImplementedException();
        }

        protected override ConcurrentDictionary<string, Action<object, object>> CreateSetters(Type type)
        {
            throw new NotImplementedException();
        }

    }
}
