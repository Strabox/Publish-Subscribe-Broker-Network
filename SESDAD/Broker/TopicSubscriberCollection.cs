using CommonTypes;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Broker
{
	using SubscriberPair = NodePair<ISubscriber>;
	using BrokerPair = NodePair<IBroker>;
	
    public class NodePair<T>
    {
		private T node;
		public T Node
		{
			get { return node; }
			set { node = value; }
		}
		
		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		
		public NodePair(string name, T node)
		{
			this.name = name;
			this.node = node;
		}
		
		public bool Is(string name)
		{
			return this.name.Equals(name);
		}

        public override bool Equals(Object other)
        {
            NodePair<T> o = other as NodePair<T>;
            return o != null && o.Name.Equals(this.name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() ^ this.Node.GetHashCode();
        }
    }

    public class TopicSubscriberCollection 
	{
		/**
		*	This static class contains helper methods for handling string topics
		*/
		static class Topic
		{
			public static string[] Split(string topic)
			{
				return topic.Split('/').Skip(1).ToArray();
			}
			
			public static bool IsWildcard(string subtopic)
			{
				return subtopic == "*";
			}
		}
		
		class Router<T>
		{
			public string topic;
			public ICollection<T> nodes;
			public ICollection<T> nodesAsterisk;
			public IDictionary<string, Router<T>> subtopics;
			
			public Router(string topic, ICollection<T> nodes, ICollection<T> nodesAsterisk, IDictionary<string, Router<T>> subtopics)
			{
				this.topic = topic;
				this.nodes = nodes;
				this.nodesAsterisk = nodesAsterisk;
				this.subtopics = subtopics;
			}
			
			public static Router<T> ForTopic(string topic)
			{
				return new Router<T>(topic, new HashSet<T>(), new HashSet<T>(), new Dictionary<string, Router<T>>());
			}
			
			public bool HasSubtopic(string subtopic)
			{
				return this.subtopics.ContainsKey(subtopic);
			}
			
			public void Add(string topic, T node)
			{
				string[] parts = Topic.Split(topic);
				var current = this;
				
				string currentTopic = "";
				
				foreach (var part in parts)
				{
					currentTopic += "/" + part;
	
					if (Topic.IsWildcard(part))
					{
						current.nodesAsterisk.Add(node);
						return;
					}
	
					if ( ! current.HasSubtopic(part))
					{
						current.subtopics.Add(part, Router<T>.ForTopic(currentTopic));
					}
					
					current = current.subtopics[part];				
				}
				
				current.nodes.Add(node);
			}
			
			public void Remove(string topic, T node)
			{
				string[] parts = Topic.Split(topic);
				var current = this;
				
				foreach (var part in parts)
				{
					if (Topic.IsWildcard(part))
					{
						current.nodesAsterisk.Remove(node);
					}
					
					if ( ! current.HasSubtopic(part))
					{
						// No subscriber has subscribed the topic
						// Still, that's weird.
						return;
					}
					
					current = current.subtopics[part];				
				}
				
				current.nodes.Remove(node);
			}
			
			public ICollection<T> NodesFor(string topic)
			{
				string[] parts = Topic.Split(topic);
				var current = this;
				HashSet<T> nodes = new HashSet<T>();
				
				foreach (var part in parts)
				{
					nodes.UnionWith(current.nodesAsterisk);
					
					if ( ! current.HasSubtopic(part))
					{
						return nodes;
					}
					
					current = current.subtopics[part];
				}
				
				nodes.UnionWith(current.nodes);
	
				return nodes;
			}
			
			public bool HasNodesFor(string topic)
			{
				string[] parts = Topic.Split(topic);
				var current = this;
				
				foreach (var part in parts)
				{
					if (current.nodesAsterisk.Count > 0)
					{
						return true;
					}
					
					if ( ! current.HasSubtopic(part))
					{
						return false;
					}
					
					current = current.subtopics[part];
				}
				
				return current.nodes.Count > 0;
			}
			
			public Dictionary<string, ICollection<T>> List()
			{
				return ListRecursive(new Dictionary<string, ICollection<T>>(), this);
			}
			
			private Dictionary<string, ICollection<T>> ListRecursive(Dictionary<string, ICollection<T>> result, Router<T> current)
			{
				if (current.nodes.Count > 0)
				{
					result.Add(current.topic, current.nodes);
				}
				
				if (current.nodesAsterisk.Count > 0)
				{
					string asterisk = current.topic.Equals("/") ? "*" : "/*";
					result.Add(current.topic + asterisk, current.nodesAsterisk);
				}
				
				foreach (var subtopic in current.subtopics)
				{
					ListRecursive(result, subtopic.Value);
				}
				
				return result;
			}
		}
		
		private Router<SubscriberPair> subscribers;
		private Router<BrokerPair> brokers;
		
		public TopicSubscriberCollection()
		{
			this.subscribers = Router<SubscriberPair>.ForTopic("/");
			this.brokers = Router<BrokerPair>.ForTopic("/");
		}
		
		/**
		 * Returns true if this node gains interest in the topic. Returns false otherwise.
		 */
		public bool AddSubscriber(string topic, string name, ISubscriber subscriber)
		{
			lock (this)
			{
				bool result = ! this.subscribers.HasNodesFor(topic);
                Console.WriteLine("Interested: " + result);
				this.subscribers.Add(topic, new SubscriberPair(name, subscriber));
				return result;
			}			
		}
		
		/**
		 * Returns true if this node loses all interest in the topic. Returns false otherwise.
		 */
		public bool RemoveSubscriber(string topic, string name, ISubscriber subscriber)
		{
			lock (this)
			{
				this.subscribers.Remove(topic, new SubscriberPair(name, subscriber));
				return ( ! this.brokers.HasNodesFor(topic)) && ( ! this.subscribers.HasNodesFor(topic));	
			}		
		}
		
		public ICollection<SubscriberPair> SubscribersFor(string topic)
		{
			lock (this)
			{
				return this.subscribers.NodesFor(topic);
			}
		}
		
		public Dictionary<string, ICollection<SubscriberPair>> SubscribersByTopic()
		{
			return this.subscribers.List();
		}
		
		/**
		 * Returns true if this node gains interest in the topic. Returns false otherwise.
		 */
		public bool AddRoute(string topic, string name, IBroker broker)
		{
			lock (this)
			{
                foreach (var pair in this.brokers.NodesFor(topic))
                {
                    if (pair.Name.Equals(name))
                    {
                        return false;
                    }
                }

                this.brokers.Add(topic, new BrokerPair(name, broker));

                return true;
			}
		}
		
		/**
		 * Returns true if this node loses all interest in the topic. Returns false otherwise.
		 */
		public bool RemoveRoute(string topic, string name, IBroker broker)
		{
			lock (this)
			{
				this.brokers.Remove(topic, new BrokerPair(name, broker));
				return ( ! this.brokers.HasNodesFor(topic)) && ( ! this.subscribers.HasNodesFor(topic));
			}			
		}
		
		public ICollection<BrokerPair> RoutingFor(string topic)
		{
			lock (this)
			{
				return this.brokers.NodesFor(topic);
			}
		}
		
		public Dictionary<string, ICollection<BrokerPair>> RoutesByTopic()
		{
			return this.brokers.List();
		}
		
	}
}