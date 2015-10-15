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
        private static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        public void LaunchProcess(string name, string args)
        {
            if(IsLinux)
                Process.Start("mono",
                string.Join(" ", CommonConstants.PROJECT_ROOT + name +
                CommonConstants.EXE_PATH + name, args));
            else
                Process.Start(CommonConstants.PROJECT_ROOT + name +
                CommonConstants.EXE_PATH + name, args);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

    }
}
