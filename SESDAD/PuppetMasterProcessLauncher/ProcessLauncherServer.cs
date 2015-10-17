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
            if(CommonUtil.IsLinux)
                Process.Start("mono",
                string.Join(" ", CommonUtil.PROJECT_ROOT + name +
                CommonUtil.EXE_PATH + name+".exe", args));
            else
                Process.Start(CommonUtil.PROJECT_ROOT + name +
                CommonUtil.EXE_PATH + name, args);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

    }
}
