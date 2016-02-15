namespace Kuchen
{
	public class Deliver
	{
		private static Deliver instance = null;
		public static Deliver Instance
		{
			get
			{
				if(instance == null) instance = new Deliver();
				return instance;
			}
		}
		
		public void Subscribe(Subscriber subscriber)
		{
			// TODO
		}
		
		public void Unsubscribe(Subscriber subscriber)
		{
			// TODO
		}
	}
}