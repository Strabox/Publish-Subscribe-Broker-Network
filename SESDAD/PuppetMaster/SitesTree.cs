using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PuppetMaster
{

    public class Broker
    {
        public string Url { get; set; }

        public string LogicName { get; set; }

        public Broker() { }

        public Broker(string url, string logicName)
        {
            Url = url;
            LogicName = logicName;
        }

        public BrokerPairDTO GetBrokerDTO()
        {
            return new BrokerPairDTO(Url, LogicName);
        }

    }

    /// <summary>
    ///     A site represents an set of brokers that work as a single one.
    /// </summary>
    public class Site
    {
        public string Name { get; set; }

        private Site parent = null;

        // List of site child sites.
        private List<Site> child;

        //List of broker's URL from this site.
        private List<Broker> brokers;

        public bool IsRoot { get { return parent == null; } }

        public Site() { }

        public Site Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Site(string name)
        {
            this.Name = name;
            child = new List<Site>();
            brokers = new List<Broker>();
        }

        private List<BrokerPairDTO> GetSiteBrokersDTO()
        {
            List<BrokerPairDTO> brokersList = new List<BrokerPairDTO>();
            foreach(Broker broker in brokers)
            {
                brokersList.Add(broker.GetBrokerDTO());
            }
            return brokersList;
        }

        private List<BrokerPairDTO> GetParentBrokersDTO()
        {
            if (!IsRoot)
                return parent.GetSiteBrokersDTO();
            return null;
        }
     
        public SiteDTO GetSiteDTO()
        {
            SiteDTO.SiteBrokers parentSite = null;
            if (!IsRoot)
                parentSite = new SiteDTO.SiteBrokers(parent.Name, parent.GetSiteBrokersDTO());
            SiteDTO dto = new SiteDTO(Name, IsRoot, parentSite,GetSiteBrokersDTO());
            foreach(Site site in child)
            {
                dto.AddSiteChild(site.Name, site.GetSiteBrokersDTO());
            }
            return dto;
        }

        public void AddChildrenSite(Site site)
        {
            child.Add(site);
        }

        public void AddBrokerToSite(string url, string logicName)
        {
            brokers.Add(new Broker(url, logicName));
        }

        public string GetChildUrl()
        {
            string res = string.Empty;
            foreach (Site site in child)
            {
                res = string.Join(" ", res, site.GetBrokersUrl());
            }
            return res;
        }

        public string GetBrokersUrl()
        {
            string res = "";
            foreach (Broker broker in brokers)
            {
                res += broker.LogicName + " " + broker.Url + " ";
            }
            return res.Substring(0, res.Length - 1);
        }
    }

    /// <summary>
    ///     Manage network sites.
    /// </summary>
    public class ManageSites
    {
        //pair <site's name, site object>
        private Dictionary<string, Site> sites;

        public ManageSites()
        {
            sites = new Dictionary<string, Site>();
        }


        public void CreateSite(string siteName)
        {
            if (!sites.ContainsKey(siteName))
                sites.Add(siteName, new Site(siteName));
        }

        public void CreateSiteWithParent(string childSiteName,
            string parentSiteName)
        {
            CreateSite(childSiteName);
            CreateSite(parentSiteName);
            sites[parentSiteName].AddChildrenSite(sites[childSiteName]);
            sites[childSiteName].Parent = sites[parentSiteName];
        }

        public string GetSiteBrokersUrl(string siteName)
        {
            return sites[siteName].GetBrokersUrl();
        }

        public string GetParentBrokersUrl(string siteName)
        {
            return sites[siteName].Parent.GetBrokersUrl();
        }

        public string GetChildrenUrl(string siteName)
        {
            return sites[siteName].GetChildUrl();
        }

        public void AddBrokerUrlToSite(string siteName, string brokerUrl,string brokerLogicName)
        {
            if (sites.ContainsKey(siteName))
                sites[siteName].AddBrokerToSite(brokerUrl,brokerLogicName);
        }

        public bool IsSiteRoot(string siteName)
        {
            if (sites.ContainsKey(siteName))
                return sites[siteName].IsRoot;
            return false;
        }

        public Site GetSite(string siteName)
        {
            if (sites[siteName] != null)
                return sites[siteName];
            else
                return null;
        }

    }

}

