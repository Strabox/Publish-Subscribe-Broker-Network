using CommonTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    public class SubscriberServer : MarshalByRefObject, ICommon, ISubscriber
    {

        public void Subscribe(string topicName)
        {
            //TODO
        }

        public void Unsubscribe(string topicName)
        {
            //TODO
        }

        public void Crash()
        {
            Process.GetCurrentProcess().Kill();
        }

        public void Freeze()
        {
            //TODO
        }

        public void Status()
        {
            //TODO
        }

        public void Unfreeze()
        {
            //TODO
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

    }
}
