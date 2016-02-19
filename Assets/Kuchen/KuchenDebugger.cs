using UnityEngine;

namespace Kuchen
{
	public class KuchenDebugger : MonoBehaviour
	{
		public void Start()
		{
			this.SubscribeWithTopic("*", (string topic, object arg1, object arg2, object arg3) => {
				if(arg1 == null) Debug.LogFormat("[KuchenDebugger] Topic:{0}", topic);
				else if(arg2 == null) Debug.LogFormat("[KuchenDebugger] Topic:{0} | Arg1:{1}", topic, arg1);
				else if(arg3 == null) Debug.LogFormat("[KuchenDebugger] Topic:{0} | Arg1:{1} Arg2:{2}", topic, arg1, arg2);
				else Debug.LogFormat("[KuchenDebugger] Topic:{0} | Arg1:{1} Arg2:{2} Arg3:{3}", topic, arg1, arg2, arg3);
			});
		}
		
		public void OnEnable()
		{
			this.Unmute("*");
		}
		
		public void OnDisable()
		{
			this.Mute("*");
		}
	}
}
