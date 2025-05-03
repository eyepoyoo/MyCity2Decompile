using System;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class Transform2d
{
	// Token: 0x06000359 RID: 857 RVA: 0x00014458 File Offset: 0x00012658
	public Transform2d(Vector2 position, float rotation)
	{
		this.Position = position;
		this.Rotation = rotation;
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x0600035B RID: 859 RVA: 0x00014480 File Offset: 0x00012680
	public static Transform2d Identity
	{
		get
		{
			return Transform2d._identity;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x0600035C RID: 860 RVA: 0x00014488 File Offset: 0x00012688
	private Vector2 _transposedXaxis
	{
		get
		{
			return new Vector2(this._xaxis.x, this._yaxis.x);
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600035D RID: 861 RVA: 0x000144A8 File Offset: 0x000126A8
	private Vector2 _transposedYaxis
	{
		get
		{
			return new Vector2(this._xaxis.y, this._yaxis.y);
		}
	}

	// Token: 0x17000061 RID: 97
	// (set) Token: 0x0600035E RID: 862 RVA: 0x000144C8 File Offset: 0x000126C8
	private bool _setIsRightHanded
	{
		set
		{
			this._isRightHanded = value;
			this.UpdateAxes();
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x0600035F RID: 863 RVA: 0x000144D8 File Offset: 0x000126D8
	// (set) Token: 0x06000360 RID: 864 RVA: 0x000144E0 File Offset: 0x000126E0
	public Vector2 Position
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

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000361 RID: 865 RVA: 0x000144EC File Offset: 0x000126EC
	// (set) Token: 0x06000362 RID: 866 RVA: 0x000144F4 File Offset: 0x000126F4
	public float Rotation
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

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000363 RID: 867 RVA: 0x00014504 File Offset: 0x00012704
	public Vector2 Right
	{
		get
		{
			return (!this._isRightHanded) ? this._xaxis : (-this._xaxis);
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000364 RID: 868 RVA: 0x00014528 File Offset: 0x00012728
	public Vector2 Left
	{
		get
		{
			return (!this._isRightHanded) ? (-this._xaxis) : this._xaxis;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000365 RID: 869 RVA: 0x0001454C File Offset: 0x0001274C
	public Vector2 Up
	{
		get
		{
			return this._yaxis;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x06000366 RID: 870 RVA: 0x00014554 File Offset: 0x00012754
	// (set) Token: 0x06000367 RID: 871 RVA: 0x0001455C File Offset: 0x0001275C
	public bool IsRightHanded
	{
		get
		{
			return this._isRightHanded;
		}
		set
		{
			this._setIsRightHanded = value;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000368 RID: 872 RVA: 0x00014568 File Offset: 0x00012768
	// (set) Token: 0x06000369 RID: 873 RVA: 0x0001457C File Offset: 0x0001277C
	public bool IsRotated
	{
		get
		{
			return this._rotation != 0f;
		}
		set
		{
			if (!value)
			{
				this.Rotation = 0f;
			}
		}
	}

	// Token: 0x17000069 RID: 105
	public Vector2 this[int i]
	{
		get
		{
			if (i == 0)
			{
				return this._xaxis;
			}
			if (i != 1)
			{
				return Vector2.zero;
			}
			return this._yaxis;
		}
	}

	// Token: 0x1700006A RID: 106
	public float this[int i, int j]
	{
		get
		{
			return this[i][j];
		}
	}

	// Token: 0x0600036C RID: 876 RVA: 0x000145E4 File Offset: 0x000127E4
	public void SetIdentity()
	{
		this.Position = Vector2.zero;
		this.Rotation = 0f;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x000145FC File Offset: 0x000127FC
	public Vector2 TransformPoint(Vector2 p)
	{
		return (this._rotation != 0f || this._isRightHanded) ? (this._position + p.x * this._xaxis + p.y * this._yaxis) : (this._position + p);
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0001466C File Offset: 0x0001286C
	public Vector2 TransformDirection(Vector2 d)
	{
		return (this._rotation != 0f || this._isRightHanded) ? (d.x * this._xaxis + d.y * this._yaxis) : d;
	}

	// Token: 0x0600036F RID: 879 RVA: 0x000146C4 File Offset: 0x000128C4
	public Vector2 InverseTransformPoint(Vector2 p)
	{
		return (this._rotation != 0f || this._isRightHanded) ? ((p.x - this._position.x) * this._transposedXaxis + (p.y - this._position.y) * this._transposedYaxis) : (p - this._position);
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00014740 File Offset: 0x00012940
	public Vector2 InverseTransformDirection(Vector2 d)
	{
		return (this._rotation != 0f || this._isRightHanded) ? (d.x * this._transposedXaxis + d.y * this._transposedYaxis) : d;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00014798 File Offset: 0x00012998
	private void UpdateAxes()
	{
		this._xaxis.x = Mathf.Cos(this._rotation);
		this._xaxis.y = Mathf.Sin(this._rotation);
		this._yaxis.x = -this._xaxis.y;
		this._yaxis.y = this._xaxis.x;
		if (this._isRightHanded)
		{
			this._xaxis.x = -this._xaxis.x;
			this._yaxis.x = -this._yaxis.x;
		}
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00014838 File Offset: 0x00012A38
	public override string ToString()
	{
		return string.Format("[{0},{1}]", this.Position, this.Rotation);
	}

	// Token: 0x040001C9 RID: 457
	private static Transform2d _identity = new Transform2d(Vector2.zero, 0f);

	// Token: 0x040001CA RID: 458
	private Vector2 _xaxis;

	// Token: 0x040001CB RID: 459
	private Vector2 _yaxis;

	// Token: 0x040001CC RID: 460
	public Vector2 _position;

	// Token: 0x040001CD RID: 461
	private float _rotation;

	// Token: 0x040001CE RID: 462
	private bool _isRightHanded;
}
