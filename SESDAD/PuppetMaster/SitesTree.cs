using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMaster
{

    public class ManageSites
    {

        private class Broker
        {
            private string url;
            public string Url { get { return url; } }

            private string logicName;
            public string LogicName { get { return logicName; } }

            public Broker(string url,string logicName)
            {
                this.url = url;
                this.logicName = logicName;
            }
        }

        private class Site
        {
            private string name;

            private Site parent = null;

            private List<Site> child;

            //List of broker's URL from this site.
            private List<Broker> brokersUrls;

            public bool IsRoot
            {
                get{ return parent == null; }
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public Site Parent
            {
                get { return parent; }
                set { parent = value; }
            }

            public Site(string name)
            {
                this.Name = name;
                child = new List<Site>();
                brokersUrls = new List<Broker>();
            }

            public void AddChildrenSite(Site site)
            {
                child.Add(site);
            }

            public void AddBrokerUrlToSite(string url,string logicName)
            {
                brokersUrls.Add(new Broker(url,logicName));
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
                string res2 = "";
                foreach(Broker broker in brokersUrls)
                {
                    res2 += broker.LogicName + " " + broker.Url + " ";
                }
                return res2.Substring(0, res2.Length - 1);
            }

        }

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
                sites[siteName].AddBrokerUrlToSite(brokerUrl,brokerLogicName);
        }

        public bool IsSiteRoot(string siteName)
        {
            if (sites.ContainsKey(siteName))
                return sites[siteName].IsRoot;
            return false;
        }

    }



}

