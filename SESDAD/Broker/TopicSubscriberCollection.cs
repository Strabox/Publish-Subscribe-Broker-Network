using System;
using System.Collections.Generic;

namespace Broker {
	class TopicSubscriberCollection {
		
		public void Add(string topic, string subscriber)
		{
			
		}
		
		public void Remove(string topic, string subscriber)
		{
			
		}
		
		public ICollection<string> SubscribersForTopic(string topic)
		{
			return new HashSet<String>();
		}
		
	}
}