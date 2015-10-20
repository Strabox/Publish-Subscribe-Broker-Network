using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMaster
{

    public class ManageSites
    {

        class Site
        {
            private string name;

            private Site parent = null;

            private List<Site> child;

            //List of broker's URL from this site.
            private List<string> brokersUrls;

            public bool IsRoot
            {
                get
                { return parent == null; }
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
                brokersUrls = new List<string>();
            }

            public void AddChildrenSite(Site site)
            {
                child.Add(site);
            }

            public void AddBrokerUrlToSite(string url)
            {
                brokersUrls.Add(url);
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
                string[] res = new string[brokersUrls.Count];
                brokersUrls.CopyTo(res);
                return string.Join(" ", res);
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

        public void AddBrokerUrlToSite(string siteName, string brokerUrl)
        {
            if (sites.ContainsKey(siteName))
                sites[siteName].AddBrokerUrlToSite(brokerUrl);
        }

        public bool IsSiteRoot(string siteName)
        {
            if (sites.ContainsKey(siteName))
                return sites[siteName].IsRoot;
            return false;
        }

    }



}

