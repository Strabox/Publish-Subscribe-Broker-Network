using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public abstract class GeneralRemoteNode : MarshalByRefObject, IGeneralControlServices
    {

        private bool freeze;
        public bool IsFreeze
        {
            get { return freeze; }
            set { freeze = value; }
        }

        private Queue<Event> events;

        public GeneralRemoteNode()
        {
            IsFreeze = false;
            events = new Queue<Event>();
        }

        public void Crash()
        {
            System.Environment.Exit(-1);
        }

        public void Freeze()
        {
            //TODO
        }

        public void Unfreeze()
        {
            //TODO
        }

        public void Init()
        {
            //TODO
        }

        public void Status()
        {
            //TODO
        }

    }
}
