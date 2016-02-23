using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kuchen.Util
{
	public static class Coroutine
	{
		public static IEnumerator WaitForMessage(this Subscriber subscriber, string[] topics, float timeout = 0.0f)
		{
			var called = false;
			var startTime = Time.time;
			var se = subscriber.Subscribe(topics, () => { called = true; });
			while(!called && (timeout <= 0.0f || Time.time < startTime + timeout)) yield return null;
			subscriber.Unsubscribe(se);
		}
	}
	
	public static class PatternMatch
	{
		public static bool IsMatch(string mask, string full)
		{
			return Tonari.Text.MiniRegex.IsMatch(full, mask);
		}
	}	
}