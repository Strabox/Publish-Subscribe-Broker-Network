using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMasterProcessLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessLauncherServer server = new ProcessLauncherServer();
            TcpChannel channel = new TcpChannel(CommonConstants.PUPPET_MASTER_PORT);
            ChannelServices.RegisterChannel(channel, false);
            RemotingServices.Marshal(server, CommonConstants.PUPPET_MASTER_NAME,
                typeof(ProcessLauncherServer));
            Console.WriteLine("Waiting to launch processes........");
            Console.ReadLine();
        }
    }
}
