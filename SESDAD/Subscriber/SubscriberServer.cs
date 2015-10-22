using CommonTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

        private bool isFreeze;
        private bool IsFreeze
        {
            get { return isFreeze; }
            set { isFreeze = value; }
        }

        public SubscriberServer(string name, string pmLogServerUrl,string loggingLevel,
            string[] brokers)
        {
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.loggingLevel = loggingLevel;
            this.brokers = brokers;
            IsFreeze = false;
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;
        }

        // Subscriber specific methods.

        public void Receive(Event e)
        {
            //TODO if we need save messages or something like that.
            logServer.LogAction("SubEvent " + name + ", " + e.Publisher
                + ", " + e.Topic + ", " + e.EventNumber);
        }

        // General test and control methods.

        public void Subscribe(string topicName)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessSubscribe),topicName);
        }

        public void Unsubscribe(string topicName)
        {
            IBroker broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
            broker.Unsubscribe(new Subscription(this.name, topicName, this.name, this));
        }

        private void ProcessSubscribe(Object o)
        {
            lock (this)
            {
                while (IsFreeze)
                    Monitor.Wait(this);
            }
            string topicName = o as string;
            IBroker broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
            broker.Subscribe(new Subscription(this.name, topicName, this.name, this as ISubscriber));
            logServer.LogAction("SubSubscribe " + name + " Subscribe " + topicName);
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

        public void Status()
        {
            //TODO
        }

        public void Unfreeze()
        {
            lock (this)
            {
                IsFreeze = false;
                Monitor.PulseAll(this);
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public string GetName()
        {
            return this.name;
        }

        public void Init()
        {
            //Do nothing
        }
    }
}
