using System;
using System.Threading;

namespace CommonTypes
{
    /// <summary>
    ///     Remote Object 
    /// </summary>
    public abstract class GenericRemoteNode : MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
