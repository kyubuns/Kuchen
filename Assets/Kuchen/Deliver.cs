using System;
using System.Collections.Generic;

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
		
		private List<Subscriber> subscribers = new List<Subscriber>();
		private List<Action> bookedEvents;
		
		public int Publish(string topic, object[] args)
		{
			int listener = 0;
			bool rootEvent = false;
			if(bookedEvents == null)
			{
				rootEvent = true;
				bookedEvents = new List<Action>();
			}
			foreach(var subscriber in subscribers) listener += subscriber.Call(topic, args);
			if(rootEvent)
			{
				var copied = bookedEvents.ToArray();
				bookedEvents = null;
				foreach(var e in copied) e();
			}
			return listener;
		}
		
		public void Subscribe(Subscriber subscriber)
		{
			if(bookedEvents != null) bookedEvents.Add(() => { Subscribe(subscriber); });
			else subscribers.Add(subscriber);
		}
		
		public void Unsubscribe(Subscriber subscriber)
		{
			if(bookedEvents != null) bookedEvents.Add(() => { Unsubscribe(subscriber); });
			else subscribers.Remove(subscriber);
		}
	}
}