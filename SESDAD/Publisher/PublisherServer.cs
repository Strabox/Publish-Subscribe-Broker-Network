using CommonTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public class PublisherServer : MarshalByRefObject, ICommon, IPublisher
    {

        public void Publish(string topicName, int numberOfEvents, int interval)
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

        /*As far as I know this is necessary to override the
        .NET Remoting lease times. */
        public override object InitializeLifetimeService()
        {
            return null;
        }

    }
}
