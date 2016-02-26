namespace Kuchen
{
	public class SubscribeEventChain
	{
		public SubscribeEventChain(Subscriber subscriber, ISubscribeEvent subscribeEvent)
		{
			this.Subscriber = subscriber;
			this.Event = subscribeEvent;
		}
		
		public Subscriber Subscriber;
		public ISubscribeEvent Event;
	}
}