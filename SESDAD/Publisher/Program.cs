using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 6) {
                Console.Error.WriteLine("Wrong usage.");
                return;                                
            }
            string nl = Environment.NewLine;
            Console.WriteLine("Port: {0}" + nl + "Name: {1}" + nl + "OrderingPolicy: {2}"
                + nl + "Routing policy: {3}" + nl + "LoggingPolicy: {4}" + nl
                + "PuppetMasterLogService: {5}",
                args[0], args[1], args[2], args[3], args[4], args[5]);
            Console.WriteLine("Brokers: ");
            List<string> brokers = new List<string>();
            for (int i = 6; i < args.Length; i = i +2)
            {
                brokers.Add(args[i + 1]);
                Console.WriteLine(args[i + 1]);
            }

            TcpChannel channel = new TcpChannel(int.Parse(args[0]));
            ChannelServices.RegisterChannel(channel, false);
            PublisherServer publisher = new PublisherServer(args[1],args[5],args[4],args[2],brokers);
            RemotingServices.Marshal(publisher, "pub", typeof(PublisherServer));
            Console.WriteLine("Publisher up and running....");
            Console.ReadLine();
        }
    }
}
