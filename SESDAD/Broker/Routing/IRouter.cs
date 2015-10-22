using CommonTypes;

namespace Broker
{
	public interface IRouter
	{
		Event Diffuse(Event e);
		void Subscribe(Subscription subscription);
		void Unsubscribe(Subscription subscription);
	}
}