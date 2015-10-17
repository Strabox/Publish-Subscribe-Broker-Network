using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMaster
{
  
    public class ManageSites
    {
        //pair <site's name, site object>
        private Dictionary<string, Site> sites;

        public ManageSites()
        {
            sites = new Dictionary<string, Site>();
        }

        public Site CreateSite(string name)
        {
            if (!sites.ContainsKey(name))
                sites.Add(name, new Site(name));
            return sites[name];
        }

        public Site GetSiteByName(string name)
        {
            return sites[name];
        }

    }

    public class Site
    {
        private string name;

        private Site parent = null;

        private List<Site> child;

        //List of broker's URL from this site.
        private List<string> brokersUrls;

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
 
}
