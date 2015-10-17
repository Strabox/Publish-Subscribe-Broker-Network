using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public class PublisherServer : MarshalByRefObject, IGeneralControlServices,
        IPublisherControlServices
    {
        private string pmLogServerUrl;

        private string loggingLevel;

        private string[] brokers;

        public PublisherServer(string pmLogServerUrl,string loggingLevel,
            string[] brokers)
        {
            this.pmLogServerUrl = pmLogServerUrl;
            this.loggingLevel = loggingLevel;
            this.brokers = brokers;
        }

        // General test and control methods.

        public void Publish(string topicName, int numberOfEvents, int interval)
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
