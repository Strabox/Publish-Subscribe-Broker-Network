using CommonTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMasterProcessLauncher
{
    public class ProcessLauncherServer : MarshalByRefObject, IPuppetMasterLauncher
    {

        public void LaunchProcess(string name, string args)
        {
            Process.Start(CommonConstants.PROJECT_ROOT + name +
                CommonConstants.EXE_PATH + name, args);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

    }
}
