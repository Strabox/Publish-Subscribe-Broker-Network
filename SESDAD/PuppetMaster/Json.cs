using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMaster
{
    
    public class PuppetMasterJson
    {
        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public PuppetMasterJson(string url)
        {
            Url = url;
        }
    }

}
