using System;
using LEGO.CoreSDK.Unity;
using UnityEngine;

namespace LEGO.CoreSDK.VPC.Unity
{
	// Token: 0x02000002 RID: 2
	public class ParentalGateView : IParentalGateView
	{
		// Token: 0x06000002 RID: 2 RVA: 0x000020F4 File Offset: 0x000002F4
		public void Show(ParentalGateModel model)
		{
			if (this.behaviour != null)
			{
				this.Cancel();
			}
			GameObject gameObject = Resources.Load("ParentalGate/ParentalGate") as GameObject;
			if (gameObject == null)
			{
				throw new UnityPrefabNotFoundException("Failed to find the Parental Gate prefab in the project");
			}
			GameObject gameObject2 = global::UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject;
			this.behaviour = gameObject2.GetComponent<ParentalGateBehaviour>();
			if (this.behaviour == null)
			{
				throw new UnityNoComponentException("Failed to find the Parental Gate Component on the Parental Gate prefab");
			}
			this.behaviour.ParentalGate = new ParentalGateModel?(model);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002190 File Offset: 0x00000390
		public void Cancel()
		{
			this.behaviour.DidTapCancel();
			this.behaviour = null;
		}

		// Token: 0x04000001 RID: 1
		private ParentalGateBehaviour behaviour;
	}
}
