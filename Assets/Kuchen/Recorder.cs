using System;
using System.Linq;
using System.Collections.Generic;

namespace Kuchen
{
	public class Recorder : IDisposable
	{
		private Subscriber subscriber;
		public List<Tuple<string, object, object, object>> Topics { get; private set; }

		public Recorder()
		{
			subscriber = new Subscriber();
			Topics = new List<Tuple<string, object, object, object>>();
			subscriber.SubscribeWithTopic<object, object, object>("*", Record);
		}
		
		private void Record(string topic, object arg1, object arg2, object arg3)
		{
			Topics.Add(new Tuple<string, object, object, object>(topic, arg1, arg2, arg3));
		}

		public void Dispose()
		{
			subscriber.Dispose();
		}

		public bool Contains(string topic) { return Topics.Any(x => x.Item1 == topic); }
		public Tuple<string, object, object, object> Find(string topic) { return Topics.Find(x => x.Item1 == topic); }
		public Tuple<string, T1> Find<T1>(string topic)
		{
			var t = Find(topic);
			return new Tuple<string, T1>(t.Item1, (T1)t.Item2);
		}
		public Tuple<string, T1, T2> Find<T1, T2>(string topic)
		{
			var t = Find(topic);
			return new Tuple<string, T1, T2>(t.Item1, (T1)t.Item2, (T2)t.Item3);
		}
		public Tuple<string, T1, T2, T3> Find<T1, T2, T3>(string topic)
		{
			var t = Find(topic);
			return new Tuple<string, T1, T2, T3>(t.Item1, (T1)t.Item2, (T2)t.Item3, (T3)t.Item4);
		}
		
		public class Tuple<T1>
		{
			public Tuple(T1 item1) { this.Item1 = item1; }
			public T1 Item1;
		}
		
		public class Tuple<T1, T2>
		{
			public Tuple(T1 item1, T2 item2) { this.Item1 = item1; this.Item2 = item2; }
			public T1 Item1;
			public T2 Item2;
		}
		
		public class Tuple<T1, T2, T3>
		{
			public Tuple(T1 item1, T2 item2, T3 item3) { this.Item1 = item1; this.Item2 = item2; this.Item3 = item3; }
			public T1 Item1;
			public T2 Item2;
			public T3 Item3;
		}
		
		public class Tuple<T1, T2, T3, T4>
		{
			public Tuple(T1 item1, T2 item2, T3 item3, T4 item4) { this.Item1 = item1; this.Item2 = item2; this.Item3 = item3; this.Item4 = item4; }
			public T1 Item1;
			public T2 Item2;
			public T3 Item3;
			public T4 Item4;
		}
	}
}
