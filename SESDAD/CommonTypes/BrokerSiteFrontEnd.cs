using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonTypes
{   

    /// <summary>
    ///     BrokerSite hides the replication in a site making the calls transparent.
    /// </summary>
    public class BrokerSiteFrontEnd : IBroker
    {
        private string siteName;

        private List<BrokerPairDTO> brokers;

        public BrokerSiteFrontEnd(ICollection<BrokerPairDTO> brokers,string siteName)
        {
            this.brokers = brokers.ToList();
            this.siteName = siteName;
        }


        public void Diffuse(Event e)
        {
            /*
            foreach(BrokerPairDTO broker in brokers)
            {
                (Activator.GetObject(typeof(IBroker), broker.Url) as IBroker).Diffuse(e);
            }
            */
            if (brokers.Count > 0)      //Call the first only for now.
                (Activator.GetObject(typeof(IBroker), brokers[0].Url) as IBroker).Diffuse(e);
        }

        public void AddRoute(Route route)
        {
            /*
            foreach (BrokerPairDTO broker in brokers)
            {
                (Activator.GetObject(typeof(IBroker), broker.Url) as IBroker).AddRoute(route);
            }
            */
            if (brokers.Count > 0)      //Call the first only for now.
                (Activator.GetObject(typeof(IBroker), brokers[0].Url) as IBroker).AddRoute(route);
        }

        public void RemoveRoute(Route route)
        {
            /*
            foreach (BrokerPairDTO broker in brokers)
            {
                (Activator.GetObject(typeof(IBroker), broker.Url) as IBroker).RemoveRoute(route);
            }
            */
            if (brokers.Count > 0)      //Call the first only for now.
                (Activator.GetObject(typeof(IBroker), brokers[0].Url) as IBroker).RemoveRoute(route);
        }

        public void Subscribe(Subscription subscription)
        {
            /*
            foreach (BrokerPairDTO broker in brokers)
            {
                (Activator.GetObject(typeof(IBroker), broker.Url) as IBroker).Subscribe(subscription);
            }
            */
            if (brokers.Count > 0)      //Call the first only for now.
                (Activator.GetObject(typeof(IBroker), brokers[0].Url) as IBroker).Subscribe(subscription);
        }

        public void Unsubscribe(Subscription subscription)
        {
            /*
            foreach (BrokerPairDTO broker in brokers)
            {
                (Activator.GetObject(typeof(IBroker), broker.Url) as IBroker).Unsubscribe(subscription);
            }
            */
            if (brokers.Count > 0)      //Call the first only for now.
                (Activator.GetObject(typeof(IBroker), brokers[0].Url) as IBroker).Unsubscribe(subscription);
        }

        public override string ToString()
        {
            string res = siteName + " :" + Environment.NewLine;
            foreach(BrokerPairDTO dto in brokers)
            {
                res += dto.ToString() + Environment.NewLine;
            }
            return res;
        }

    }
}
