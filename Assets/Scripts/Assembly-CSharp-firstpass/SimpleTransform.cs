using System;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class SimpleTransform
{
	// Token: 0x06000343 RID: 835 RVA: 0x00013E64 File Offset: 0x00012064
	public SimpleTransform(Vector3 Position, Quaternion Rotation)
	{
		position = Position;
		rotation = Rotation;
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00013E74 File Offset: 0x00012074
	public SimpleTransform(Transform unityTransform)
	{
		position = unityTransform.position;
		rotation = unityTransform.rotation;
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000345 RID: 837 RVA: 0x00013E9C File Offset: 0x0001209C
	private Vector3 TransposedXaxis
	{
		get
		{
			return new Vector3(this._xaxis.x, this._yaxis.x, this._zaxis.x);
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x06000346 RID: 838 RVA: 0x00013ED0 File Offset: 0x000120D0
	private Vector3 TransposedYaxis
	{
		get
		{
			return new Vector3(this._xaxis.y, this._yaxis.y, this._zaxis.y);
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000347 RID: 839 RVA: 0x00013F04 File Offset: 0x00012104
	private Vector3 TransposedZaxis
	{
		get
		{
			return new Vector3(this._xaxis.z, this._yaxis.z, this._zaxis.z);
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06000348 RID: 840 RVA: 0x00013F38 File Offset: 0x00012138
	// (set) Token: 0x06000349 RID: 841 RVA: 0x00013F40 File Offset: 0x00012140
	public Vector3 position
	{
		get
		{
			return this._position;
		}
		set
		{
			this._position = value;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x0600034A RID: 842 RVA: 0x00013F4C File Offset: 0x0001214C
	// (set) Token: 0x0600034B RID: 843 RVA: 0x00013F54 File Offset: 0x00012154
	public Quaternion rotation
	{
		get
		{
			return this._rotation;
		}
		set
		{
			this._rotation = value;
			this.UpdateAxes();
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x0600034C RID: 844 RVA: 0x00013F64 File Offset: 0x00012164
	public Vector3 right
	{
		get
		{
			return this._xaxis;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x0600034D RID: 845 RVA: 0x00013F6C File Offset: 0x0001216C
	public Vector3 up
	{
		get
		{
			return this._yaxis;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x0600034E RID: 846 RVA: 0x00013F74 File Offset: 0x00012174
	public Vector3 forward
	{
		get
		{
			return this._zaxis;
		}
	}

	// Token: 0x1700005C RID: 92
	public Vector3 this[int i]
	{
		get
		{
			switch (i)
			{
			case 0:
				return this._xaxis;
			case 1:
				return this._yaxis;
			case 2:
				return this._zaxis;
			default:
				return Vector3.zero;
			}
		}
	}

	// Token: 0x1700005D RID: 93
	public float this[int i, int j]
	{
		get
		{
			return this[i][j];
		}
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00013FDC File Offset: 0x000121DC
	public void SetIdentity()
	{
		this.position = Vector3.zero;
		this.rotation = Quaternion.identity;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00013FF4 File Offset: 0x000121F4
	public Vector3 TransformPoint(Vector3 p)
	{
		return this._position + p.x * this._xaxis + p.y * this._yaxis + p.z * this._zaxis;
	}

	// Token: 0x06000353 RID: 851 RVA: 0x0001404C File Offset: 0x0001224C
	public Vector3 TransformDirection(Vector3 d)
	{
		return d.x * this._xaxis + d.y * this._yaxis + d.z * this._zaxis;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x0001409C File Offset: 0x0001229C
	public Vector3 InverseTransformPoint(Vector3 p)
	{
		return (p.x - this._position.x) * this.TransposedXaxis + (p.y - this._position.y) * this.TransposedYaxis + (p.z - this._position.z) * this.TransposedZaxis;
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00014110 File Offset: 0x00012310
	public Vector3 InverseTransformDirection(Vector3 d)
	{
		return d.x * this.TransposedXaxis + d.y * this.TransposedYaxis + d.z * this.TransposedZaxis;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00014160 File Offset: 0x00012360
	private void UpdateAxes()
	{
		this._xaxis.x = 1f - 2f * this._rotation.y * this._rotation.y - 2f * this._rotation.z * this._rotation.z;
		this._xaxis.y = 2f * this._rotation.x * this._rotation.y + 2f * this._rotation.w * this._rotation.z;
		this._xaxis.z = 2f * this._rotation.x * this._rotation.z - 2f * this._rotation.w * this._rotation.y;
		this._yaxis.x = 2f * this._rotation.x * this._rotation.y - 2f * this._rotation.w * this._rotation.z;
		this._yaxis.y = 1f - 2f * this._rotation.x * this._rotation.x - 2f * this._rotation.z * this._rotation.z;
		this._yaxis.z = 2f * this._rotation.y * this._rotation.z + 2f * this._rotation.w * this._rotation.x;
		this._zaxis.x = 2f * this._rotation.x * this._rotation.z + 2f * this._rotation.w * this._rotation.y;
		this._zaxis.y = 2f * this._rotation.y * this._rotation.z - 2f * this._rotation.w * this._rotation.x;
		this._zaxis.z = 1f - 2f * this._rotation.x * this._rotation.x - 2f * this._rotation.y * this._rotation.y;
	}

	// Token: 0x06000357 RID: 855 RVA: 0x000143F8 File Offset: 0x000125F8
	public override string ToString()
	{
		return string.Format("[{0},{1},{2},{3}]", new object[] { this._xaxis, this._yaxis, this._zaxis, this._position });
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00014450 File Offset: 0x00012650
	public static implicit operator SimpleTransform(Transform src)
	{
		return new SimpleTransform(src);
	}

	// Token: 0x040001C4 RID: 452
	private Vector3 _xaxis;

	// Token: 0x040001C5 RID: 453
	private Vector3 _yaxis;

	// Token: 0x040001C6 RID: 454
	private Vector3 _zaxis;

	// Token: 0x040001C7 RID: 455
	private Vector3 _position;

	// Token: 0x040001C8 RID: 456
	private Quaternion _rotation;
}
