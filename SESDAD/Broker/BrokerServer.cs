using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    public class BrokerServer : MarshalByRefObject, IGeneralControlServices, IBroker
    {
        private string name;

        private string orderingPolicy;

        private string routingPolicy;

        private string loggingLevel;

        private string pmLogServerUrl;

        private string parent;
        
        private string[] children;

        private IPuppetMasterLog logServer;

        private TopicSubscriberCollection topicSubscribers;

        public BrokerServer(string name,string orderingPolicy,string routingPolicy,
            string loggingLevel,string pmLogServerUrl, string parent, string[] children)
        {
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.orderingPolicy = orderingPolicy;
            this.routingPolicy = routingPolicy;
            this.loggingLevel = loggingLevel;
            this.parent = parent;
            this.children = children;
            
            this.topicSubscribers = new TopicSubscriberCollection();
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;
        }

        // Broker specific methods.

        public void Diffuse(Message message)
        {
            // TODO
            // Enviar aos Brokers vizinhos.
            // Enviar aos Subscribers que querem a messagem.
            if (loggingLevel.Equals("full"))
                logServer.LogAction("BroEvent "+ name +", "+"MissingPublisherName"+", "
                    + message.Topic+", " + "MissingEventNumber");
        }

        public void Subscribe(ISubscriber subscriber, string topic)
        {
            this.topicSubscribers.Add(topic, subscriber);
            // Enviar aos brokers vizinhos
            // TODO
        }

        public void Unsubscribe(ISubscriber subscriber, string topic)
        {
            this.topicSubscribers.Remove(topic, subscriber);
            // Enviar aos brokers vizinhos
            // TODO
        }

        // General test and control methods.

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
