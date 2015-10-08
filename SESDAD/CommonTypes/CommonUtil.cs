using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public static class CommonUtil
    {

        public static int PUPPET_MASTER_PORT = 9999;

        public static string PUPPET_MASTER_NAME = "PuppetMaster";

        public static string PUPPET_MASTER_URL = "tcp://localhost:"
                                               + PUPPET_MASTER_PORT + "/"
                                               + PUPPET_MASTER_NAME;

    }
}
