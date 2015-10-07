using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    class Program
    { 
        // port = args[0], name = args[1]
        static void Main(string[] args)
        {
            if (args[0] == null || args[1] == null)
                return;
            TcpChannel channel = new TcpChannel(int.Parse(args[0]));
            ChannelServices.RegisterChannel(channel, false);
            BrokerServer broker = new BrokerServer();
            RemotingServices.Marshal(broker, args[1], typeof(BrokerServer));
            Console.WriteLine("Broker up and running...");
            Console.ReadLine();
        }
    }
}
