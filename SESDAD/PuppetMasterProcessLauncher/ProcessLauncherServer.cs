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
            if (args == null || name == null)
                return;
            if (Util.IsLinux)
                Process.Start("mono",
                string.Join(" ", Util.PROJECT_ROOT + name +
                Util.EXE_PATH + name + ".exe", args));
            else
                Process.Start(Util.PROJECT_ROOT + name +
                Util.EXE_PATH + name, args);
            string[] argv = args.Split(' ');
            Console.WriteLine("{0} {1} launched..", name, argv[1]);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

    }
}
