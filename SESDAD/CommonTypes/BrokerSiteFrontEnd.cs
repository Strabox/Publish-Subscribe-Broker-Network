using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace CommonTypes
{   

    /// <summary>
    ///     BrokerSite hides the replication in a site making the calls transparent.
    /// </summary>
    public class BrokerSiteFrontEnd : IBroker
    {
        private string siteName;

        private List<BrokerPairDTO> brokers;

        private int primaryIndex = 0;

        public BrokerSiteFrontEnd(ICollection<BrokerPairDTO> brokers,string siteName)
        {
            this.brokers = brokers.ToList();
            this.siteName = siteName;
        }


        public void Diffuse(Event e)
        {
            for (; primaryIndex < brokers.Count; primaryIndex++)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), brokers[primaryIndex].Url) as IBroker).Diffuse(e);
                }
                catch (Exception)
                {
                    Console.WriteLine("Diffuse Failed: Trying next broker...");
                    continue;
                }
                Console.WriteLine("Diffuse Success");
                return;
            }
            Console.WriteLine("Diffuse Failed: No more brokers system is now broken pray for the new gods and the old ones....");
        }

        public void AddRoute(Route route)
        {
            for (; primaryIndex < brokers.Count; primaryIndex++)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), brokers[primaryIndex].Url) as IBroker).AddRoute(route);
                }
                catch (Exception)
                {
                    //Do nothing
                }
            }
        }

        public void RemoveRoute(Route route)
        {
            for (; primaryIndex < brokers.Count; primaryIndex++)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), brokers[primaryIndex].Url) as IBroker).RemoveRoute(route);
                }
                catch (Exception)
                {
                    //Do nothing
                }
            }
        }

        public void Subscribe(Subscription subscription)
        {
            for (; primaryIndex < brokers.Count; primaryIndex++)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), brokers[primaryIndex].Url) as IBroker).Subscribe(subscription);
                }
                catch (Exception)
                {
                   //Do nothing
                }
            }
        }

        public void Unsubscribe(Subscription subscription)
        {
            for (; primaryIndex < brokers.Count; primaryIndex++)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), brokers[primaryIndex].Url) as IBroker).Unsubscribe(subscription);
                }
                catch (Exception)
                {
                    //Do nothing
                }
            }
        }

        public void Sequence(Bludger bludger)
        {
            for (; primaryIndex < brokers.Count; primaryIndex++)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), brokers[primaryIndex].Url) as IBroker).Sequence(bludger);
                }
                catch (Exception)
                {
                    continue;
                }
                return;
            }
        }

        public void Bludger(Bludger bludger)
        {
            for (; primaryIndex < brokers.Count; primaryIndex++)
            {
                try
                {
                    (Activator.GetObject(typeof(IBroker), brokers[primaryIndex].Url) as IBroker).Bludger(bludger);
                }
                catch (Exception)
                {
                    continue;
                }
                return;
            }
        }

        public override string ToString()
        {
            string res = siteName + " :" + Environment.NewLine;
            foreach (BrokerPairDTO dto in brokers)
            {
                res += dto.ToString() + Environment.NewLine;
            }
            return res;
        }
    }
}
