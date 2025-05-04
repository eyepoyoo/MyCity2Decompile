using UnityEngine;

namespace VacuumShaders.CurvedWorld.Demo
{
	[AddComponentMenu("VacuumShaders/Curved World/Demo/On Start")]
	public class CW_Demo_OnStart : MonoBehaviour
	{
		public bool enableOnStart;

		public bool disableOnStart;

		private void Start()
		{
			if (enableOnStart)
			{
				base.gameObject.SetActive(true);
			}
			if (disableOnStart)
			{
				base.gameObject.SetActive(false);
			}
		}
	}
}
