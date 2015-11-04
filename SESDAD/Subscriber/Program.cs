using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
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
                + nl + "Routing policy: {3}" + nl + "LoggingPolicy: {4}" + nl
                + "PuppetMasterLogService: {5}"+ nl + "Brokers:",
                args[0], args[1], args[2], args[3], args[4], args[5]);
            List<string> brokers = new List<string>(); 
            for (int i = 6; i < args.Length; i = i + 2)
            {
                Console.WriteLine(args[i + 1]);
                brokers.Add(args[i + 1]);
            }

            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["port"] = int.Parse(args[0]);
            TcpChannel channel = new TcpChannel(props, null, provider);
            ChannelServices.RegisterChannel(channel, false);
            SubscriberServer subscriber = new SubscriberServer(args[2],args[1],args[5], 
                args[4], brokers);
            RemotingServices.Marshal(subscriber, "sub", typeof(SubscriberServer));
            Console.WriteLine("Subscriber up and running....");
            Console.ReadLine();
        }
    }
}
