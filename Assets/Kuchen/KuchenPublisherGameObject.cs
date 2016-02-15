using UnityEngine;

namespace Kuchen
{
	[DisallowMultipleComponent]
	public class KuchenPublisherGameObject : MonoBehaviour
	{
		public void Publish(string topic)
		{
			Publisher.Publish(topic);
		}
	}
}