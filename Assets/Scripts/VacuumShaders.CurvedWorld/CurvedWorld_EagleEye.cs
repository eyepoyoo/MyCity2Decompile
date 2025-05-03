using System;
using UnityEngine;

namespace VacuumShaders.CurvedWorld
{
	// Token: 0x02000002 RID: 2
	[AddComponentMenu("VacuumShaders/Curved World/Eagle Eye")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class CurvedWorld_EagleEye : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private void OnEnable()
		{
			CurvedWorld_EagleEye.get = this;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		private void OnDisable()
		{
			CurvedWorld_EagleEye.get = null;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
		private void Start()
		{
			if (this._camer == null)
			{
				this._camer = base.GetComponent<Camera>();
			}
			if (this._camer != null)
			{
				this.savedValue = this._camer.fieldOfView;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000209C File Offset: 0x0000029C
		private void OnPreCull()
		{
			if (this._camer != null)
			{
				this.savedValue = this._camer.fieldOfView;
				this._camer.fieldOfView = Mathf.Clamp(this.fieldOfView, 1f, 179f);
				Shader.SetGlobalMatrix("_V_CW_Camera2World", this._camer.cameraToWorldMatrix);
				Shader.SetGlobalMatrix("_V_CW_World2Camera", this._camer.cameraToWorldMatrix.inverse);
				CurvedWorld_EagleEye.get = this;
				return;
			}
			CurvedWorld_EagleEye.get = null;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002127 File Offset: 0x00000327
		private void OnPreRender()
		{
			if (this._camer != null)
			{
				this._camer.fieldOfView = this.savedValue;
			}
		}

		// Token: 0x04000001 RID: 1
		[Range(1f, 180f)]
		public float fieldOfView = 60f;

		// Token: 0x04000002 RID: 2
		private float savedValue;

		// Token: 0x04000003 RID: 3
		private Camera _camer;

		// Token: 0x04000004 RID: 4
		public static CurvedWorld_EagleEye get;
	}
}
