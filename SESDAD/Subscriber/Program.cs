using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == null || args[1] == null)
                return;
            TcpChannel channel = new TcpChannel(int.Parse(args[0]));
            ChannelServices.RegisterChannel(channel, false);
            SubscriberServer subscriber = new SubscriberServer();
            RemotingServices.Marshal(subscriber, args[1], typeof(SubscriberServer));
            Console.WriteLine("Subscriber up and running....");
            Console.ReadLine();
        }
    }
}
