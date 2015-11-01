using CommonTypes;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Broker
{
    public class BrokerServer : GenericRemoteNode, IGeneralControlServices ,IBroker
    {

        private BrokerLogic broker;

        public BrokerServer(string name,string orderingPolicy,string routingPolicy,
            string loggingLevel,string pmLogServerUrl, string parent, string[] children)
        {
            broker = new BrokerLogic(this,name, orderingPolicy, routingPolicy, loggingLevel, pmLogServerUrl,
                parent, children);
        }

        /**
         * Init is called after launch all processes and before the system start working.
         */
        public void Init()
        {
            broker.Init();
        }
        
        public ICollection<NodePair<IBroker>> GetNeighbours()
        {
            return broker.GetNeighbours();
        }

        // Broker remote interface methods

        public void Status()
        {
            broker.Status();
        }

        public void Diffuse(Event e)
        {
            broker.AddEventToDiffusion(e);
        }

        public void Subscribe(Subscription subscription)
        {
            broker.AddSubscription(subscription);
        }
   
        public void Unsubscribe(Subscription subscription)
        {
            broker.AddUnsubscription(subscription);
        }
        
        public void AddRoute(Route route)
        {
            broker.AddRoute(route);
        }

        public void RemoveRoute(Route route)
        {
            broker.RemoveRoute(route);
        }

        public void Crash()
        {
            broker.Crash();
        }

        public void Freeze()
        {
            broker.Freeze();
        }

        public void Unfreeze()
        {
            broker.Unfreeze();
        }
    }
}
