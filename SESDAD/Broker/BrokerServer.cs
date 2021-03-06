﻿using CommonTypes;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Broker
{
    public class BrokerServer : GenericRemoteNode, IGeneralControlServices ,IBroker
    {

        private BrokerLogic broker;

        private IDetectMessagesRepeated repeated = null;

        public BrokerServer(string name,string orderingPolicy,string routingPolicy,
            string loggingLevel,string pmLogServerUrl)
        {
            broker = new BrokerLogic(this,name, orderingPolicy, routingPolicy, loggingLevel, pmLogServerUrl);
            if (orderingPolicy.Equals("NO"))
            {
                repeated = new DetectRepeatedMessageNO();
            }
        }

        /*
         * Init is called after launch all processes and before the system start working.
         */
        public void Init(Object o)
        {
            broker.Init(o);
        }
        
        public ICollection<NodePair<IBroker>> GetNeighbours()
        {
            return broker.GetNeighbours();
        }

        public List<IBroker> GetChildren()
        {
            return broker.GetChildren();
        } 

        // Broker remote interface methods

        public void Status()
        {
            broker.Status();
        }

        public void Diffuse(Event e)
        {
            if(repeated != null)
            {
                if (repeated.IsRepeated(e.SequenceNumber, e.Publisher))
                    return;
            }
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

        public void Sequence(Bludger bludger)
        {
            lock (this)
            {
                broker.Sequence(bludger);
            }
        }

        public void Bludger(Bludger bludger)
        {
            lock (this)
            {
                broker.Bludger(bludger);
            }
        }

        public void DoSequence(Bludger bludger)
        {
            lock (this)
            {
                broker.DoSequence(bludger);
            }
        }

        public void DoBludger(Bludger bludger)
        {
            lock (this)
            {
                broker.DoBludger(bludger);
            }
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
