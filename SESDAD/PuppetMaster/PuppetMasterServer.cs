using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMaster
{
    public class PuppetMasterServer : MarshalByRefObject, IPuppetMaster
    {
        //TODO

        /*As far as I know this is necessary to override the
        .NET lease times. */
        public override object InitializeLifetimeService()
        {
            return null;
        }

    }
}
