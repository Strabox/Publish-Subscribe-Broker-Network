using System;
using System.Threading;

namespace CommonTypes
{
    /// <summary>
    ///     Remote Object 
    /// </summary>
    public abstract class GenericRemoteNode : MarshalByRefObject, IGeneralControlServices
    {

        private bool freeze;
        protected bool IsFreeze
        {
            get { return freeze; }
            set { freeze = value; }
        }

        public GenericRemoteNode()
        {
            IsFreeze = false;
        }

        protected void BlockWhileFrozen()
        {
            lock (this)
            {
                while (IsFreeze)
                    Monitor.Wait(this);
            }
        }

        // Control Services Interface method's

        public void Crash()
        {
            System.Environment.Exit(-1);
        }

        public void Freeze()
        {
            lock (this)
            {
                IsFreeze = true;
            }
        }

        public void Unfreeze()
        {
            lock(this)
            {
                IsFreeze = false;
                Monitor.PulseAll(this);
            }
        }

        public abstract void Init();

        public abstract void Status();

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
