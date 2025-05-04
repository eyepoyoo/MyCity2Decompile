using System;
using UnityEngine;

namespace VacuumShaders.CurvedWorld
{
	// Token: 0x02000004 RID: 4
	[AddComponentMenu("VacuumShaders/Curved World/Controller")]
	[ExecuteInEditMode]
	public class CurvedWorld_Controller : MonoBehaviour
	{
		// Token: 0x06000007 RID: 7 RVA: 0x0000215B File Offset: 0x0000035B
		private void OnEnable()
		{
			this.LoadIDs();
			this.EnableBend();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002169 File Offset: 0x00000369
		private void OnDisable()
		{
			this.DisableBend();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002171 File Offset: 0x00000371
		private void OnDestroy()
		{
			if (CurvedWorld_Controller.get == this)
			{
				CurvedWorld_Controller.get = null;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002186 File Offset: 0x00000386
		private void Start()
		{
			if (CurvedWorld_Controller.get != null && CurvedWorld_Controller.get != this)
			{
				Debug.LogError("There is more then one CurvedWorld Global Controller in the scene.\nPlease ensure there is always exactly one CurvedWorld Global Controller in the scene.\n", CurvedWorld_Controller.get.gameObject);
			}
			CurvedWorld_Controller.get = this;
			this.LoadIDs();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021C4 File Offset: 0x000003C4
		private void Update()
		{
			if (CurvedWorld_Controller.get == null)
			{
				CurvedWorld_Controller.get = this;
			}
			if (base.isActiveAndEnabled)
			{
				Shader.SetGlobalVector(this._V_CW_PivotPoint_Position_ID, (this.pivotPoint == null) ? Vector3.zero : this.pivotPoint.transform.position);
				if (this._V_CW_Bend_X_current != this._V_CW_Bend_X || this._V_CW_Bend_Y_current != this._V_CW_Bend_Y || this._V_CW_Bend_Z_current != this._V_CW_Bend_Z)
				{
					this._V_CW_Bend_X_current = this._V_CW_Bend_X;
					this._V_CW_Bend_Y_current = this._V_CW_Bend_Y;
					this._V_CW_Bend_Z_current = this._V_CW_Bend_Z;
					this._V_CW_Bend = new Vector3(this._V_CW_Bend_X, this._V_CW_Bend_Y, this._V_CW_Bend_Z);
					Shader.SetGlobalVector(this._V_CW_Bend_ID, this._V_CW_Bend);
				}
				if (this._V_CW_Bias_X_current != this._V_CW_Bias_X || this._V_CW_Bias_Y_current != this._V_CW_Bias_Y || this._V_CW_Bias_Z_current != this._V_CW_Bias_Z)
				{
					if (this._V_CW_Bias_X < 0f)
					{
						this._V_CW_Bias_X = 0f;
					}
					if (this._V_CW_Bias_Y < 0f)
					{
						this._V_CW_Bias_Y = 0f;
					}
					if (this._V_CW_Bias_Z < 0f)
					{
						this._V_CW_Bias_Z = 0f;
					}
					this._V_CW_Bias_X_current = this._V_CW_Bias_X;
					this._V_CW_Bias_Y_current = this._V_CW_Bias_Y;
					this._V_CW_Bias_Z_current = this._V_CW_Bias_Z;
					this._V_CW_Bias = new Vector3(this._V_CW_Bias_X, this._V_CW_Bias_Y, this._V_CW_Bias_Z);
					Shader.SetGlobalVector(this._V_CW_Bias_ID, this._V_CW_Bias);
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002371 File Offset: 0x00000571
		private void LoadIDs()
		{
			this._V_CW_PivotPoint_Position_ID = Shader.PropertyToID("_V_CW_PivotPoint_Position");
			this._V_CW_Bend_ID = Shader.PropertyToID("_V_CW_Bend");
			this._V_CW_Bias_ID = Shader.PropertyToID("_V_CW_Bias");
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000023A4 File Offset: 0x000005A4
		public void Reset()
		{
			Shader.SetGlobalVector(this._V_CW_PivotPoint_Position_ID, Vector3.zero);
			this._V_CW_Bend = Vector3.zero;
			this._V_CW_Bias = Vector3.zero;
			this._V_CW_Bend_X_current = (this._V_CW_Bend_X = 0f);
			this._V_CW_Bend_Y_current = (this._V_CW_Bend_Y = 0f);
			this._V_CW_Bend_Z_current = (this._V_CW_Bend_Z = 0f);
			this._V_CW_Bias_X_current = (this._V_CW_Bias_X = 0f);
			this._V_CW_Bias_Y_current = (this._V_CW_Bias_Y = 0f);
			this._V_CW_Bias_Z_current = (this._V_CW_Bias_Z = 0f);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002458 File Offset: 0x00000658
		public void ForceUpdate()
		{
			this.LoadIDs();
			Shader.SetGlobalVector(this._V_CW_PivotPoint_Position_ID, (this.pivotPoint == null) ? Vector3.zero : this.pivotPoint.transform.position);
			this._V_CW_Bend = new Vector3(this._V_CW_Bend_X, this._V_CW_Bend_Y, this._V_CW_Bend_Z);
			Shader.SetGlobalVector(this._V_CW_Bend_ID, this._V_CW_Bend);
			this._V_CW_Bias = new Vector3(this._V_CW_Bias_X, this._V_CW_Bias_Y, this._V_CW_Bias_Z);
			Shader.SetGlobalVector(this._V_CW_Bias_ID, this._V_CW_Bias);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002506 File Offset: 0x00000706
		public void EnableBend()
		{
			this.ForceUpdate();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002510 File Offset: 0x00000710
		public void DisableBend()
		{
			this.LoadIDs();
			Shader.SetGlobalVector(this._V_CW_PivotPoint_Position_ID, Vector3.zero);
			Shader.SetGlobalVector(this._V_CW_Bend_ID, Vector3.zero);
			Shader.SetGlobalVector(this._V_CW_Bias_ID, Vector3.zero);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002564 File Offset: 0x00000764
		public static Vector3 TransformPoint(Vector3 _transformPoint, BEND_TYPE _bendType, Vector3 _bendSize, Vector3 _bendBias, Vector3 _pivotPoint)
		{
			switch (_bendType)
			{
			case BEND_TYPE.ClassicRunner:
			{
				Vector3 vector = _transformPoint - _pivotPoint;
				float num = Mathf.Max(0f, vector.z - _bendBias.x);
				float num2 = Mathf.Max(0f, vector.z - _bendBias.y);
				vector = new Vector3(-_bendSize.y * num2 * num2, _bendSize.x * num * num, 0f) * 0.001f;
				return _transformPoint + vector;
			}
			case BEND_TYPE.LittlePlanet:
			{
				Vector3 vector2 = _transformPoint - _pivotPoint;
				float num3 = Mathf.Max(0f, Mathf.Abs(vector2.z) - _bendBias.x) * ((vector2.z < 0f) ? (-1f) : 1f);
				float num4 = Mathf.Max(0f, Mathf.Abs(vector2.x) - _bendBias.z) * ((vector2.x < 0f) ? (-1f) : 1f);
				vector2 = new Vector3(0f, (_bendSize.x * num3 * num3 + _bendSize.z * num4 * num4) * 0.001f, 0f);
				return _transformPoint + vector2;
			}
			case BEND_TYPE.Universal:
			{
				Vector3 vector3 = _transformPoint - _pivotPoint;
				float num5 = Mathf.Max(0f, Mathf.Abs(vector3.z) - _bendBias.x) * ((vector3.z < 0f) ? (-1f) : 1f);
				float num6 = Mathf.Max(0f, Mathf.Abs(vector3.z) - _bendBias.y) * ((vector3.z < 0f) ? (-1f) : 1f);
				float num7 = Mathf.Max(0f, Mathf.Abs(vector3.x) - _bendBias.z) * ((vector3.x < 0f) ? (-1f) : 1f);
				vector3 = new Vector3(-_bendSize.y * num6 * num6, _bendSize.x * num5 * num5 + num7 * num7 * _bendSize.z, 0f) * 0.001f;
				return _transformPoint + vector3;
			}
			case BEND_TYPE.Perspective2D:
			{
				Vector3 vector4 = _transformPoint - _pivotPoint;
				vector4 = Camera.main.worldToCameraMatrix.MultiplyPoint(vector4);
				float num8 = Mathf.Max(0f, Mathf.Abs(vector4.y) - _bendBias.x) * ((vector4.y < 0f) ? (-1f) : 1f);
				num8 *= num8;
				float num9 = Mathf.Max(0f, Mathf.Abs(vector4.x) - _bendBias.y) * ((vector4.x < 0f) ? (-1f) : 1f);
				num9 *= num9;
				Vector3 vector5 = vector4;
				vector5.z -= (_bendSize.x * num8 + _bendSize.y * num8) * 0.001f;
				return Camera.main.worldToCameraMatrix.inverse.MultiplyPoint(vector5);
			}
			default:
				return _transformPoint;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000028B0 File Offset: 0x00000AB0
		public Vector3 TransformPoint(Vector3 _transformPoint)
		{
			if (!base.enabled)
			{
				return _transformPoint;
			}
			return CurvedWorld_Controller.TransformPoint(_transformPoint, this.bendType, this.GetBend(), this.GetBias(), (this.pivotPoint == null) ? Vector3.zero : this.pivotPoint.position);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000028FF File Offset: 0x00000AFF
		public Vector3 GetBend()
		{
			return this._V_CW_Bend;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002908 File Offset: 0x00000B08
		public void SetBend(Vector3 _newBend)
		{
			this._V_CW_Bend_X = _newBend.x;
			this._V_CW_Bend_Y = _newBend.y;
			this._V_CW_Bend_Z = _newBend.z;
			this._V_CW_Bend = new Vector3(this._V_CW_Bend_X, this._V_CW_Bend_Y, this._V_CW_Bend_Z);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002959 File Offset: 0x00000B59
		public Vector3 GetBias()
		{
			return this._V_CW_Bias;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002964 File Offset: 0x00000B64
		public void SetBias(Vector3 _newBias)
		{
			if (_newBias.x < 0f)
			{
				_newBias.x = 0f;
			}
			if (_newBias.y < 0f)
			{
				_newBias.y = 0f;
			}
			if (_newBias.z < 0f)
			{
				_newBias.z = 0f;
			}
			this._V_CW_Bias_X = _newBias.x;
			this._V_CW_Bias_Y = _newBias.y;
			this._V_CW_Bias_Z = _newBias.z;
			this._V_CW_Bias = new Vector3(this._V_CW_Bias_X, this._V_CW_Bias_Y, this._V_CW_Bias_Z);
		}

		// Token: 0x0400000A RID: 10
		[HideInInspector]
		public BEND_TYPE bendType;

		// Token: 0x0400000B RID: 11
		[HideInInspector]
		public Transform pivotPoint;

		// Token: 0x0400000C RID: 12
		private int _V_CW_PivotPoint_Position_ID;

		// Token: 0x0400000D RID: 13
		[HideInInspector]
		private Vector3 _V_CW_Bend = Vector3.zero;

		// Token: 0x0400000E RID: 14
		private int _V_CW_Bend_ID;

		// Token: 0x0400000F RID: 15
		[HideInInspector]
		private Vector3 _V_CW_Bias = Vector3.zero;

		// Token: 0x04000010 RID: 16
		private int _V_CW_Bias_ID;

		// Token: 0x04000011 RID: 17
		[HideInInspector]
		public float _V_CW_Bend_X;

		// Token: 0x04000012 RID: 18
		private float _V_CW_Bend_X_current = 1f;

		// Token: 0x04000013 RID: 19
		[HideInInspector]
		public float _V_CW_Bend_Y;

		// Token: 0x04000014 RID: 20
		private float _V_CW_Bend_Y_current = 1f;

		// Token: 0x04000015 RID: 21
		[HideInInspector]
		public float _V_CW_Bend_Z;

		// Token: 0x04000016 RID: 22
		private float _V_CW_Bend_Z_current = 1f;

		// Token: 0x04000017 RID: 23
		[HideInInspector]
		public float _V_CW_Bias_X;

		// Token: 0x04000018 RID: 24
		private float _V_CW_Bias_X_current = 1f;

		// Token: 0x04000019 RID: 25
		[HideInInspector]
		public float _V_CW_Bias_Y;

		// Token: 0x0400001A RID: 26
		private float _V_CW_Bias_Y_current = 1f;

		// Token: 0x0400001B RID: 27
		[HideInInspector]
		public float _V_CW_Bias_Z;

		// Token: 0x0400001C RID: 28
		private float _V_CW_Bias_Z_current = 1f;

		// Token: 0x0400001D RID: 29
		public static CurvedWorld_Controller get;
	}
}
