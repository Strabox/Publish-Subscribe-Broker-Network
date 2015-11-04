using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Broker
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
                + "PuppetMasterLogService: {5}" + nl + "ParentName: {6} ParentUrl: {7}",
                args[0], args[1], args[2], args[3], args[4],args[5],args[6],args[7]);
            Console.WriteLine("Broker's Children:");
            List<BrokerPairDTO> childs = new List<BrokerPairDTO>();
            for (int i = 8; i < args.Length; i = i + 2)
            {
                childs.Add(new BrokerPairDTO(args[i + 1], args[i]));
                Console.WriteLine("Broker: {0} Url: {1}",args[i],args[i + 1]);
            }
      
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["port"] = int.Parse(args[0]);
            TcpChannel channel = new TcpChannel(props, null, provider);
            ChannelServices.RegisterChannel(channel, false);
            BrokerServer broker = new BrokerServer(args[1],args[2], args[3], args[4], args[5],
                 args[6],args[7], childs);
            RemotingServices.Marshal(broker, "broker", typeof(BrokerServer));
            Console.WriteLine("Broker up and running...");
            Console.ReadLine();
        }
    }
}
