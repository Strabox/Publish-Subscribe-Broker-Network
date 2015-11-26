using CommonTypes;

namespace Broker
{
	public interface IRouter
	{
		Event Diffuse(Event e);
        void DiffuseBludger(Bludger b);
		void Subscribe(Subscription subscription);
		void Unsubscribe(Subscription subscription);
		void AddRoute(Route route);
		void RemoveRoute(Route route);
	}
}