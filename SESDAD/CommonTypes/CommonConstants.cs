using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public class CommonConstants
    {
        public static string PROJECT_ROOT = ".." +
                            Path.DirectorySeparatorChar + ".." +
                            Path.DirectorySeparatorChar + ".." +
                            Path.DirectorySeparatorChar;
        public static string EXE_PATH = Path.DirectorySeparatorChar +
                            "bin" + Path.DirectorySeparatorChar +
                            "Debug" + Path.DirectorySeparatorChar;

        public static int PUPPET_MASTER_PORT = 6969;

        public static string PUPPET_MASTER_NAME = "PuppetMasterURL";
    }
}
