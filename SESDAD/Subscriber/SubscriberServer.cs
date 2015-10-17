using CommonTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    public class SubscriberServer : MarshalByRefObject, IGeneralControlServices,
        ISubscriberControlServices
    {
        private string pmLogServerUrl;

        private string loggingLevel;

        private string[] brokers;

        public SubscriberServer(string pmLogServerUrl,string loggingLevel,
            string[] brokers)
        {
            this.pmLogServerUrl = pmLogServerUrl;
            this.loggingLevel = loggingLevel;
            this.brokers = brokers;
        }

        // General test and control methods.

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
            System.Environment.Exit(-1);
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
