using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTypes
{
    /// <summary>
    ///     Remote Object 
    /// </summary>
    public abstract class GeneralRemoteNode : MarshalByRefObject, IGeneralControlServices
    {

        private bool freeze;
        public bool IsFreeze
        {
            get { return freeze; }
            set { freeze = value; }
        }

        public GeneralRemoteNode()
        {
            IsFreeze = false;
        }

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

    }
}
