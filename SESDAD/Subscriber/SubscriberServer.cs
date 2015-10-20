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
        ISubscriberControlServices, ISubscriber
    {
        private string name;

        private string pmLogServerUrl;

        private string loggingLevel;

        private string[] brokers;

        private IPuppetMasterLog logServer;


        public SubscriberServer(string name, string pmLogServerUrl,string loggingLevel,
            string[] brokers)
        {
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.loggingLevel = loggingLevel;
            this.brokers = brokers;

            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;
        }

        // Subscriber specific methods.

        public void Receive(Event e)
        {
            //TODO if we need save messages or something like that.
            logServer.LogAction("SubEvent " + name+", " + "MissingPublisherName" 
                + ", " + e.Topic + ", " + "MissingEventNumber");
        }

        // General test and control methods.

        public void Subscribe(string topicName)
        {
            IBroker broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
            broker.Subscribe(new Subscription(this.name, topicName, this as ISubscriber));
            logServer.LogAction("SubSubscribe " + name + " Subscribe " + topicName);
            Console.WriteLine("SubSubscribe " + name + " Subscribe " + topicName);
        }

        public void Unsubscribe(string topicName)
        {
            IBroker broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
            broker.Unsubscribe(new Subscription(this.name, topicName, this));
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

        public string GetName()
        {
            return this.name;
        }

    }
}
