using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000015 RID: 21
public class LTDescr
{
	// Token: 0x060000AC RID: 172 RVA: 0x00004014 File Offset: 0x00002214
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			(!(this.trans != null)) ? "gameObject:null" : ("gameObject:" + this.trans.gameObject),
			" toggle:",
			this.toggle,
			" passed:",
			this.passed,
			" time:",
			this.time,
			" delay:",
			this.delay,
			" direction:",
			this.direction,
			" from:",
			this.from,
			" to:",
			this.to,
			" type:",
			this.type,
			" ease:",
			this.tweenType,
			" useEstimatedTime:",
			this.useEstimatedTime,
			" id:",
			this.id,
			" hasInitiliazed:",
			this.hasInitiliazed
		});
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00004178 File Offset: 0x00002378
	[Obsolete("Use 'LeanTween.cancel( id )' instead")]
	public LTDescr cancel(GameObject gameObject)
	{
		if (gameObject == this.trans.gameObject)
		{
			LeanTween.removeTween((int)this._id, this.uniqueId);
		}
		return this;
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x060000AE RID: 174 RVA: 0x000041B0 File Offset: 0x000023B0
	public int uniqueId
	{
		get
		{
			return (int)(this._id | (this.counter << 16));
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060000AF RID: 175 RVA: 0x000041D0 File Offset: 0x000023D0
	public int id
	{
		get
		{
			return this.uniqueId;
		}
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x000041D8 File Offset: 0x000023D8
	public void reset()
	{
		this.toggle = true;
		this.trans = null;
		this.passed = (this.delay = (this.lastVal = 0f));
		this.hasUpdateCallback = (this.useEstimatedTime = (this.useFrames = (this.hasInitiliazed = (this.onCompleteOnRepeat = (this.destroyOnComplete = (this.onCompleteOnStart = (this.useManualTime = false)))))));
		this.animationCurve = null;
		this.tweenType = LeanTweenType.linear;
		this.loopType = LeanTweenType.once;
		this.loopCount = 0;
		this.direction = (this.directionLast = (this.overshoot = 1f));
		this.period = 0.3f;
		this.point = Vector3.zero;
		this.cleanup();
		LTDescr.global_counter += 1U;
		if (LTDescr.global_counter > 32768U)
		{
			LTDescr.global_counter = 0U;
		}
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000042D0 File Offset: 0x000024D0
	public void cleanup()
	{
		this.onUpdateFloat = null;
		this.onUpdateFloatRatio = null;
		this.onUpdateVector2 = null;
		this.onUpdateVector3 = null;
		this.onUpdateFloatObject = null;
		this.onUpdateVector3Object = null;
		this.onUpdateColor = null;
		this.onComplete = null;
		this.onCompleteObject = null;
		this.onCompleteParam = null;
		this.onStart = null;
		this.rectTransform = null;
		this.uiText = null;
		this.uiImage = null;
		this.sprites = null;
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00004348 File Offset: 0x00002548
	public void init()
	{
		this.hasInitiliazed = true;
		if (this.onStart != null)
		{
			this.onStart();
		}
		switch (this.type)
		{
		case TweenAction.MOVE_X:
			this.from.x = this.trans.position.x;
			break;
		case TweenAction.MOVE_Y:
			this.from.x = this.trans.position.y;
			break;
		case TweenAction.MOVE_Z:
			this.from.x = this.trans.position.z;
			break;
		case TweenAction.MOVE_LOCAL_X:
			this.from.x = this.trans.localPosition.x;
			break;
		case TweenAction.MOVE_LOCAL_Y:
			this.from.x = this.trans.localPosition.y;
			break;
		case TweenAction.MOVE_LOCAL_Z:
			this.from.x = this.trans.localPosition.z;
			break;
		case TweenAction.MOVE_CURVED:
		case TweenAction.MOVE_CURVED_LOCAL:
		case TweenAction.MOVE_SPLINE:
		case TweenAction.MOVE_SPLINE_LOCAL:
			this.from.x = 0f;
			break;
		case TweenAction.SCALE_X:
			this.from.x = this.trans.localScale.x;
			break;
		case TweenAction.SCALE_Y:
			this.from.x = this.trans.localScale.y;
			break;
		case TweenAction.SCALE_Z:
			this.from.x = this.trans.localScale.z;
			break;
		case TweenAction.ROTATE_X:
			this.from.x = this.trans.eulerAngles.x;
			this.to.x = LeanTween.closestRot(this.from.x, this.to.x);
			break;
		case TweenAction.ROTATE_Y:
			this.from.x = this.trans.eulerAngles.y;
			this.to.x = LeanTween.closestRot(this.from.x, this.to.x);
			break;
		case TweenAction.ROTATE_Z:
			this.from.x = this.trans.eulerAngles.z;
			this.to.x = LeanTween.closestRot(this.from.x, this.to.x);
			break;
		case TweenAction.ROTATE_AROUND:
			this.lastVal = 0f;
			this.from.x = 0f;
			this.origRotation = this.trans.rotation;
			break;
		case TweenAction.ROTATE_AROUND_LOCAL:
			this.lastVal = 0f;
			this.from.x = 0f;
			this.origRotation = this.trans.localRotation;
			break;
		case TweenAction.CANVAS_ROTATEAROUND:
		case TweenAction.CANVAS_ROTATEAROUND_LOCAL:
			this.lastVal = 0f;
			this.from.x = 0f;
			this.origRotation = this.rectTransform.rotation;
			break;
		case TweenAction.CANVAS_PLAYSPRITE:
			this.uiImage = this.trans.gameObject.GetComponent<Image>();
			this.from.x = 0f;
			break;
		case TweenAction.ALPHA:
		{
			SpriteRenderer component = this.trans.gameObject.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				this.from.x = component.color.a;
			}
			else if (this.trans.gameObject.GetComponent<Renderer>() != null && this.trans.gameObject.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				this.from.x = this.trans.gameObject.GetComponent<Renderer>().material.color.a;
			}
			else if (this.trans.gameObject.GetComponent<Renderer>() != null && this.trans.gameObject.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				this.from.x = this.trans.gameObject.GetComponent<Renderer>().material.GetColor("_TintColor").a;
			}
			else if (this.trans.childCount > 0)
			{
				foreach (object obj in this.trans)
				{
					Transform transform = (Transform)obj;
					if (transform.gameObject.GetComponent<Renderer>() != null)
					{
						this.from.x = transform.gameObject.GetComponent<Renderer>().material.color.a;
						break;
					}
				}
			}
			break;
		}
		case TweenAction.TEXT_ALPHA:
			this.uiText = this.trans.gameObject.GetComponent<Text>();
			if (this.uiText != null)
			{
				this.from.x = this.uiText.color.a;
			}
			break;
		case TweenAction.CANVAS_ALPHA:
			this.uiImage = this.trans.gameObject.GetComponent<Image>();
			if (this.uiImage != null)
			{
				this.from.x = this.uiImage.color.a;
			}
			break;
		case TweenAction.ALPHA_VERTEX:
			this.from.x = (float)this.trans.GetComponent<MeshFilter>().mesh.colors32[0].a;
			break;
		case TweenAction.COLOR:
		{
			SpriteRenderer component2 = this.trans.gameObject.GetComponent<SpriteRenderer>();
			if (component2 != null)
			{
				Color color = component2.color;
				this.setFromColor(color);
			}
			else if (this.trans.gameObject.GetComponent<Renderer>() != null && this.trans.gameObject.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				Color color2 = this.trans.gameObject.GetComponent<Renderer>().material.color;
				this.setFromColor(color2);
			}
			else if (this.trans.gameObject.GetComponent<Renderer>() != null && this.trans.gameObject.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				Color color3 = this.trans.gameObject.GetComponent<Renderer>().material.GetColor("_TintColor");
				this.setFromColor(color3);
			}
			else if (this.trans.childCount > 0)
			{
				foreach (object obj2 in this.trans)
				{
					Transform transform2 = (Transform)obj2;
					if (transform2.gameObject.GetComponent<Renderer>() != null)
					{
						Color color4 = transform2.gameObject.GetComponent<Renderer>().material.color;
						this.setFromColor(color4);
						break;
					}
				}
			}
			break;
		}
		case TweenAction.CALLBACK_COLOR:
			this.diff = new Vector3(1f, 0f, 0f);
			break;
		case TweenAction.TEXT_COLOR:
			this.uiText = this.trans.gameObject.GetComponent<Text>();
			if (this.uiText != null)
			{
				this.setFromColor(this.uiText.color);
			}
			break;
		case TweenAction.CANVAS_COLOR:
			this.uiImage = this.trans.gameObject.GetComponent<Image>();
			if (this.uiImage != null)
			{
				this.setFromColor(this.uiImage.color);
			}
			break;
		case TweenAction.CANVAS_MOVE_X:
			this.from.x = this.rectTransform.anchoredPosition3D.x;
			break;
		case TweenAction.CANVAS_MOVE_Y:
			this.from.x = this.rectTransform.anchoredPosition3D.y;
			break;
		case TweenAction.CANVAS_MOVE_Z:
			this.from.x = this.rectTransform.anchoredPosition3D.z;
			break;
		case TweenAction.MOVE:
			this.from = this.trans.position;
			break;
		case TweenAction.MOVE_LOCAL:
			this.from = this.trans.localPosition;
			break;
		case TweenAction.ROTATE:
			this.from = this.trans.eulerAngles;
			this.to = new Vector3(LeanTween.closestRot(this.from.x, this.to.x), LeanTween.closestRot(this.from.y, this.to.y), LeanTween.closestRot(this.from.z, this.to.z));
			break;
		case TweenAction.ROTATE_LOCAL:
			this.from = this.trans.localEulerAngles;
			this.to = new Vector3(LeanTween.closestRot(this.from.x, this.to.x), LeanTween.closestRot(this.from.y, this.to.y), LeanTween.closestRot(this.from.z, this.to.z));
			break;
		case TweenAction.SCALE:
			this.from = this.trans.localScale;
			break;
		case TweenAction.GUI_MOVE:
			this.from = new Vector3(this.ltRect.rect.x, this.ltRect.rect.y, 0f);
			break;
		case TweenAction.GUI_MOVE_MARGIN:
			this.from = new Vector2(this.ltRect.margin.x, this.ltRect.margin.y);
			break;
		case TweenAction.GUI_SCALE:
			this.from = new Vector3(this.ltRect.rect.width, this.ltRect.rect.height, 0f);
			break;
		case TweenAction.GUI_ALPHA:
			this.from.x = this.ltRect.alpha;
			break;
		case TweenAction.GUI_ROTATE:
			if (!this.ltRect.rotateEnabled)
			{
				this.ltRect.rotateEnabled = true;
				this.ltRect.resetForRotation();
			}
			this.from.x = this.ltRect.rotation;
			break;
		case TweenAction.CANVAS_MOVE:
			this.from = this.rectTransform.anchoredPosition3D;
			break;
		case TweenAction.CANVAS_SCALE:
			this.from = this.rectTransform.localScale;
			break;
		}
		if (this.type != TweenAction.CALLBACK_COLOR && this.type != TweenAction.COLOR && this.type != TweenAction.TEXT_COLOR && this.type != TweenAction.CANVAS_COLOR)
		{
			this.diff = this.to - this.from;
		}
		if (this.onCompleteOnStart)
		{
			if (this.onComplete != null)
			{
				this.onComplete();
			}
			else if (this.onCompleteObject != null)
			{
				this.onCompleteObject(this.onCompleteParam);
			}
		}
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00004F90 File Offset: 0x00003190
	public LTDescr setFromColor(Color col)
	{
		this.from = new Vector3(0f, col.a, 0f);
		this.diff = new Vector3(1f, 0f, 0f);
		this.axis = new Vector3(col.r, col.g, col.b);
		return this;
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00004FF4 File Offset: 0x000031F4
	public LTDescr pause()
	{
		if (this.direction != 0f)
		{
			this.directionLast = this.direction;
			this.direction = 0f;
		}
		return this;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x0000502C File Offset: 0x0000322C
	public LTDescr resume()
	{
		this.direction = this.directionLast;
		return this;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0000503C File Offset: 0x0000323C
	public LTDescr setAxis(Vector3 axis)
	{
		this.axis = axis;
		return this;
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00005048 File Offset: 0x00003248
	public LTDescr setDelay(float delay)
	{
		if (this.useEstimatedTime)
		{
			this.delay = delay;
		}
		else
		{
			this.delay = delay;
		}
		return this;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x0000506C File Offset: 0x0000326C
	public LTDescr setEase(LeanTweenType easeType)
	{
		this.tweenType = easeType;
		return this;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00005078 File Offset: 0x00003278
	public LTDescr setOvershoot(float overshoot)
	{
		this.overshoot = overshoot;
		return this;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00005084 File Offset: 0x00003284
	public LTDescr setPeriod(float period)
	{
		this.period = period;
		return this;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00005090 File Offset: 0x00003290
	public LTDescr setEase(AnimationCurve easeCurve)
	{
		this.animationCurve = easeCurve;
		return this;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0000509C File Offset: 0x0000329C
	public LTDescr setTo(Vector3 to)
	{
		if (this.hasInitiliazed)
		{
			this.to = to;
			this.diff = to - this.from;
		}
		else
		{
			this.to = to;
		}
		return this;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x000050D0 File Offset: 0x000032D0
	public LTDescr setFrom(Vector3 from)
	{
		if (this.trans)
		{
			this.init();
		}
		this.from = from;
		this.diff = this.to - this.from;
		return this;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00005108 File Offset: 0x00003308
	public LTDescr setFrom(float from)
	{
		return this.setFrom(new Vector3(from, 0f, 0f));
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00005120 File Offset: 0x00003320
	public LTDescr setDiff(Vector3 diff)
	{
		this.diff = diff;
		return this;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x0000512C File Offset: 0x0000332C
	public LTDescr setHasInitialized(bool has)
	{
		this.hasInitiliazed = has;
		return this;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00005138 File Offset: 0x00003338
	public LTDescr setId(uint id)
	{
		this._id = id;
		this.counter = LTDescr.global_counter;
		return this;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00005150 File Offset: 0x00003350
	public LTDescr setTime(float time)
	{
		this.time = time;
		return this;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x0000515C File Offset: 0x0000335C
	public LTDescr setRepeat(int repeat)
	{
		this.loopCount = repeat;
		if ((repeat > 1 && this.loopType == LeanTweenType.once) || (repeat < 0 && this.loopType == LeanTweenType.once))
		{
			this.loopType = LeanTweenType.clamp;
		}
		if (this.type == TweenAction.CALLBACK || this.type == TweenAction.CALLBACK_COLOR)
		{
			this.setOnCompleteOnRepeat(true);
		}
		return this;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x000051C4 File Offset: 0x000033C4
	public LTDescr setLoopType(LeanTweenType loopType)
	{
		this.loopType = loopType;
		return this;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x000051D0 File Offset: 0x000033D0
	public LTDescr setUseEstimatedTime(bool useEstimatedTime)
	{
		this.useEstimatedTime = useEstimatedTime;
		return this;
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x000051DC File Offset: 0x000033DC
	public LTDescr setIgnoreTimeScale(bool useUnScaledTime)
	{
		this.useEstimatedTime = useUnScaledTime;
		return this;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x000051E8 File Offset: 0x000033E8
	public LTDescr setUseFrames(bool useFrames)
	{
		this.useFrames = useFrames;
		return this;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x000051F4 File Offset: 0x000033F4
	public LTDescr setUseManualTime(bool useManualTime)
	{
		this.useManualTime = useManualTime;
		return this;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00005200 File Offset: 0x00003400
	public LTDescr setLoopCount(int loopCount)
	{
		this.loopCount = loopCount;
		return this;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x0000520C File Offset: 0x0000340C
	public LTDescr setLoopOnce()
	{
		this.loopType = LeanTweenType.once;
		return this;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00005218 File Offset: 0x00003418
	public LTDescr setLoopClamp()
	{
		this.loopType = LeanTweenType.clamp;
		if (this.loopCount == 0)
		{
			this.loopCount = -1;
		}
		return this;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00005238 File Offset: 0x00003438
	public LTDescr setLoopClamp(int loops)
	{
		this.loopCount = loops;
		return this;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005244 File Offset: 0x00003444
	public LTDescr setLoopPingPong()
	{
		this.loopType = LeanTweenType.pingPong;
		if (this.loopCount == 0)
		{
			this.loopCount = -1;
		}
		return this;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00005264 File Offset: 0x00003464
	public LTDescr setLoopPingPong(int loops)
	{
		this.loopType = LeanTweenType.pingPong;
		this.loopCount = ((loops != -1) ? (loops * 2) : loops);
		return this;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00005288 File Offset: 0x00003488
	public LTDescr setOnComplete(Action onComplete)
	{
		this.onComplete = onComplete;
		return this;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00005294 File Offset: 0x00003494
	public LTDescr setOnComplete(Action<object> onComplete)
	{
		this.onCompleteObject = onComplete;
		return this;
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x000052A0 File Offset: 0x000034A0
	public LTDescr setOnComplete(Action<object> onComplete, object onCompleteParam)
	{
		this.onCompleteObject = onComplete;
		if (onCompleteParam != null)
		{
			this.onCompleteParam = onCompleteParam;
		}
		return this;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x000052B8 File Offset: 0x000034B8
	public LTDescr setOnCompleteParam(object onCompleteParam)
	{
		this.onCompleteParam = onCompleteParam;
		return this;
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x000052C4 File Offset: 0x000034C4
	public LTDescr setOnUpdate(Action<float> onUpdate)
	{
		this.onUpdateFloat = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x000052D8 File Offset: 0x000034D8
	public LTDescr setOnUpdateRatio(Action<float, float> onUpdate)
	{
		this.onUpdateFloatRatio = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x000052EC File Offset: 0x000034EC
	public LTDescr setOnUpdateObject(Action<float, object> onUpdate)
	{
		this.onUpdateFloatObject = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00005300 File Offset: 0x00003500
	public LTDescr setOnUpdateVector2(Action<Vector2> onUpdate)
	{
		this.onUpdateVector2 = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00005314 File Offset: 0x00003514
	public LTDescr setOnUpdateVector3(Action<Vector3> onUpdate)
	{
		this.onUpdateVector3 = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00005328 File Offset: 0x00003528
	public LTDescr setOnUpdateColor(Action<Color> onUpdate)
	{
		this.onUpdateColor = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x0000533C File Offset: 0x0000353C
	public LTDescr setOnUpdate(Action<Color> onUpdate)
	{
		this.onUpdateColor = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00005350 File Offset: 0x00003550
	public LTDescr setOnUpdate(Action<float, object> onUpdate, object onUpdateParam = null)
	{
		this.onUpdateFloatObject = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00005370 File Offset: 0x00003570
	public LTDescr setOnUpdate(Action<Vector3, object> onUpdate, object onUpdateParam = null)
	{
		this.onUpdateVector3Object = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00005390 File Offset: 0x00003590
	public LTDescr setOnUpdate(Action<Vector2> onUpdate, object onUpdateParam = null)
	{
		this.onUpdateVector2 = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x000053B0 File Offset: 0x000035B0
	public LTDescr setOnUpdate(Action<Vector3> onUpdate, object onUpdateParam = null)
	{
		this.onUpdateVector3 = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x000053D0 File Offset: 0x000035D0
	public LTDescr setOnUpdateParam(object onUpdateParam)
	{
		this.onUpdateParam = onUpdateParam;
		return this;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x000053DC File Offset: 0x000035DC
	public LTDescr setOrientToPath(bool doesOrient)
	{
		if (this.type == TweenAction.MOVE_CURVED || this.type == TweenAction.MOVE_CURVED_LOCAL)
		{
			if (this.path == null)
			{
				this.path = new LTBezierPath();
			}
			this.path.orientToPath = doesOrient;
		}
		else
		{
			this.spline.orientToPath = doesOrient;
		}
		return this;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00005438 File Offset: 0x00003638
	public LTDescr setOrientToPath2d(bool doesOrient2d)
	{
		this.setOrientToPath(doesOrient2d);
		if (this.type == TweenAction.MOVE_CURVED || this.type == TweenAction.MOVE_CURVED_LOCAL)
		{
			this.path.orientToPath2d = doesOrient2d;
		}
		else
		{
			this.spline.orientToPath2d = doesOrient2d;
		}
		return this;
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00005484 File Offset: 0x00003684
	public LTDescr setRect(LTRect rect)
	{
		this.ltRect = rect;
		return this;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00005490 File Offset: 0x00003690
	public LTDescr setRect(Rect rect)
	{
		this.ltRect = new LTRect(rect);
		return this;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x000054A0 File Offset: 0x000036A0
	public LTDescr setPath(LTBezierPath path)
	{
		this.path = path;
		return this;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000054AC File Offset: 0x000036AC
	public LTDescr setPoint(Vector3 point)
	{
		this.point = point;
		return this;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x000054B8 File Offset: 0x000036B8
	public LTDescr setDestroyOnComplete(bool doesDestroy)
	{
		this.destroyOnComplete = doesDestroy;
		return this;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x000054C4 File Offset: 0x000036C4
	public LTDescr setAudio(object audio)
	{
		this.onCompleteParam = audio;
		return this;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x000054D0 File Offset: 0x000036D0
	public LTDescr setOnCompleteOnRepeat(bool isOn)
	{
		this.onCompleteOnRepeat = isOn;
		return this;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x000054DC File Offset: 0x000036DC
	public LTDescr setOnCompleteOnStart(bool isOn)
	{
		this.onCompleteOnStart = isOn;
		return this;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x000054E8 File Offset: 0x000036E8
	public LTDescr setRect(RectTransform rect)
	{
		this.rectTransform = rect;
		return this;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000054F4 File Offset: 0x000036F4
	public LTDescr setSprites(Sprite[] sprites)
	{
		this.sprites = sprites;
		return this;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00005500 File Offset: 0x00003700
	public LTDescr setFrameRate(float frameRate)
	{
		this.time = (float)this.sprites.Length / frameRate;
		return this;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00005514 File Offset: 0x00003714
	public LTDescr setOnStart(Action onStart)
	{
		this.onStart = onStart;
		return this;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00005520 File Offset: 0x00003720
	public LTDescr setDirection(float direction)
	{
		if (this.direction != -1f && this.direction != 1f)
		{
			Debug.LogWarning("You have passed an incorrect direction of '" + direction + "', direction must be -1f or 1f");
			return this;
		}
		if (this.direction != direction)
		{
			if (this.path != null)
			{
				this.path = new LTBezierPath(LTUtility.reverse(this.path.pts));
			}
			else if (this.spline != null)
			{
				this.spline = new LTSpline(LTUtility.reverse(this.spline.pts));
			}
		}
		return this;
	}

	// Token: 0x04000092 RID: 146
	public bool toggle;

	// Token: 0x04000093 RID: 147
	public bool useEstimatedTime;

	// Token: 0x04000094 RID: 148
	public bool useFrames;

	// Token: 0x04000095 RID: 149
	public bool useManualTime;

	// Token: 0x04000096 RID: 150
	public bool hasInitiliazed;

	// Token: 0x04000097 RID: 151
	public bool hasPhysics;

	// Token: 0x04000098 RID: 152
	public bool onCompleteOnRepeat;

	// Token: 0x04000099 RID: 153
	public bool onCompleteOnStart;

	// Token: 0x0400009A RID: 154
	public float passed;

	// Token: 0x0400009B RID: 155
	public float delay;

	// Token: 0x0400009C RID: 156
	public float time;

	// Token: 0x0400009D RID: 157
	public float lastVal;

	// Token: 0x0400009E RID: 158
	private uint _id;

	// Token: 0x0400009F RID: 159
	public int loopCount;

	// Token: 0x040000A0 RID: 160
	public uint counter;

	// Token: 0x040000A1 RID: 161
	public float direction;

	// Token: 0x040000A2 RID: 162
	public float directionLast;

	// Token: 0x040000A3 RID: 163
	public float overshoot;

	// Token: 0x040000A4 RID: 164
	public float period;

	// Token: 0x040000A5 RID: 165
	public bool destroyOnComplete;

	// Token: 0x040000A6 RID: 166
	public Transform trans;

	// Token: 0x040000A7 RID: 167
	public LTRect ltRect;

	// Token: 0x040000A8 RID: 168
	public Vector3 from;

	// Token: 0x040000A9 RID: 169
	public Vector3 to;

	// Token: 0x040000AA RID: 170
	public Vector3 diff;

	// Token: 0x040000AB RID: 171
	public Vector3 point;

	// Token: 0x040000AC RID: 172
	public Vector3 axis;

	// Token: 0x040000AD RID: 173
	public Quaternion origRotation;

	// Token: 0x040000AE RID: 174
	public LTBezierPath path;

	// Token: 0x040000AF RID: 175
	public LTSpline spline;

	// Token: 0x040000B0 RID: 176
	public TweenAction type;

	// Token: 0x040000B1 RID: 177
	public LeanTweenType tweenType;

	// Token: 0x040000B2 RID: 178
	public AnimationCurve animationCurve;

	// Token: 0x040000B3 RID: 179
	public LeanTweenType loopType;

	// Token: 0x040000B4 RID: 180
	public bool hasUpdateCallback;

	// Token: 0x040000B5 RID: 181
	public Action<float> onUpdateFloat;

	// Token: 0x040000B6 RID: 182
	public Action<float, float> onUpdateFloatRatio;

	// Token: 0x040000B7 RID: 183
	public Action<float, object> onUpdateFloatObject;

	// Token: 0x040000B8 RID: 184
	public Action<Vector2> onUpdateVector2;

	// Token: 0x040000B9 RID: 185
	public Action<Vector3> onUpdateVector3;

	// Token: 0x040000BA RID: 186
	public Action<Vector3, object> onUpdateVector3Object;

	// Token: 0x040000BB RID: 187
	public Action<Color> onUpdateColor;

	// Token: 0x040000BC RID: 188
	public Action onComplete;

	// Token: 0x040000BD RID: 189
	public Action<object> onCompleteObject;

	// Token: 0x040000BE RID: 190
	public object onCompleteParam;

	// Token: 0x040000BF RID: 191
	public object onUpdateParam;

	// Token: 0x040000C0 RID: 192
	public Action onStart;

	// Token: 0x040000C1 RID: 193
	public RectTransform rectTransform;

	// Token: 0x040000C2 RID: 194
	public Text uiText;

	// Token: 0x040000C3 RID: 195
	public Image uiImage;

	// Token: 0x040000C4 RID: 196
	public Sprite[] sprites;

	// Token: 0x040000C5 RID: 197
	private static uint global_counter;
}
