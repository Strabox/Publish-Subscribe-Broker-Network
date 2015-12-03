using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace CommonTypes
{   

    /// <summary>
    ///     BrokerSite hides the replication in a site making the calls transparent.
    /// </summary>
    [Serializable]
    public class BrokerSiteFrontEnd : IBroker
    {
        private string siteName;

        private List<BrokerPairDTO> brokersAlive;


        public BrokerSiteFrontEnd(ICollection<BrokerPairDTO> brokers,string siteName)
        {
            this.brokersAlive = brokers.ToList();
            this.siteName = siteName;
        }

        private void RemoveCrashedBrokers(string brokerName)
        {
            lock (this)
            {
                foreach (BrokerPairDTO pair in brokersAlive)
                {
                    if (pair.LogicName.Equals(brokerName))
                    {
                        brokersAlive.Remove(pair);
                        break;
                    }
                }
            }
        }

        private BrokerPairDTO[] GetCopy()
        {
            lock (this)
            {
                return brokersAlive.ToArray();
            }
        }


        public void Diffuse(Event e)
        {
            BrokerPairDTO[] brokers = GetCopy();
            foreach (BrokerPairDTO pair in brokers)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), pair.Url) as IBroker).Diffuse(e);
                }
                catch (Exception)
                {
                    RemoveCrashedBrokers(pair.LogicName);
                }
            }
        }

        public void AddRoute(Route route)
        {
            BrokerPairDTO[] brokers = GetCopy();
            foreach (BrokerPairDTO pair in brokers)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), pair.Url) as IBroker).AddRoute(route);
                }
                catch (Exception e)
                {
                    RemoveCrashedBrokers(pair.LogicName);
                    Console.WriteLine(e);
                }
            }
        }

        public void RemoveRoute(Route route)
        {
            BrokerPairDTO[] brokers = GetCopy();
            foreach (BrokerPairDTO pair in brokers)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), pair.Url) as IBroker).RemoveRoute(route);
                }
                catch (Exception)
                {
                    RemoveCrashedBrokers(pair.LogicName);
                }
            }
        }

        public void Subscribe(Subscription subscription)
        {
            BrokerPairDTO[] brokers = GetCopy();
            foreach (BrokerPairDTO pair in brokers)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), pair.Url) as IBroker).Subscribe(subscription);
                }
                catch (Exception)
                {
                    RemoveCrashedBrokers(pair.LogicName);
                }
            }
        }

        public void Unsubscribe(Subscription subscription)
        {
            BrokerPairDTO[] brokers = GetCopy();
            foreach (BrokerPairDTO pair in brokers)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), pair.Url) as IBroker).Unsubscribe(subscription);
                }
                catch (Exception)
                {
                    RemoveCrashedBrokers(pair.LogicName);
                }
            }
        }

        public void Sequence(Bludger bludger)
        {
            BrokerPairDTO[] brokers = GetCopy();
            foreach (BrokerPairDTO pair in brokers)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), pair.Url) as IBroker).Sequence(bludger);
                }
                catch (Exception)
                {
                    RemoveCrashedBrokers(pair.LogicName);
                }
            }
        }

        public void Bludger(Bludger bludger)
        {
            BrokerPairDTO[] brokers = GetCopy();
            foreach (BrokerPairDTO pair in brokers)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), pair.Url) as IBroker).Bludger(bludger);
                }
                catch (Exception)
                {
                    RemoveCrashedBrokers(pair.LogicName);
                }
            }
        }

        public override string ToString()
        {
            lock (this)
            {
                string res = siteName + " :" + Environment.NewLine;
                foreach (BrokerPairDTO dto in brokersAlive)
                {
                    res += dto.ToString() + Environment.NewLine;
                }
                return res;
            }
        }
    }
}
