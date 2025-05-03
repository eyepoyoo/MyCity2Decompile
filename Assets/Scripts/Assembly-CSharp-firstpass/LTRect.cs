using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
[Serializable]
public class LTRect
{
	// Token: 0x060001A5 RID: 421 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
	public LTRect()
	{
		this.reset();
		this.rotateEnabled = (this.alphaEnabled = true);
		this._rect = new Rect(0f, 0f, 1f, 1f);
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000C44C File Offset: 0x0000A64C
	public LTRect(Rect rect)
	{
		this._rect = rect;
		this.reset();
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
	public LTRect(float x, float y, float width, float height)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = 1f;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000C530 File Offset: 0x0000A730
	public LTRect(float x, float y, float width, float height, float alpha)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = alpha;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000C5B4 File Offset: 0x0000A7B4
	public LTRect(float x, float y, float width, float height, float alpha, float rotation)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = alpha;
		this.rotation = rotation;
		this.rotateEnabled = (this.alphaEnabled = false);
		if (rotation != 0f)
		{
			this.rotateEnabled = true;
			this.resetForRotation();
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060001AA RID: 426 RVA: 0x0000C64C File Offset: 0x0000A84C
	public bool hasInitiliazed
	{
		get
		{
			return this._id != -1;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060001AB RID: 427 RVA: 0x0000C65C File Offset: 0x0000A85C
	public int id
	{
		get
		{
			return this._id | (this.counter << 16);
		}
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000C67C File Offset: 0x0000A87C
	public void setId(int id, int counter)
	{
		this._id = id;
		this.counter = counter;
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000C68C File Offset: 0x0000A88C
	public void reset()
	{
		this.alpha = 1f;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
		this.margin = Vector2.zero;
		this.sizeByHeight = false;
		this.useColor = false;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000C6D8 File Offset: 0x0000A8D8
	public void resetForRotation()
	{
		Vector3 vector = new Vector3(GUI.matrix[0, 0], GUI.matrix[1, 1], GUI.matrix[2, 2]);
		if (this.pivot == Vector2.zero)
		{
			this.pivot = new Vector2((this._rect.x + this._rect.width * 0.5f) * vector.x + GUI.matrix[0, 3], (this._rect.y + this._rect.height * 0.5f) * vector.y + GUI.matrix[1, 3]);
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060001AF RID: 431 RVA: 0x0000C7A8 File Offset: 0x0000A9A8
	// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000C7B8 File Offset: 0x0000A9B8
	public float x
	{
		get
		{
			return this._rect.x;
		}
		set
		{
			this._rect.x = value;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000C7C8 File Offset: 0x0000A9C8
	// (set) Token: 0x060001B2 RID: 434 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
	public float y
	{
		get
		{
			return this._rect.y;
		}
		set
		{
			this._rect.y = value;
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000C7E8 File Offset: 0x0000A9E8
	// (set) Token: 0x060001B4 RID: 436 RVA: 0x0000C7F8 File Offset: 0x0000A9F8
	public float width
	{
		get
		{
			return this._rect.width;
		}
		set
		{
			this._rect.width = value;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000C808 File Offset: 0x0000AA08
	// (set) Token: 0x060001B6 RID: 438 RVA: 0x0000C818 File Offset: 0x0000AA18
	public float height
	{
		get
		{
			return this._rect.height;
		}
		set
		{
			this._rect.height = value;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000C828 File Offset: 0x0000AA28
	// (set) Token: 0x060001B8 RID: 440 RVA: 0x0000C968 File Offset: 0x0000AB68
	public Rect rect
	{
		get
		{
			if (LTRect.colorTouched)
			{
				LTRect.colorTouched = false;
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
			}
			if (this.rotateEnabled)
			{
				if (this.rotateFinished)
				{
					this.rotateFinished = false;
					this.rotateEnabled = false;
					this.pivot = Vector2.zero;
				}
				else
				{
					GUIUtility.RotateAroundPivot(this.rotation, this.pivot);
				}
			}
			if (this.alphaEnabled)
			{
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alpha);
				LTRect.colorTouched = true;
			}
			if (this.fontScaleToFit)
			{
				if (this.useSimpleScale)
				{
					this.style.fontSize = (int)(this._rect.height * this.relativeRect.height);
				}
				else
				{
					this.style.fontSize = (int)this._rect.height;
				}
			}
			return this._rect;
		}
		set
		{
			this._rect = value;
		}
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000C974 File Offset: 0x0000AB74
	public LTRect setStyle(GUIStyle style)
	{
		this.style = style;
		return this;
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000C980 File Offset: 0x0000AB80
	public LTRect setFontScaleToFit(bool fontScaleToFit)
	{
		this.fontScaleToFit = fontScaleToFit;
		return this;
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000C98C File Offset: 0x0000AB8C
	public LTRect setColor(Color color)
	{
		this.color = color;
		this.useColor = true;
		return this;
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000C9A0 File Offset: 0x0000ABA0
	public LTRect setAlpha(float alpha)
	{
		this.alpha = alpha;
		return this;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000C9AC File Offset: 0x0000ABAC
	public LTRect setLabel(string str)
	{
		this.labelStr = str;
		return this;
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000C9B8 File Offset: 0x0000ABB8
	public LTRect setUseSimpleScale(bool useSimpleScale, Rect relativeRect)
	{
		this.useSimpleScale = useSimpleScale;
		this.relativeRect = relativeRect;
		return this;
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000C9CC File Offset: 0x0000ABCC
	public LTRect setUseSimpleScale(bool useSimpleScale)
	{
		this.useSimpleScale = useSimpleScale;
		this.relativeRect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		return this;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000C9F8 File Offset: 0x0000ABF8
	public LTRect setSizeByHeight(bool sizeByHeight)
	{
		this.sizeByHeight = sizeByHeight;
		return this;
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000CA04 File Offset: 0x0000AC04
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"x:",
			this._rect.x,
			" y:",
			this._rect.y,
			" width:",
			this._rect.width,
			" height:",
			this._rect.height
		});
	}

	// Token: 0x04000104 RID: 260
	public Rect _rect;

	// Token: 0x04000105 RID: 261
	public float alpha = 1f;

	// Token: 0x04000106 RID: 262
	public float rotation;

	// Token: 0x04000107 RID: 263
	public Vector2 pivot;

	// Token: 0x04000108 RID: 264
	public Vector2 margin;

	// Token: 0x04000109 RID: 265
	public Rect relativeRect = new Rect(0f, 0f, float.PositiveInfinity, float.PositiveInfinity);

	// Token: 0x0400010A RID: 266
	public bool rotateEnabled;

	// Token: 0x0400010B RID: 267
	[HideInInspector]
	public bool rotateFinished;

	// Token: 0x0400010C RID: 268
	public bool alphaEnabled;

	// Token: 0x0400010D RID: 269
	public string labelStr;

	// Token: 0x0400010E RID: 270
	public LTGUI.Element_Type type;

	// Token: 0x0400010F RID: 271
	public GUIStyle style;

	// Token: 0x04000110 RID: 272
	public bool useColor;

	// Token: 0x04000111 RID: 273
	public Color color = Color.white;

	// Token: 0x04000112 RID: 274
	public bool fontScaleToFit;

	// Token: 0x04000113 RID: 275
	public bool useSimpleScale;

	// Token: 0x04000114 RID: 276
	public bool sizeByHeight;

	// Token: 0x04000115 RID: 277
	public Texture texture;

	// Token: 0x04000116 RID: 278
	private int _id = -1;

	// Token: 0x04000117 RID: 279
	[HideInInspector]
	public int counter;

	// Token: 0x04000118 RID: 280
	public static bool colorTouched;
}
