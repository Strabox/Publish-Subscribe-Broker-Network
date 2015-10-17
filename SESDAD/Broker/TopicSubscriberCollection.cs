using CommonTypes;
using System;
using System.Collections.Generic;

namespace Broker {
	class TopicSubscriberCollection {
		
		public void Add(string topic, ISubscriber subscriber)
		{
			
		}
		
		public void Remove(string topic, ISubscriber subscriber)
		{
			
		}
		
		public ICollection<ISubscriber> SubscribersForTopic(string topic)
		{
			return new HashSet<ISubscriber>();
		}
		
	}
}