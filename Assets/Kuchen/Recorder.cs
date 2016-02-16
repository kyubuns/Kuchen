using System;
using System.Collections.Generic;

namespace Kuchen
{
	public class Recorder : IDisposable
	{
		private Subscriber subscriber;
		public List<string> Topics { get; private set; }

		public Recorder()
		{
			subscriber = new Subscriber();
			Topics = new List<string>();
			subscriber.Subscribe("*", (topic) => { Topics.Add(topic); });
		}

		public void Dispose()
		{
			subscriber.Dispose();
		}

		public bool Contains(string topic) { return Topics.Contains(topic); }
	}
}
