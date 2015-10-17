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
        private string orderingPolicy;

        private string routingPolicy;

        private string loggingLevel;

        private string pmLogServerUrl;

        private string parent;
        
        private string[] children;

        private TopicSubscriberCollection topicSubscribers;

        public BrokerServer(string orderingPolicy,string routingPolicy,
            string loggingLevel,string pmLogServerUrl, string parent, string[] children)
        {
            this.pmLogServerUrl = pmLogServerUrl;
            this.orderingPolicy = orderingPolicy;
            this.routingPolicy = routingPolicy;
            this.loggingLevel = loggingLevel;
            this.parent = parent;
            this.children = children;
            
            this.topicSubscribers = new TopicSubscriberCollection();
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

        public void Diffuse(Message message)
        {
            throw new NotImplementedException();
            // Enviar aos Brokers vizinhos.
            // Enviar aos Subscribers que querem a messagem.
        }

        public void Subscribe(string subscriber, string topic)
        {
            this.topicSubscribers.Add(topic, subscriber);
            // Enviar aos brokers vizinhos
            throw new NotImplementedException();
        }

        public void Unsubscribe(string subscriber, string topic)
        {
            this.topicSubscribers.Remove(topic, subscriber);
            // Enviar aos brokers vizinhos
            throw new NotImplementedException();
        }
    }
}
