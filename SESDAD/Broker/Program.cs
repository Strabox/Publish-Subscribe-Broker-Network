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
        
        static void Main(string[] args)
        {
            if (args.Length < 5) {
                Console.Error.WriteLine("Wrong usage.");
                return;                                
            }
            string nl = Environment.NewLine;
            Console.WriteLine("Port: {0}" + nl + "Name: {1}" + nl + "OrderingPolicy: {2}"
                + nl + "Routing policy: {3}" + nl + "LoggingPolicy: {4}"+ nl
                + "PuppetMasterLogService: {5}",
                args[0], args[1], args[2], args[3], args[4],args[5]);
            Console.WriteLine("Brokers parents and children:");
            for (int i = 6; i < args.Length; i++)
                Console.WriteLine(args[i]);

            TcpChannel channel = new TcpChannel(int.Parse(args[0]));
            ChannelServices.RegisterChannel(channel, false);
            //TODO detect and distinguish parents and children, and pass them to the server.
            BrokerServer broker = new BrokerServer(args[2], args[3], args[4], args[5], args[6], args.Skip(7).ToArray());
            RemotingServices.Marshal(broker, args[1], typeof(BrokerServer));
            Console.WriteLine("Broker up and running...");
            Console.ReadLine();
        }
    }
}
