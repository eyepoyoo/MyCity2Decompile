using UnityEngine;

namespace VacuumShaders.CurvedWorld.Water
{
	[ExecuteInEditMode]
	[AddComponentMenu("VacuumShaders/Curved World/Water/WaterTile")]
	public class WaterTile : MonoBehaviour
	{
		public WaterBase waterBase;

		public void Start()
		{
			AcquireComponents();
		}

		private void AcquireComponents()
		{
			if (!waterBase)
			{
				if ((bool)base.transform.parent)
				{
					waterBase = base.transform.parent.GetComponent<WaterBase>();
				}
				else
				{
					waterBase = base.transform.GetComponent<WaterBase>();
				}
			}
		}

		public void OnWillRenderObject()
		{
			if ((bool)waterBase)
			{
				waterBase.WaterTileBeingRendered(base.transform, Camera.current);
			}
		}
	}
}
