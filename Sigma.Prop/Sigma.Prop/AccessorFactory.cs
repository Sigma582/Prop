using System;

namespace Sigma.Prop
{
    public class AccessorFactory
    {
        private static readonly Lazy<AccessorFactory> _lazy = new Lazy<AccessorFactory>();

        public static AccessorFactory Instance => _lazy.Value;

        /// <summary>
        /// Create an instance of IAccessor, using the best implementation available.
        /// </summary>
        /// <returns></returns>
        public IAccessor Get()
        {
            throw new NotImplementedException();
        }
    }
}
