using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000017 RID: 23
public class LeanTween : MonoBehaviour
{
	// Token: 0x060000F2 RID: 242 RVA: 0x0000581C File Offset: 0x00003A1C
	public static void init()
	{
		LeanTween.init(LeanTween.maxTweens);
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005828 File Offset: 0x00003A28
	public static int maxSearch
	{
		get
		{
			return LeanTween.tweenMaxSearch;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060000F4 RID: 244 RVA: 0x00005830 File Offset: 0x00003A30
	public static int tweensRunning
	{
		get
		{
			int num = 0;
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].toggle)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0000586C File Offset: 0x00003A6C
	public static void init(int maxSimultaneousTweens)
	{
		if (LeanTween.tweens == null)
		{
			LeanTween.maxTweens = maxSimultaneousTweens;
			LeanTween.tweens = new LTDescr[LeanTween.maxTweens];
			LeanTween.tweensFinished = new int[LeanTween.maxTweens];
			LeanTween._tweenEmpty = new GameObject();
			LeanTween._tweenEmpty.name = "~LeanTween";
			LeanTween._tweenEmpty.AddComponent(typeof(LeanTween));
			LeanTween._tweenEmpty.isStatic = true;
			LeanTween._tweenEmpty.hideFlags = HideFlags.HideAndDontSave;
			global::UnityEngine.Object.DontDestroyOnLoad(LeanTween._tweenEmpty);
			for (int i = 0; i < LeanTween.maxTweens; i++)
			{
				LeanTween.tweens[i] = new LTDescr();
			}
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00005918 File Offset: 0x00003B18
	public static void reset()
	{
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LeanTween.tweens[i].toggle = false;
		}
		LeanTween.tweens = null;
		global::UnityEngine.Object.Destroy(LeanTween._tweenEmpty);
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00005958 File Offset: 0x00003B58
	public void Update()
	{
		LeanTween.update();
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00005960 File Offset: 0x00003B60
	public void OnLevelWasLoaded(int lvl)
	{
		LTGUI.reset();
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00005968 File Offset: 0x00003B68
	public static void update()
	{
		if (LeanTween.frameRendered != Time.frameCount)
		{
			LeanTween.init();
			if (LeanTween.dtEstimated < 0f)
			{
				LeanTween.dtEstimated = 0f;
			}
			else
			{
				LeanTween.dtEstimated = Time.unscaledDeltaTime;
			}
			LeanTween.dtActual = Time.deltaTime;
			LeanTween.maxTweenReached = 0;
			LeanTween.finishedCnt = 0;
			int num = 0;
			while (num <= LeanTween.tweenMaxSearch && num < LeanTween.maxTweens)
			{
				if (LeanTween.tweens[num].toggle)
				{
					LeanTween.maxTweenReached = num;
					LeanTween.tween = LeanTween.tweens[num];
					LeanTween.trans = LeanTween.tween.trans;
					LeanTween.timeTotal = LeanTween.tween.time;
					LeanTween.tweenAction = LeanTween.tween.type;
					if (LeanTween.tween.useEstimatedTime)
					{
						LeanTween.dt = LeanTween.dtEstimated;
					}
					else if (LeanTween.tween.useFrames)
					{
						LeanTween.dt = 1f;
					}
					else if (LeanTween.tween.useManualTime)
					{
						LeanTween.dt = LeanTween.dtManual;
					}
					else if (LeanTween.tween.direction == 0f)
					{
						LeanTween.dt = 0f;
					}
					else
					{
						LeanTween.dt = LeanTween.dtActual;
					}
					if (LeanTween.trans == null)
					{
						LeanTween.removeTween(num);
					}
					else
					{
						LeanTween.isTweenFinished = false;
						if (LeanTween.tween.delay <= 0f)
						{
							if (LeanTween.tween.passed + LeanTween.dt > LeanTween.tween.time && LeanTween.tween.direction > 0f)
							{
								LeanTween.isTweenFinished = true;
								LeanTween.tween.passed = LeanTween.tween.time;
							}
							else if (LeanTween.tween.direction < 0f && LeanTween.tween.passed - LeanTween.dt < 0f)
							{
								LeanTween.isTweenFinished = true;
								LeanTween.tween.passed = Mathf.Epsilon;
							}
						}
						if (!LeanTween.tween.hasInitiliazed && (((double)LeanTween.tween.passed == 0.0 && (double)LeanTween.tween.delay == 0.0) || (double)LeanTween.tween.passed > 0.0))
						{
							LeanTween.tween.init();
						}
						if (LeanTween.tween.delay <= 0f)
						{
							if (LeanTween.timeTotal <= 0f)
							{
								LeanTween.ratioPassed = 1f;
							}
							else
							{
								LeanTween.ratioPassed = LeanTween.tween.passed / LeanTween.timeTotal;
							}
							if (LeanTween.ratioPassed > 1f)
							{
								LeanTween.ratioPassed = 1f;
							}
							else if (LeanTween.ratioPassed < 0f)
							{
								LeanTween.ratioPassed = 0f;
							}
							if (LeanTween.tweenAction >= TweenAction.MOVE_X && LeanTween.tweenAction < TweenAction.MOVE)
							{
								if (LeanTween.tween.animationCurve != null)
								{
									LeanTween.val = LeanTween.tweenOnCurve(LeanTween.tween, LeanTween.ratioPassed);
								}
								else
								{
									switch (LeanTween.tween.tweenType)
									{
									case LeanTweenType.linear:
										LeanTween.val = LeanTween.tween.from.x + LeanTween.tween.diff.x * LeanTween.ratioPassed;
										break;
									case LeanTweenType.easeOutQuad:
										LeanTween.val = LeanTween.easeOutQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInQuad:
										LeanTween.val = LeanTween.easeInQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInOutQuad:
										LeanTween.val = LeanTween.easeInOutQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInCubic:
										LeanTween.val = LeanTween.easeInCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeOutCubic:
										LeanTween.val = LeanTween.easeOutCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInOutCubic:
										LeanTween.val = LeanTween.easeInOutCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInQuart:
										LeanTween.val = LeanTween.easeInQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeOutQuart:
										LeanTween.val = LeanTween.easeOutQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInOutQuart:
										LeanTween.val = LeanTween.easeInOutQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInQuint:
										LeanTween.val = LeanTween.easeInQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeOutQuint:
										LeanTween.val = LeanTween.easeOutQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInOutQuint:
										LeanTween.val = LeanTween.easeInOutQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInSine:
										LeanTween.val = LeanTween.easeInSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeOutSine:
										LeanTween.val = LeanTween.easeOutSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInOutSine:
										LeanTween.val = LeanTween.easeInOutSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInExpo:
										LeanTween.val = LeanTween.easeInExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeOutExpo:
										LeanTween.val = LeanTween.easeOutExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInOutExpo:
										LeanTween.val = LeanTween.easeInOutExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInCirc:
										LeanTween.val = LeanTween.easeInCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeOutCirc:
										LeanTween.val = LeanTween.easeOutCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInOutCirc:
										LeanTween.val = LeanTween.easeInOutCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInBounce:
										LeanTween.val = LeanTween.easeInBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeOutBounce:
										LeanTween.val = LeanTween.easeOutBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInOutBounce:
										LeanTween.val = LeanTween.easeInOutBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeInBack:
										LeanTween.val = LeanTween.easeInBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot);
										break;
									case LeanTweenType.easeOutBack:
										LeanTween.val = LeanTween.easeOutBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot);
										break;
									case LeanTweenType.easeInOutBack:
										LeanTween.val = LeanTween.easeInOutBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot);
										break;
									case LeanTweenType.easeInElastic:
										LeanTween.val = LeanTween.easeInElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period);
										break;
									case LeanTweenType.easeOutElastic:
										LeanTween.val = LeanTween.easeOutElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period);
										break;
									case LeanTweenType.easeInOutElastic:
										LeanTween.val = LeanTween.easeInOutElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period);
										break;
									case LeanTweenType.easeSpring:
										LeanTween.val = LeanTween.spring(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
										break;
									case LeanTweenType.easeShake:
									case LeanTweenType.punch:
										if (LeanTween.tween.tweenType == LeanTweenType.punch)
										{
											LeanTween.tween.animationCurve = LeanTween.punch;
										}
										else if (LeanTween.tween.tweenType == LeanTweenType.easeShake)
										{
											LeanTween.tween.animationCurve = LeanTween.shake;
										}
										LeanTween.tween.to.x = LeanTween.tween.from.x + LeanTween.tween.to.x;
										LeanTween.tween.diff.x = LeanTween.tween.to.x - LeanTween.tween.from.x;
										LeanTween.val = LeanTween.tweenOnCurve(LeanTween.tween, LeanTween.ratioPassed);
										break;
									default:
										LeanTween.val = LeanTween.tween.from.x + LeanTween.tween.diff.x * LeanTween.ratioPassed;
										break;
									}
								}
								if (LeanTween.tweenAction == TweenAction.MOVE_X)
								{
									LeanTween.trans.position = new Vector3(LeanTween.val, LeanTween.trans.position.y, LeanTween.trans.position.z);
								}
								else if (LeanTween.tweenAction == TweenAction.MOVE_Y)
								{
									LeanTween.trans.position = new Vector3(LeanTween.trans.position.x, LeanTween.val, LeanTween.trans.position.z);
								}
								else if (LeanTween.tweenAction == TweenAction.MOVE_Z)
								{
									LeanTween.trans.position = new Vector3(LeanTween.trans.position.x, LeanTween.trans.position.y, LeanTween.val);
								}
								if (LeanTween.tweenAction == TweenAction.MOVE_LOCAL_X)
								{
									LeanTween.trans.localPosition = new Vector3(LeanTween.val, LeanTween.trans.localPosition.y, LeanTween.trans.localPosition.z);
								}
								else if (LeanTween.tweenAction == TweenAction.MOVE_LOCAL_Y)
								{
									LeanTween.trans.localPosition = new Vector3(LeanTween.trans.localPosition.x, LeanTween.val, LeanTween.trans.localPosition.z);
								}
								else if (LeanTween.tweenAction == TweenAction.MOVE_LOCAL_Z)
								{
									LeanTween.trans.localPosition = new Vector3(LeanTween.trans.localPosition.x, LeanTween.trans.localPosition.y, LeanTween.val);
								}
								else if (LeanTween.tweenAction == TweenAction.MOVE_CURVED)
								{
									if (LeanTween.tween.path.orientToPath)
									{
										if (LeanTween.tween.path.orientToPath2d)
										{
											LeanTween.tween.path.place2d(LeanTween.trans, LeanTween.val);
										}
										else
										{
											LeanTween.tween.path.place(LeanTween.trans, LeanTween.val);
										}
									}
									else
									{
										LeanTween.trans.position = LeanTween.tween.path.point(LeanTween.val);
									}
								}
								else if (LeanTween.tweenAction == TweenAction.MOVE_CURVED_LOCAL)
								{
									if (LeanTween.tween.path.orientToPath)
									{
										if (LeanTween.tween.path.orientToPath2d)
										{
											LeanTween.tween.path.placeLocal2d(LeanTween.trans, LeanTween.val);
										}
										else
										{
											LeanTween.tween.path.placeLocal(LeanTween.trans, LeanTween.val);
										}
									}
									else
									{
										LeanTween.trans.localPosition = LeanTween.tween.path.point(LeanTween.val);
									}
								}
								else if (LeanTween.tweenAction == TweenAction.MOVE_SPLINE)
								{
									if (LeanTween.tween.spline.orientToPath)
									{
										if (LeanTween.tween.spline.orientToPath2d)
										{
											LeanTween.tween.spline.place2d(LeanTween.trans, LeanTween.val);
										}
										else
										{
											LeanTween.tween.spline.place(LeanTween.trans, LeanTween.val);
										}
									}
									else
									{
										LeanTween.trans.position = LeanTween.tween.spline.point(LeanTween.val);
									}
								}
								else if (LeanTween.tweenAction == TweenAction.MOVE_SPLINE_LOCAL)
								{
									if (LeanTween.tween.spline.orientToPath)
									{
										if (LeanTween.tween.spline.orientToPath2d)
										{
											LeanTween.tween.spline.placeLocal2d(LeanTween.trans, LeanTween.val);
										}
										else
										{
											LeanTween.tween.spline.placeLocal(LeanTween.trans, LeanTween.val);
										}
									}
									else
									{
										LeanTween.trans.localPosition = LeanTween.tween.spline.point(LeanTween.val);
									}
								}
								else if (LeanTween.tweenAction == TweenAction.SCALE_X)
								{
									LeanTween.trans.localScale = new Vector3(LeanTween.val, LeanTween.trans.localScale.y, LeanTween.trans.localScale.z);
								}
								else if (LeanTween.tweenAction == TweenAction.SCALE_Y)
								{
									LeanTween.trans.localScale = new Vector3(LeanTween.trans.localScale.x, LeanTween.val, LeanTween.trans.localScale.z);
								}
								else if (LeanTween.tweenAction == TweenAction.SCALE_Z)
								{
									LeanTween.trans.localScale = new Vector3(LeanTween.trans.localScale.x, LeanTween.trans.localScale.y, LeanTween.val);
								}
								else if (LeanTween.tweenAction == TweenAction.ROTATE_X)
								{
									LeanTween.trans.eulerAngles = new Vector3(LeanTween.val, LeanTween.trans.eulerAngles.y, LeanTween.trans.eulerAngles.z);
								}
								else if (LeanTween.tweenAction == TweenAction.ROTATE_Y)
								{
									LeanTween.trans.eulerAngles = new Vector3(LeanTween.trans.eulerAngles.x, LeanTween.val, LeanTween.trans.eulerAngles.z);
								}
								else if (LeanTween.tweenAction == TweenAction.ROTATE_Z)
								{
									LeanTween.trans.eulerAngles = new Vector3(LeanTween.trans.eulerAngles.x, LeanTween.trans.eulerAngles.y, LeanTween.val);
								}
								else if (LeanTween.tweenAction == TweenAction.ROTATE_AROUND)
								{
									Vector3 localPosition = LeanTween.trans.localPosition;
									Vector3 vector = LeanTween.trans.TransformPoint(LeanTween.tween.point);
									LeanTween.trans.RotateAround(vector, LeanTween.tween.axis, -LeanTween.val);
									Vector3 vector2 = localPosition - LeanTween.trans.localPosition;
									LeanTween.trans.localPosition = localPosition - vector2;
									LeanTween.trans.rotation = LeanTween.tween.origRotation;
									vector = LeanTween.trans.TransformPoint(LeanTween.tween.point);
									LeanTween.trans.RotateAround(vector, LeanTween.tween.axis, LeanTween.val);
								}
								else if (LeanTween.tweenAction == TweenAction.ROTATE_AROUND_LOCAL)
								{
									Vector3 localPosition2 = LeanTween.trans.localPosition;
									LeanTween.trans.RotateAround(LeanTween.trans.TransformPoint(LeanTween.tween.point), LeanTween.trans.TransformDirection(LeanTween.tween.axis), -LeanTween.val);
									Vector3 vector3 = localPosition2 - LeanTween.trans.localPosition;
									LeanTween.trans.localPosition = localPosition2 - vector3;
									LeanTween.trans.localRotation = LeanTween.tween.origRotation;
									Vector3 vector4 = LeanTween.trans.TransformPoint(LeanTween.tween.point);
									LeanTween.trans.RotateAround(vector4, LeanTween.trans.TransformDirection(LeanTween.tween.axis), LeanTween.val);
								}
								else if (LeanTween.tweenAction == TweenAction.ALPHA)
								{
									SpriteRenderer component = LeanTween.trans.gameObject.GetComponent<SpriteRenderer>();
									if (component != null)
									{
										component.color = new Color(component.color.r, component.color.g, component.color.b, LeanTween.val);
									}
									else
									{
										if (LeanTween.trans.gameObject.GetComponent<Renderer>() != null)
										{
											foreach (Material material in LeanTween.trans.gameObject.GetComponent<Renderer>().materials)
											{
												if (material.HasProperty("_Color"))
												{
													material.color = new Color(material.color.r, material.color.g, material.color.b, LeanTween.val);
												}
												else if (material.HasProperty("_TintColor"))
												{
													Color color = material.GetColor("_TintColor");
													material.SetColor("_TintColor", new Color(color.r, color.g, color.b, LeanTween.val));
												}
											}
										}
										if (LeanTween.trans.childCount > 0)
										{
											foreach (object obj in LeanTween.trans)
											{
												Transform transform = (Transform)obj;
												if (transform.gameObject.GetComponent<Renderer>() != null)
												{
													foreach (Material material2 in transform.gameObject.GetComponent<Renderer>().materials)
													{
														material2.color = new Color(material2.color.r, material2.color.g, material2.color.b, LeanTween.val);
													}
												}
											}
										}
									}
								}
								else if (LeanTween.tweenAction == TweenAction.ALPHA_VERTEX)
								{
									Mesh mesh = LeanTween.trans.GetComponent<MeshFilter>().mesh;
									Vector3[] vertices = mesh.vertices;
									Color32[] array = new Color32[vertices.Length];
									Color32 color2 = mesh.colors32[0];
									color2 = new Color((float)color2.r, (float)color2.g, (float)color2.b, LeanTween.val);
									for (int k = 0; k < vertices.Length; k++)
									{
										array[k] = color2;
									}
									mesh.colors32 = array;
								}
								else if (LeanTween.tweenAction == TweenAction.COLOR || LeanTween.tweenAction == TweenAction.CALLBACK_COLOR)
								{
									Color color3 = LeanTween.tweenColor(LeanTween.tween, LeanTween.val);
									SpriteRenderer component2 = LeanTween.trans.gameObject.GetComponent<SpriteRenderer>();
									if (component2 != null)
									{
										component2.color = color3;
									}
									else if (LeanTween.tweenAction == TweenAction.COLOR)
									{
										if (LeanTween.trans.gameObject.GetComponent<Renderer>() != null)
										{
											foreach (Material material3 in LeanTween.trans.gameObject.GetComponent<Renderer>().materials)
											{
												material3.color = color3;
											}
										}
										if (LeanTween.trans.childCount > 0)
										{
											foreach (object obj2 in LeanTween.trans)
											{
												Transform transform2 = (Transform)obj2;
												if (transform2.gameObject.GetComponent<Renderer>() != null)
												{
													foreach (Material material4 in transform2.gameObject.GetComponent<Renderer>().materials)
													{
														material4.color = color3;
													}
												}
											}
										}
									}
									if (LeanTween.dt != 0f && LeanTween.tween.onUpdateColor != null)
									{
										LeanTween.tween.onUpdateColor(color3);
									}
								}
								else if (LeanTween.tweenAction == TweenAction.CANVAS_ALPHA)
								{
									Color color4 = LeanTween.tween.uiImage.color;
									color4.a = LeanTween.val;
									LeanTween.tween.uiImage.color = color4;
								}
								else if (LeanTween.tweenAction == TweenAction.CANVAS_COLOR)
								{
									Color color5 = LeanTween.tweenColor(LeanTween.tween, LeanTween.val);
									LeanTween.tween.uiImage.color = color5;
									if (LeanTween.dt != 0f && LeanTween.tween.onUpdateColor != null)
									{
										LeanTween.tween.onUpdateColor(color5);
									}
								}
								else if (LeanTween.tweenAction == TweenAction.TEXT_ALPHA)
								{
									LeanTween.textAlphaRecursive(LeanTween.trans, LeanTween.val);
								}
								else if (LeanTween.tweenAction == TweenAction.TEXT_COLOR)
								{
									Color color6 = LeanTween.tweenColor(LeanTween.tween, LeanTween.val);
									LeanTween.tween.uiText.color = color6;
									if (LeanTween.dt != 0f && LeanTween.tween.onUpdateColor != null)
									{
										LeanTween.tween.onUpdateColor(color6);
									}
									if (LeanTween.trans.childCount > 0)
									{
										foreach (object obj3 in LeanTween.trans)
										{
											Transform transform3 = (Transform)obj3;
											Text component3 = transform3.gameObject.GetComponent<Text>();
											if (component3 != null)
											{
												component3.color = color6;
											}
										}
									}
								}
								else if (LeanTween.tweenAction == TweenAction.CANVAS_ROTATEAROUND)
								{
									RectTransform rectTransform = LeanTween.tween.rectTransform;
									Vector3 localPosition3 = rectTransform.localPosition;
									rectTransform.RotateAround(rectTransform.TransformPoint(LeanTween.tween.point), LeanTween.tween.axis, -LeanTween.val);
									Vector3 vector5 = localPosition3 - rectTransform.localPosition;
									rectTransform.localPosition = localPosition3 - vector5;
									rectTransform.rotation = LeanTween.tween.origRotation;
									rectTransform.RotateAround(rectTransform.TransformPoint(LeanTween.tween.point), LeanTween.tween.axis, LeanTween.val);
								}
								else if (LeanTween.tweenAction == TweenAction.CANVAS_ROTATEAROUND_LOCAL)
								{
									RectTransform rectTransform2 = LeanTween.tween.rectTransform;
									Vector3 localPosition4 = rectTransform2.localPosition;
									rectTransform2.RotateAround(rectTransform2.TransformPoint(LeanTween.tween.point), rectTransform2.TransformDirection(LeanTween.tween.axis), -LeanTween.val);
									Vector3 vector6 = localPosition4 - rectTransform2.localPosition;
									rectTransform2.localPosition = localPosition4 - vector6;
									rectTransform2.rotation = LeanTween.tween.origRotation;
									rectTransform2.RotateAround(rectTransform2.TransformPoint(LeanTween.tween.point), rectTransform2.TransformDirection(LeanTween.tween.axis), LeanTween.val);
								}
								else if (LeanTween.tweenAction == TweenAction.CANVAS_PLAYSPRITE)
								{
									int num2 = (int)Mathf.Round(LeanTween.val);
									LeanTween.tween.uiImage.sprite = LeanTween.tween.sprites[num2];
								}
								else if (LeanTween.tweenAction == TweenAction.CANVAS_MOVE_X)
								{
									Vector3 anchoredPosition3D = LeanTween.tween.rectTransform.anchoredPosition3D;
									LeanTween.tween.rectTransform.anchoredPosition3D = new Vector3(LeanTween.val, anchoredPosition3D.y, anchoredPosition3D.z);
								}
								else if (LeanTween.tweenAction == TweenAction.CANVAS_MOVE_Y)
								{
									Vector3 anchoredPosition3D2 = LeanTween.tween.rectTransform.anchoredPosition3D;
									LeanTween.tween.rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D2.x, LeanTween.val, anchoredPosition3D2.z);
								}
								else if (LeanTween.tweenAction == TweenAction.CANVAS_MOVE_Z)
								{
									Vector3 anchoredPosition3D3 = LeanTween.tween.rectTransform.anchoredPosition3D;
									LeanTween.tween.rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D3.x, anchoredPosition3D3.y, LeanTween.val);
								}
							}
							else if (LeanTween.tweenAction >= TweenAction.MOVE)
							{
								if (LeanTween.tween.animationCurve != null)
								{
									LeanTween.newVect = LeanTween.tweenOnCurveVector(LeanTween.tween, LeanTween.ratioPassed);
								}
								else if (LeanTween.tween.tweenType == LeanTweenType.linear)
								{
									LeanTween.newVect = new Vector3(LeanTween.tween.from.x + LeanTween.tween.diff.x * LeanTween.ratioPassed, LeanTween.tween.from.y + LeanTween.tween.diff.y * LeanTween.ratioPassed, LeanTween.tween.from.z + LeanTween.tween.diff.z * LeanTween.ratioPassed);
								}
								else if (LeanTween.tween.tweenType >= LeanTweenType.linear)
								{
									switch (LeanTween.tween.tweenType)
									{
									case LeanTweenType.easeOutQuad:
										LeanTween.newVect = new Vector3(LeanTween.easeOutQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed), LeanTween.easeOutQuadOpt(LeanTween.tween.from.y, LeanTween.tween.diff.y, LeanTween.ratioPassed), LeanTween.easeOutQuadOpt(LeanTween.tween.from.z, LeanTween.tween.diff.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInQuad:
										LeanTween.newVect = new Vector3(LeanTween.easeInQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed), LeanTween.easeInQuadOpt(LeanTween.tween.from.y, LeanTween.tween.diff.y, LeanTween.ratioPassed), LeanTween.easeInQuadOpt(LeanTween.tween.from.z, LeanTween.tween.diff.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInOutQuad:
										LeanTween.newVect = new Vector3(LeanTween.easeInOutQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed), LeanTween.easeInOutQuadOpt(LeanTween.tween.from.y, LeanTween.tween.diff.y, LeanTween.ratioPassed), LeanTween.easeInOutQuadOpt(LeanTween.tween.from.z, LeanTween.tween.diff.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInCubic:
										LeanTween.newVect = new Vector3(LeanTween.easeInCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInCubic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInCubic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeOutCubic:
										LeanTween.newVect = new Vector3(LeanTween.easeOutCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutCubic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutCubic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInOutCubic:
										LeanTween.newVect = new Vector3(LeanTween.easeInOutCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutCubic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutCubic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInQuart:
										LeanTween.newVect = new Vector3(LeanTween.easeInQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInQuart(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInQuart(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeOutQuart:
										LeanTween.newVect = new Vector3(LeanTween.easeOutQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutQuart(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutQuart(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInOutQuart:
										LeanTween.newVect = new Vector3(LeanTween.easeInOutQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutQuart(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutQuart(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInQuint:
										LeanTween.newVect = new Vector3(LeanTween.easeInQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInQuint(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInQuint(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeOutQuint:
										LeanTween.newVect = new Vector3(LeanTween.easeOutQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutQuint(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutQuint(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInOutQuint:
										LeanTween.newVect = new Vector3(LeanTween.easeInOutQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutQuint(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutQuint(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInSine:
										LeanTween.newVect = new Vector3(LeanTween.easeInSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInSine(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInSine(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeOutSine:
										LeanTween.newVect = new Vector3(LeanTween.easeOutSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutSine(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutSine(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInOutSine:
										LeanTween.newVect = new Vector3(LeanTween.easeInOutSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutSine(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutSine(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInExpo:
										LeanTween.newVect = new Vector3(LeanTween.easeInExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInExpo(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInExpo(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeOutExpo:
										LeanTween.newVect = new Vector3(LeanTween.easeOutExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutExpo(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutExpo(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInOutExpo:
										LeanTween.newVect = new Vector3(LeanTween.easeInOutExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutExpo(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutExpo(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInCirc:
										LeanTween.newVect = new Vector3(LeanTween.easeInCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInCirc(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInCirc(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeOutCirc:
										LeanTween.newVect = new Vector3(LeanTween.easeOutCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutCirc(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutCirc(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInOutCirc:
										LeanTween.newVect = new Vector3(LeanTween.easeInOutCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutCirc(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutCirc(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInBounce:
										LeanTween.newVect = new Vector3(LeanTween.easeInBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInBounce(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInBounce(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeOutBounce:
										LeanTween.newVect = new Vector3(LeanTween.easeOutBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutBounce(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutBounce(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInOutBounce:
										LeanTween.newVect = new Vector3(LeanTween.easeInOutBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutBounce(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutBounce(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeInBack:
										LeanTween.newVect = new Vector3(LeanTween.easeInBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, 1f), LeanTween.easeInBack(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, 1f), LeanTween.easeInBack(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, 1f));
										break;
									case LeanTweenType.easeOutBack:
										LeanTween.newVect = new Vector3(LeanTween.easeOutBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot), LeanTween.easeOutBack(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, LeanTween.tween.overshoot), LeanTween.easeOutBack(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, LeanTween.tween.overshoot));
										break;
									case LeanTweenType.easeInOutBack:
										LeanTween.newVect = new Vector3(LeanTween.easeInOutBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot), LeanTween.easeInOutBack(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, LeanTween.tween.overshoot), LeanTween.easeInOutBack(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, LeanTween.tween.overshoot));
										break;
									case LeanTweenType.easeInElastic:
										LeanTween.newVect = new Vector3(LeanTween.easeInElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeInElastic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeInElastic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period));
										break;
									case LeanTweenType.easeOutElastic:
										LeanTween.newVect = new Vector3(LeanTween.easeOutElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeOutElastic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeOutElastic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period));
										break;
									case LeanTweenType.easeInOutElastic:
										LeanTween.newVect = new Vector3(LeanTween.easeInOutElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeInOutElastic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeInOutElastic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period));
										break;
									case LeanTweenType.easeSpring:
										LeanTween.newVect = new Vector3(LeanTween.spring(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.spring(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.spring(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
										break;
									case LeanTweenType.easeShake:
									case LeanTweenType.punch:
										if (LeanTween.tween.tweenType == LeanTweenType.punch)
										{
											LeanTween.tween.animationCurve = LeanTween.punch;
										}
										else if (LeanTween.tween.tweenType == LeanTweenType.easeShake)
										{
											LeanTween.tween.animationCurve = LeanTween.shake;
										}
										LeanTween.tween.to = LeanTween.tween.from + LeanTween.tween.to;
										LeanTween.tween.diff = LeanTween.tween.to - LeanTween.tween.from;
										if (LeanTween.tweenAction == TweenAction.ROTATE || LeanTween.tweenAction == TweenAction.ROTATE_LOCAL)
										{
											LeanTween.tween.to = new Vector3(LeanTween.closestRot(LeanTween.tween.from.x, LeanTween.tween.to.x), LeanTween.closestRot(LeanTween.tween.from.y, LeanTween.tween.to.y), LeanTween.closestRot(LeanTween.tween.from.z, LeanTween.tween.to.z));
										}
										LeanTween.newVect = LeanTween.tweenOnCurveVector(LeanTween.tween, LeanTween.ratioPassed);
										break;
									}
								}
								else
								{
									LeanTween.newVect = new Vector3(LeanTween.tween.from.x + LeanTween.tween.diff.x * LeanTween.ratioPassed, LeanTween.tween.from.y + LeanTween.tween.diff.y * LeanTween.ratioPassed, LeanTween.tween.from.z + LeanTween.tween.diff.z * LeanTween.ratioPassed);
								}
								if (LeanTween.tweenAction == TweenAction.MOVE)
								{
									LeanTween.trans.position = LeanTween.newVect;
								}
								else if (LeanTween.tweenAction == TweenAction.MOVE_LOCAL)
								{
									LeanTween.trans.localPosition = LeanTween.newVect;
								}
								else if (LeanTween.tweenAction == TweenAction.ROTATE)
								{
									LeanTween.trans.eulerAngles = LeanTween.newVect;
								}
								else if (LeanTween.tweenAction == TweenAction.ROTATE_LOCAL)
								{
									LeanTween.trans.localEulerAngles = LeanTween.newVect;
								}
								else if (LeanTween.tweenAction == TweenAction.SCALE)
								{
									LeanTween.trans.localScale = LeanTween.newVect;
								}
								else if (LeanTween.tweenAction == TweenAction.GUI_MOVE)
								{
									LeanTween.tween.ltRect.rect = new Rect(LeanTween.newVect.x, LeanTween.newVect.y, LeanTween.tween.ltRect.rect.width, LeanTween.tween.ltRect.rect.height);
								}
								else if (LeanTween.tweenAction == TweenAction.GUI_MOVE_MARGIN)
								{
									LeanTween.tween.ltRect.margin = new Vector2(LeanTween.newVect.x, LeanTween.newVect.y);
								}
								else if (LeanTween.tweenAction == TweenAction.GUI_SCALE)
								{
									LeanTween.tween.ltRect.rect = new Rect(LeanTween.tween.ltRect.rect.x, LeanTween.tween.ltRect.rect.y, LeanTween.newVect.x, LeanTween.newVect.y);
								}
								else if (LeanTween.tweenAction == TweenAction.GUI_ALPHA)
								{
									LeanTween.tween.ltRect.alpha = LeanTween.newVect.x;
								}
								else if (LeanTween.tweenAction == TweenAction.GUI_ROTATE)
								{
									LeanTween.tween.ltRect.rotation = LeanTween.newVect.x;
								}
								else if (LeanTween.tweenAction == TweenAction.CANVAS_MOVE)
								{
									LeanTween.tween.rectTransform.anchoredPosition3D = LeanTween.newVect;
								}
								else if (LeanTween.tweenAction == TweenAction.CANVAS_SCALE)
								{
									LeanTween.tween.rectTransform.localScale = LeanTween.newVect;
								}
							}
							if (LeanTween.dt != 0f && LeanTween.tween.hasUpdateCallback)
							{
								if (LeanTween.tween.onUpdateFloat != null)
								{
									LeanTween.tween.onUpdateFloat(LeanTween.val);
								}
								if (LeanTween.tween.onUpdateFloatRatio != null)
								{
									LeanTween.tween.onUpdateFloatRatio(LeanTween.val, LeanTween.ratioPassed);
								}
								else if (LeanTween.tween.onUpdateFloatObject != null)
								{
									LeanTween.tween.onUpdateFloatObject(LeanTween.val, LeanTween.tween.onUpdateParam);
								}
								else if (LeanTween.tween.onUpdateVector3Object != null)
								{
									LeanTween.tween.onUpdateVector3Object(LeanTween.newVect, LeanTween.tween.onUpdateParam);
								}
								else if (LeanTween.tween.onUpdateVector3 != null)
								{
									LeanTween.tween.onUpdateVector3(LeanTween.newVect);
								}
								else if (LeanTween.tween.onUpdateVector2 != null)
								{
									LeanTween.tween.onUpdateVector2(new Vector2(LeanTween.newVect.x, LeanTween.newVect.y));
								}
							}
						}
						if (LeanTween.isTweenFinished)
						{
							if (LeanTween.tween.loopType == LeanTweenType.once || LeanTween.tween.loopCount == 1)
							{
								LeanTween.tweensFinished[LeanTween.finishedCnt] = num;
								LeanTween.finishedCnt++;
								if (LeanTween.tweenAction == TweenAction.GUI_ROTATE)
								{
									LeanTween.tween.ltRect.rotateFinished = true;
								}
								if (LeanTween.tweenAction == TweenAction.DELAYED_SOUND)
								{
									AudioSource.PlayClipAtPoint((AudioClip)LeanTween.tween.onCompleteParam, LeanTween.tween.to, LeanTween.tween.from.x);
								}
							}
							else
							{
								if ((LeanTween.tween.loopCount < 0 && LeanTween.tween.type == TweenAction.CALLBACK) || LeanTween.tween.onCompleteOnRepeat)
								{
									if (LeanTween.tweenAction == TweenAction.DELAYED_SOUND)
									{
										AudioSource.PlayClipAtPoint((AudioClip)LeanTween.tween.onCompleteParam, LeanTween.tween.to, LeanTween.tween.from.x);
									}
									if (LeanTween.tween.onComplete != null)
									{
										LeanTween.tween.onComplete();
									}
									else if (LeanTween.tween.onCompleteObject != null)
									{
										LeanTween.tween.onCompleteObject(LeanTween.tween.onCompleteParam);
									}
								}
								if (LeanTween.tween.loopCount >= 1)
								{
									LeanTween.tween.loopCount--;
								}
								if (LeanTween.tween.loopType == LeanTweenType.pingPong)
								{
									LeanTween.tween.direction = 0f - LeanTween.tween.direction;
								}
								else
								{
									LeanTween.tween.passed = Mathf.Epsilon;
								}
							}
						}
						else if (LeanTween.tween.delay <= 0f)
						{
							LeanTween.tween.passed += LeanTween.dt * LeanTween.tween.direction;
						}
						else
						{
							LeanTween.tween.delay -= LeanTween.dt;
							if (LeanTween.tween.delay < 0f)
							{
								LeanTween.tween.passed = 0f;
								LeanTween.tween.delay = 0f;
							}
						}
					}
				}
				num++;
			}
			LeanTween.tweenMaxSearch = LeanTween.maxTweenReached;
			LeanTween.frameRendered = Time.frameCount;
			for (int n = 0; n < LeanTween.finishedCnt; n++)
			{
				LeanTween.j = LeanTween.tweensFinished[n];
				LeanTween.tween = LeanTween.tweens[LeanTween.j];
				if (LeanTween.tween.onComplete != null)
				{
					Action onComplete = LeanTween.tween.onComplete;
					LeanTween.removeTween(LeanTween.j);
					onComplete();
				}
				else if (LeanTween.tween.onCompleteObject != null)
				{
					Action<object> onCompleteObject = LeanTween.tween.onCompleteObject;
					object onCompleteParam = LeanTween.tween.onCompleteParam;
					LeanTween.removeTween(LeanTween.j);
					onCompleteObject(onCompleteParam);
				}
				else
				{
					LeanTween.removeTween(LeanTween.j);
				}
			}
		}
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00008FD0 File Offset: 0x000071D0
	private static void textAlphaRecursive(Transform trans, float val)
	{
		Text component = trans.gameObject.GetComponent<Text>();
		if (component != null)
		{
			Color color = component.color;
			color.a = val;
			component.color = color;
		}
		if (trans.childCount > 0)
		{
			foreach (object obj in trans)
			{
				Transform transform = (Transform)obj;
				LeanTween.textAlphaRecursive(transform, val);
			}
		}
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00009078 File Offset: 0x00007278
	private static Color tweenColor(LTDescr tween, float val)
	{
		Vector3 vector = tween.point - tween.axis;
		float num = tween.to.y - tween.from.y;
		return new Color(tween.axis.x + vector.x * val, tween.axis.y + vector.y * val, tween.axis.z + vector.z * val, tween.from.y + num * val);
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00009104 File Offset: 0x00007304
	public static void removeTween(int i, int uniqueId)
	{
		if (LeanTween.tweens[i].uniqueId == uniqueId)
		{
			LeanTween.removeTween(i);
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00009120 File Offset: 0x00007320
	public static void removeTween(int i)
	{
		if (LeanTween.tweens[i].toggle)
		{
			LeanTween.tweens[i].toggle = false;
			if (LeanTween.tweens[i].destroyOnComplete)
			{
				if (LeanTween.tweens[i].ltRect != null)
				{
					LTGUI.destroy(LeanTween.tweens[i].ltRect.id);
				}
				else if (LeanTween.tweens[i].trans.gameObject != LeanTween._tweenEmpty)
				{
					global::UnityEngine.Object.Destroy(LeanTween.tweens[i].trans.gameObject);
				}
			}
			LeanTween.tweens[i].cleanup();
			LeanTween.startSearch = i;
			if (i + 1 >= LeanTween.tweenMaxSearch)
			{
				LeanTween.startSearch = 0;
			}
		}
	}

	// Token: 0x060000FE RID: 254 RVA: 0x000091E4 File Offset: 0x000073E4
	public static Vector3[] add(Vector3[] a, Vector3 b)
	{
		Vector3[] array = new Vector3[a.Length];
		LeanTween.i = 0;
		while (LeanTween.i < a.Length)
		{
			array[LeanTween.i] = a[LeanTween.i] + b;
			LeanTween.i++;
		}
		return array;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00009248 File Offset: 0x00007448
	public static float closestRot(float from, float to)
	{
		float num = 0f - (360f - to);
		float num2 = 360f + to;
		float num3 = Mathf.Abs(to - from);
		float num4 = Mathf.Abs(num - from);
		float num5 = Mathf.Abs(num2 - from);
		if (num3 < num4 && num3 < num5)
		{
			return to;
		}
		if (num4 < num5)
		{
			return num;
		}
		return num2;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x000092A4 File Offset: 0x000074A4
	public static void cancelAll()
	{
		LeanTween.cancelAll(false);
	}

	// Token: 0x06000101 RID: 257 RVA: 0x000092AC File Offset: 0x000074AC
	public static void cancelAll(bool callComplete)
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans != null)
			{
				if (callComplete && LeanTween.tweens[i].onComplete != null)
				{
					LeanTween.tweens[i].onComplete();
				}
				LeanTween.removeTween(i);
			}
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x0000931C File Offset: 0x0000751C
	public static void cancel(GameObject gameObject)
	{
		LeanTween.cancel(gameObject, false);
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00009328 File Offset: 0x00007528
	public static void cancel(GameObject gameObject, bool callOnComplete)
	{
		LeanTween.init();
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].toggle && LeanTween.tweens[i].trans == transform)
			{
				if (callOnComplete && LeanTween.tweens[i].onComplete != null)
				{
					LeanTween.tweens[i].onComplete();
				}
				LeanTween.removeTween(i);
			}
		}
	}

	// Token: 0x06000104 RID: 260 RVA: 0x000093B0 File Offset: 0x000075B0
	public static void cancel(GameObject gameObject, int uniqueId)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (LeanTween.tweens[num].trans == null || (LeanTween.tweens[num].trans.gameObject == gameObject && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2)))
			{
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00009424 File Offset: 0x00007624
	public static void cancel(LTRect ltRect, int uniqueId)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (LeanTween.tweens[num].ltRect == ltRect && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
			{
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00009478 File Offset: 0x00007678
	public static void cancel(int uniqueId)
	{
		LeanTween.cancel(uniqueId, false);
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00009484 File Offset: 0x00007684
	public static void cancel(int uniqueId, bool callOnComplete)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
			{
				if (callOnComplete && LeanTween.tweens[num].onComplete != null)
				{
					LeanTween.tweens[num].onComplete();
				}
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x06000108 RID: 264 RVA: 0x000094EC File Offset: 0x000076EC
	public static LTDescr descr(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if (LeanTween.tweens[num] != null && LeanTween.tweens[num].uniqueId == uniqueId && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			return LeanTween.tweens[num];
		}
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].uniqueId == uniqueId && (ulong)LeanTween.tweens[i].counter == (ulong)((long)num2))
			{
				return LeanTween.tweens[i];
			}
		}
		return null;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00009588 File Offset: 0x00007788
	public static LTDescr description(int uniqueId)
	{
		return LeanTween.descr(uniqueId);
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00009590 File Offset: 0x00007790
	public static LTDescr[] descriptions(GameObject gameObject = null)
	{
		if (gameObject == null)
		{
			return null;
		}
		List<LTDescr> list = new List<LTDescr>();
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].toggle && LeanTween.tweens[i].trans == transform)
			{
				list.Add(LeanTween.tweens[i]);
			}
		}
		return list.ToArray();
	}

	// Token: 0x0600010B RID: 267 RVA: 0x0000960C File Offset: 0x0000780C
	[Obsolete("Use 'pause( id )' instead")]
	public static void pause(GameObject gameObject, int uniqueId)
	{
		LeanTween.pause(uniqueId);
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00009614 File Offset: 0x00007814
	public static void pause(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			LeanTween.tweens[num].pause();
		}
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00009650 File Offset: 0x00007850
	public static void pause(GameObject gameObject)
	{
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans == transform)
			{
				LeanTween.tweens[i].pause();
			}
		}
	}

	// Token: 0x0600010E RID: 270 RVA: 0x000096A0 File Offset: 0x000078A0
	public static void pauseAll()
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LeanTween.tweens[i].pause();
		}
	}

	// Token: 0x0600010F RID: 271 RVA: 0x000096D8 File Offset: 0x000078D8
	public static void resumeAll()
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LeanTween.tweens[i].resume();
		}
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00009710 File Offset: 0x00007910
	[Obsolete("Use 'resume( id )' instead")]
	public static void resume(GameObject gameObject, int uniqueId)
	{
		LeanTween.resume(uniqueId);
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00009718 File Offset: 0x00007918
	public static void resume(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			LeanTween.tweens[num].resume();
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00009754 File Offset: 0x00007954
	public static void resume(GameObject gameObject)
	{
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans == transform)
			{
				LeanTween.tweens[i].resume();
			}
		}
	}

	// Token: 0x06000113 RID: 275 RVA: 0x000097A4 File Offset: 0x000079A4
	public static bool isTweening(GameObject gameObject = null)
	{
		if (gameObject == null)
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].toggle)
				{
					return true;
				}
			}
			return false;
		}
		Transform transform = gameObject.transform;
		for (int j = 0; j <= LeanTween.tweenMaxSearch; j++)
		{
			if (LeanTween.tweens[j].toggle && LeanTween.tweens[j].trans == transform)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00009830 File Offset: 0x00007A30
	public static bool isTweening(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		return num >= 0 && num < LeanTween.maxTweens && ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2) && LeanTween.tweens[num].toggle);
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00009888 File Offset: 0x00007A88
	public static bool isTweening(LTRect ltRect)
	{
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].toggle && LeanTween.tweens[i].ltRect == ltRect)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000116 RID: 278 RVA: 0x000098D4 File Offset: 0x00007AD4
	public static void drawBezierPath(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float arrowSize = 0f, Transform arrowTransform = null)
	{
		Vector3 vector = a;
		Vector3 vector2 = -a + 3f * (b - c) + d;
		Vector3 vector3 = 3f * (a + c) - 6f * b;
		Vector3 vector4 = 3f * (b - a);
		if (arrowSize > 0f)
		{
			Vector3 position = arrowTransform.position;
			Quaternion rotation = arrowTransform.rotation;
			float num = 0f;
			for (float num2 = 1f; num2 <= 120f; num2 += 1f)
			{
				float num3 = num2 / 120f;
				Vector3 vector5 = ((vector2 * num3 + vector3) * num3 + vector4) * num3 + a;
				Gizmos.DrawLine(vector, vector5);
				num += (vector5 - vector).magnitude;
				if (num > 1f)
				{
					num -= 1f;
					arrowTransform.position = vector5;
					arrowTransform.LookAt(vector, Vector3.forward);
					Vector3 vector6 = arrowTransform.TransformDirection(Vector3.right);
					Vector3 normalized = (vector - vector5).normalized;
					Gizmos.DrawLine(vector5, vector5 + (vector6 + normalized) * arrowSize);
					vector6 = arrowTransform.TransformDirection(-Vector3.right);
					Gizmos.DrawLine(vector5, vector5 + (vector6 + normalized) * arrowSize);
				}
				vector = vector5;
			}
			arrowTransform.position = position;
			arrowTransform.rotation = rotation;
		}
		else
		{
			for (float num4 = 1f; num4 <= 30f; num4 += 1f)
			{
				float num3 = num4 / 30f;
				Vector3 vector5 = ((vector2 * num3 + vector3) * num3 + vector4) * num3 + a;
				Gizmos.DrawLine(vector, vector5);
				vector = vector5;
			}
		}
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00009AE0 File Offset: 0x00007CE0
	public static object logError(string error)
	{
		if (LeanTween.throwErrors)
		{
			Debug.LogError(error);
		}
		else
		{
			Debug.Log(error);
		}
		return null;
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00009B00 File Offset: 0x00007D00
	public static LTDescr options(LTDescr seed)
	{
		Debug.LogError("error this function is no longer used");
		return null;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00009B10 File Offset: 0x00007D10
	public static LTDescr options()
	{
		LeanTween.init();
		bool flag = false;
		LeanTween.j = 0;
		LeanTween.i = LeanTween.startSearch;
		while (LeanTween.j < LeanTween.maxTweens)
		{
			if (LeanTween.i >= LeanTween.maxTweens - 1)
			{
				LeanTween.i = 0;
			}
			if (!LeanTween.tweens[LeanTween.i].toggle)
			{
				if (LeanTween.i + 1 > LeanTween.tweenMaxSearch)
				{
					LeanTween.tweenMaxSearch = LeanTween.i + 1;
				}
				LeanTween.startSearch = LeanTween.i + 1;
				flag = true;
				break;
			}
			LeanTween.j++;
			if (LeanTween.j >= LeanTween.maxTweens)
			{
				return LeanTween.logError("LeanTween - You have run out of available spaces for tweening. To avoid this error increase the number of spaces to available for tweening when you initialize the LeanTween class ex: LeanTween.init( " + LeanTween.maxTweens * 2 + " );") as LTDescr;
			}
			LeanTween.i++;
		}
		if (!flag)
		{
			LeanTween.logError("no available tween found!");
		}
		LeanTween.tweens[LeanTween.i].reset();
		LeanTween.tweens[LeanTween.i].setId((uint)LeanTween.i);
		return LeanTween.tweens[LeanTween.i];
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x0600011A RID: 282 RVA: 0x00009C34 File Offset: 0x00007E34
	public static GameObject tweenEmpty
	{
		get
		{
			LeanTween.init(LeanTween.maxTweens);
			return LeanTween._tweenEmpty;
		}
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00009C48 File Offset: 0x00007E48
	private static LTDescr pushNewTween(GameObject gameObject, Vector3 to, float time, TweenAction tweenAction, LTDescr tween)
	{
		LeanTween.init(LeanTween.maxTweens);
		if (gameObject == null || tween == null)
		{
			return null;
		}
		tween.trans = gameObject.transform;
		tween.to = to;
		tween.time = time;
		tween.type = tweenAction;
		return tween;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00009C9C File Offset: 0x00007E9C
	public static LTDescr play(RectTransform rectTransform, Sprite[] sprites)
	{
		float num = 0.25f;
		float num2 = num * (float)sprites.Length;
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3((float)sprites.Length - 1f, 0f, 0f), num2, TweenAction.CANVAS_PLAYSPRITE, LeanTween.options().setSprites(sprites).setRepeat(-1));
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00009CF0 File Offset: 0x00007EF0
	public static LTDescr alpha(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ALPHA, LeanTween.options());
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00009D10 File Offset: 0x00007F10
	public static LTDescr alpha(LTRect ltRect, float to, float time)
	{
		ltRect.alphaEnabled = true;
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, TweenAction.GUI_ALPHA, LeanTween.options().setRect(ltRect));
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00009D4C File Offset: 0x00007F4C
	public static LTDescr textAlpha(RectTransform rectTransform, float to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.TEXT_ALPHA, LeanTween.options());
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00009D7C File Offset: 0x00007F7C
	public static LTDescr alphaVertex(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ALPHA_VERTEX, LeanTween.options());
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00009D9C File Offset: 0x00007F9C
	public static LTDescr color(GameObject gameObject, Color to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.COLOR, LeanTween.options().setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00009DEC File Offset: 0x00007FEC
	public static LTDescr textColor(RectTransform rectTransform, Color to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.TEXT_COLOR, LeanTween.options().setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00009E44 File Offset: 0x00008044
	public static LTDescr delayedCall(float delayTime, Action callback)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, TweenAction.CALLBACK, LeanTween.options().setOnComplete(callback));
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00009E70 File Offset: 0x00008070
	public static LTDescr delayedCall(float delayTime, Action<object> callback)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, TweenAction.CALLBACK, LeanTween.options().setOnComplete(callback));
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00009E9C File Offset: 0x0000809C
	public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action callback)
	{
		return LeanTween.pushNewTween(gameObject, Vector3.zero, delayTime, TweenAction.CALLBACK, LeanTween.options().setOnComplete(callback));
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00009EC4 File Offset: 0x000080C4
	public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action<object> callback)
	{
		return LeanTween.pushNewTween(gameObject, Vector3.zero, delayTime, TweenAction.CALLBACK, LeanTween.options().setOnComplete(callback));
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00009EEC File Offset: 0x000080EC
	public static LTDescr destroyAfter(LTRect rect, float delayTime)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, TweenAction.CALLBACK, LeanTween.options().setRect(rect).setDestroyOnComplete(true));
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00009F1C File Offset: 0x0000811C
	public static LTDescr move(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, TweenAction.MOVE, LeanTween.options());
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00009F30 File Offset: 0x00008130
	public static LTDescr move(GameObject gameObject, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, gameObject.transform.position.z), time, TweenAction.MOVE, LeanTween.options());
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00009F74 File Offset: 0x00008174
	public static LTDescr move(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options();
		if (LeanTween.d.path == null)
		{
			LeanTween.d.path = new LTBezierPath(to);
		}
		else
		{
			LeanTween.d.path.setPoints(to);
		}
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED, LeanTween.d);
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00009FE0 File Offset: 0x000081E0
	public static LTDescr move(GameObject gameObject, LTBezierPath to, float time)
	{
		LeanTween.d = LeanTween.options();
		LeanTween.d.path = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED, LeanTween.d);
	}

	// Token: 0x0600012C RID: 300 RVA: 0x0000A024 File Offset: 0x00008224
	public static LTDescr move(GameObject gameObject, LTSpline to, float time)
	{
		LeanTween.d = LeanTween.options();
		LeanTween.d.spline = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE, LeanTween.d);
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0000A068 File Offset: 0x00008268
	public static LTDescr moveSpline(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options();
		LeanTween.d.spline = new LTSpline(to);
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE, LeanTween.d);
	}

	// Token: 0x0600012E RID: 302 RVA: 0x0000A0A8 File Offset: 0x000082A8
	public static LTDescr moveSplineLocal(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options();
		LeanTween.d.spline = new LTSpline(to);
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE_LOCAL, LeanTween.d);
	}

	// Token: 0x0600012F RID: 303 RVA: 0x0000A0F4 File Offset: 0x000082F4
	public static LTDescr move(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, TweenAction.GUI_MOVE, LeanTween.options().setRect(ltRect));
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000A120 File Offset: 0x00008320
	public static LTDescr moveMargin(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, TweenAction.GUI_MOVE_MARGIN, LeanTween.options().setRect(ltRect));
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000A14C File Offset: 0x0000834C
	public static LTDescr moveX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_X, LeanTween.options());
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000A16C File Offset: 0x0000836C
	public static LTDescr moveY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_Y, LeanTween.options());
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0000A18C File Offset: 0x0000838C
	public static LTDescr moveZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_Z, LeanTween.options());
	}

	// Token: 0x06000134 RID: 308 RVA: 0x0000A1AC File Offset: 0x000083AC
	public static LTDescr moveLocal(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, TweenAction.MOVE_LOCAL, LeanTween.options());
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000A1C0 File Offset: 0x000083C0
	public static LTDescr moveLocal(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options();
		if (LeanTween.d.path == null)
		{
			LeanTween.d.path = new LTBezierPath(to);
		}
		else
		{
			LeanTween.d.path.setPoints(to);
		}
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED_LOCAL, LeanTween.d);
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000A22C File Offset: 0x0000842C
	public static LTDescr moveLocalX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_LOCAL_X, LeanTween.options());
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000A24C File Offset: 0x0000844C
	public static LTDescr moveLocalY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_LOCAL_Y, LeanTween.options());
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000A26C File Offset: 0x0000846C
	public static LTDescr moveLocalZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_LOCAL_Z, LeanTween.options());
	}

	// Token: 0x06000139 RID: 313 RVA: 0x0000A28C File Offset: 0x0000848C
	public static LTDescr moveLocal(GameObject gameObject, LTBezierPath to, float time)
	{
		LeanTween.d = LeanTween.options();
		LeanTween.d.path = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED_LOCAL, LeanTween.d);
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000A2D0 File Offset: 0x000084D0
	public static LTDescr moveLocal(GameObject gameObject, LTSpline to, float time)
	{
		LeanTween.d = LeanTween.options();
		LeanTween.d.spline = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE_LOCAL, LeanTween.d);
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000A30C File Offset: 0x0000850C
	public static LTDescr rotate(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, TweenAction.ROTATE, LeanTween.options());
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0000A320 File Offset: 0x00008520
	public static LTDescr rotate(LTRect ltRect, float to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, TweenAction.GUI_ROTATE, LeanTween.options().setRect(ltRect));
	}

	// Token: 0x0600013D RID: 317 RVA: 0x0000A358 File Offset: 0x00008558
	public static LTDescr rotateLocal(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, TweenAction.ROTATE_LOCAL, LeanTween.options());
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000A36C File Offset: 0x0000856C
	public static LTDescr rotateX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ROTATE_X, LeanTween.options());
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0000A38C File Offset: 0x0000858C
	public static LTDescr rotateY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ROTATE_Y, LeanTween.options());
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0000A3AC File Offset: 0x000085AC
	public static LTDescr rotateZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ROTATE_Z, LeanTween.options());
	}

	// Token: 0x06000141 RID: 321 RVA: 0x0000A3CC File Offset: 0x000085CC
	public static LTDescr rotateAround(GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, TweenAction.ROTATE_AROUND, LeanTween.options().setAxis(axis));
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000A400 File Offset: 0x00008600
	public static LTDescr rotateAroundLocal(GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, TweenAction.ROTATE_AROUND_LOCAL, LeanTween.options().setAxis(axis));
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000A434 File Offset: 0x00008634
	public static LTDescr scale(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, TweenAction.SCALE, LeanTween.options());
	}

	// Token: 0x06000144 RID: 324 RVA: 0x0000A448 File Offset: 0x00008648
	public static LTDescr scale(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, TweenAction.GUI_SCALE, LeanTween.options().setRect(ltRect));
	}

	// Token: 0x06000145 RID: 325 RVA: 0x0000A474 File Offset: 0x00008674
	public static LTDescr scaleX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.SCALE_X, LeanTween.options());
	}

	// Token: 0x06000146 RID: 326 RVA: 0x0000A494 File Offset: 0x00008694
	public static LTDescr scaleY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.SCALE_Y, LeanTween.options());
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000A4B4 File Offset: 0x000086B4
	public static LTDescr scaleZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.SCALE_Z, LeanTween.options());
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000A4D4 File Offset: 0x000086D4
	public static LTDescr value(GameObject gameObject, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CALLBACK, LeanTween.options().setFrom(new Vector3(from, 0f, 0f)));
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000A514 File Offset: 0x00008714
	public static LTDescr value(GameObject gameObject, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, TweenAction.VALUE3, LeanTween.options().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f)));
	}

	// Token: 0x0600014A RID: 330 RVA: 0x0000A584 File Offset: 0x00008784
	public static LTDescr value(GameObject gameObject, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, TweenAction.VALUE3, LeanTween.options().setFrom(from));
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0000A5A8 File Offset: 0x000087A8
	public static LTDescr value(GameObject gameObject, Color from, Color to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.CALLBACK_COLOR, LeanTween.options().setPoint(new Vector3(to.r, to.g, to.b)).setFromColor(from)
			.setHasInitialized(false));
	}

	// Token: 0x0600014C RID: 332 RVA: 0x0000A604 File Offset: 0x00008804
	public static LTDescr value(GameObject gameObject, Action<float> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CALLBACK, LeanTween.options().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f))
			.setOnUpdate(callOnUpdate));
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000A660 File Offset: 0x00008860
	public static LTDescr value(GameObject gameObject, Action<float, float> callOnUpdateRatio, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CALLBACK, LeanTween.options().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f))
			.setOnUpdateRatio(callOnUpdateRatio));
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0000A6BC File Offset: 0x000088BC
	public static LTDescr value(GameObject gameObject, Action<Color> callOnUpdate, Color from, Color to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.CALLBACK_COLOR, LeanTween.options().setPoint(new Vector3(to.r, to.g, to.b)).setAxis(new Vector3(from.r, from.g, from.b))
			.setFrom(new Vector3(0f, from.a, 0f))
			.setHasInitialized(false)
			.setOnUpdateColor(callOnUpdate));
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000A754 File Offset: 0x00008954
	public static LTDescr value(GameObject gameObject, Action<Vector2> callOnUpdate, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, TweenAction.VALUE3, LeanTween.options().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f))
			.setOnUpdateVector2(callOnUpdate));
	}

	// Token: 0x06000150 RID: 336 RVA: 0x0000A7C8 File Offset: 0x000089C8
	public static LTDescr value(GameObject gameObject, Action<Vector3> callOnUpdate, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, TweenAction.VALUE3, LeanTween.options().setTo(to).setFrom(from)
			.setOnUpdateVector3(callOnUpdate));
	}

	// Token: 0x06000151 RID: 337 RVA: 0x0000A7F8 File Offset: 0x000089F8
	public static LTDescr value(GameObject gameObject, Action<float, object> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CALLBACK, LeanTween.options().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f))
			.setOnUpdateObject(callOnUpdate));
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0000A854 File Offset: 0x00008A54
	public static LTDescr delayedSound(AudioClip audio, Vector3 pos, float volume)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, pos, 0f, TweenAction.DELAYED_SOUND, LeanTween.options().setTo(pos).setFrom(new Vector3(volume, 0f, 0f))
			.setAudio(audio));
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000A89C File Offset: 0x00008A9C
	public static LTDescr delayedSound(GameObject gameObject, AudioClip audio, Vector3 pos, float volume)
	{
		return LeanTween.pushNewTween(gameObject, pos, 0f, TweenAction.DELAYED_SOUND, LeanTween.options().setTo(pos).setFrom(new Vector3(volume, 0f, 0f))
			.setAudio(audio));
	}

	// Token: 0x06000154 RID: 340 RVA: 0x0000A8E0 File Offset: 0x00008AE0
	public static LTDescr move(RectTransform rectTrans, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, TweenAction.CANVAS_MOVE, LeanTween.options().setRect(rectTrans));
	}

	// Token: 0x06000155 RID: 341 RVA: 0x0000A908 File Offset: 0x00008B08
	public static LTDescr moveX(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_MOVE_X, LeanTween.options().setRect(rectTrans));
	}

	// Token: 0x06000156 RID: 342 RVA: 0x0000A940 File Offset: 0x00008B40
	public static LTDescr moveY(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_MOVE_Y, LeanTween.options().setRect(rectTrans));
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000A978 File Offset: 0x00008B78
	public static LTDescr moveZ(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_MOVE_Z, LeanTween.options().setRect(rectTrans));
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0000A9B0 File Offset: 0x00008BB0
	public static LTDescr rotate(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ROTATEAROUND, LeanTween.options().setRect(rectTrans).setAxis(Vector3.forward));
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000A9F0 File Offset: 0x00008BF0
	public static LTDescr rotateAround(RectTransform rectTrans, Vector3 axis, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ROTATEAROUND, LeanTween.options().setRect(rectTrans).setAxis(axis));
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000AA2C File Offset: 0x00008C2C
	public static LTDescr rotateAroundLocal(RectTransform rectTrans, Vector3 axis, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ROTATEAROUND_LOCAL, LeanTween.options().setRect(rectTrans).setAxis(axis));
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000AA68 File Offset: 0x00008C68
	public static LTDescr scale(RectTransform rectTrans, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, TweenAction.CANVAS_SCALE, LeanTween.options().setRect(rectTrans));
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000AA90 File Offset: 0x00008C90
	public static LTDescr alpha(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ALPHA, LeanTween.options().setRect(rectTrans));
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000AAC8 File Offset: 0x00008CC8
	public static LTDescr color(RectTransform rectTrans, Color to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.CANVAS_COLOR, LeanTween.options().setRect(rectTrans).setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000AB24 File Offset: 0x00008D24
	private static float tweenOnCurve(LTDescr tweenDescr, float ratioPassed)
	{
		return tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.animationCurve.Evaluate(ratioPassed);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000AB58 File Offset: 0x00008D58
	private static Vector3 tweenOnCurveVector(LTDescr tweenDescr, float ratioPassed)
	{
		return new Vector3(tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.animationCurve.Evaluate(ratioPassed), tweenDescr.from.y + tweenDescr.diff.y * tweenDescr.animationCurve.Evaluate(ratioPassed), tweenDescr.from.z + tweenDescr.diff.z * tweenDescr.animationCurve.Evaluate(ratioPassed));
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000ABD8 File Offset: 0x00008DD8
	private static float easeOutQuadOpt(float start, float diff, float ratioPassed)
	{
		return -diff * ratioPassed * (ratioPassed - 2f) + start;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000ABE8 File Offset: 0x00008DE8
	private static float easeInQuadOpt(float start, float diff, float ratioPassed)
	{
		return diff * ratioPassed * ratioPassed + start;
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000ABF4 File Offset: 0x00008DF4
	private static float easeInOutQuadOpt(float start, float diff, float ratioPassed)
	{
		ratioPassed /= 0.5f;
		if (ratioPassed < 1f)
		{
			return diff / 2f * ratioPassed * ratioPassed + start;
		}
		ratioPassed -= 1f;
		return -diff / 2f * (ratioPassed * (ratioPassed - 2f) - 1f) + start;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x0000AC48 File Offset: 0x00008E48
	private static float linear(float start, float end, float val)
	{
		return Mathf.Lerp(start, end, val);
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0000AC54 File Offset: 0x00008E54
	private static float clerp(float start, float end, float val)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float num5;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * val;
			num5 = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * val;
			num5 = start + num4;
		}
		else
		{
			num5 = start + (end - start) * val;
		}
		return num5;
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000ACCC File Offset: 0x00008ECC
	private static float spring(float start, float end, float val)
	{
		val = Mathf.Clamp01(val);
		val = (Mathf.Sin(val * 3.1415927f * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + 1.2f * (1f - val));
		return start + (end - start) * val;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000AD30 File Offset: 0x00008F30
	private static float easeInQuad(float start, float end, float val)
	{
		end -= start;
		return end * val * val + start;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000AD40 File Offset: 0x00008F40
	private static float easeOutQuad(float start, float end, float val)
	{
		end -= start;
		return -end * val * (val - 2f) + start;
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000AD58 File Offset: 0x00008F58
	private static float easeInOutQuad(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val + start;
		}
		val -= 1f;
		return -end / 2f * (val * (val - 2f) - 1f) + start;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000ADB0 File Offset: 0x00008FB0
	private static float easeInCubic(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val + start;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000ADC0 File Offset: 0x00008FC0
	private static float easeOutCubic(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * (val * val * val + 1f) + start;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0000ADE0 File Offset: 0x00008FE0
	private static float easeInOutCubic(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val + start;
		}
		val -= 2f;
		return end / 2f * (val * val * val + 2f) + start;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000AE34 File Offset: 0x00009034
	private static float easeInQuart(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val * val + start;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000AE48 File Offset: 0x00009048
	private static float easeOutQuart(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return -end * (val * val * val * val - 1f) + start;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000AE78 File Offset: 0x00009078
	private static float easeInOutQuart(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val * val + start;
		}
		val -= 2f;
		return -end / 2f * (val * val * val * val - 2f) + start;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000AED4 File Offset: 0x000090D4
	private static float easeInQuint(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val * val * val + start;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000AEE8 File Offset: 0x000090E8
	private static float easeOutQuint(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * (val * val * val * val * val + 1f) + start;
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000AF0C File Offset: 0x0000910C
	private static float easeInOutQuint(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val * val * val + start;
		}
		val -= 2f;
		return end / 2f * (val * val * val * val * val + 2f) + start;
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000AF68 File Offset: 0x00009168
	private static float easeInSine(float start, float end, float val)
	{
		end -= start;
		return -end * Mathf.Cos(val / 1f * 1.5707964f) + end + start;
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000AF88 File Offset: 0x00009188
	private static float easeOutSine(float start, float end, float val)
	{
		end -= start;
		return end * Mathf.Sin(val / 1f * 1.5707964f) + start;
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000AFA8 File Offset: 0x000091A8
	private static float easeInOutSine(float start, float end, float val)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.1415927f * val / 1f) - 1f) + start;
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000AFE0 File Offset: 0x000091E0
	private static float easeInExpo(float start, float end, float val)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (val / 1f - 1f)) + start;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0000B014 File Offset: 0x00009214
	private static float easeOutExpo(float start, float end, float val)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * val / 1f) + 1f) + start;
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0000B040 File Offset: 0x00009240
	private static float easeInOutExpo(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (val - 1f)) + start;
		}
		val -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * val) + 2f) + start;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000B0B4 File Offset: 0x000092B4
	private static float easeInCirc(float start, float end, float val)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - val * val) - 1f) + start;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000B0D4 File Offset: 0x000092D4
	private static float easeOutCirc(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - val * val) + start;
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000B104 File Offset: 0x00009304
	private static float easeInOutCirc(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - val * val) - 1f) + start;
		}
		val -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - val * val) + 1f) + start;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000B174 File Offset: 0x00009374
	private static float easeInBounce(float start, float end, float val)
	{
		end -= start;
		float num = 1f;
		return end - LeanTween.easeOutBounce(0f, end, num - val) + start;
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000B1A0 File Offset: 0x000093A0
	private static float easeOutBounce(float start, float end, float val)
	{
		val /= 1f;
		end -= start;
		if (val < 0.36363637f)
		{
			return end * (7.5625f * val * val) + start;
		}
		if (val < 0.72727275f)
		{
			val -= 0.54545456f;
			return end * (7.5625f * val * val + 0.75f) + start;
		}
		if ((double)val < 0.9090909090909091)
		{
			val -= 0.8181818f;
			return end * (7.5625f * val * val + 0.9375f) + start;
		}
		val -= 0.95454544f;
		return end * (7.5625f * val * val + 0.984375f) + start;
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000B248 File Offset: 0x00009448
	private static float easeInOutBounce(float start, float end, float val)
	{
		end -= start;
		float num = 1f;
		if (val < num / 2f)
		{
			return LeanTween.easeInBounce(0f, end, val * 2f) * 0.5f + start;
		}
		return LeanTween.easeOutBounce(0f, end, val * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000B2AC File Offset: 0x000094AC
	private static float easeInBack(float start, float end, float val, float overshoot = 1f)
	{
		end -= start;
		val /= 1f;
		float num = 1.70158f * overshoot;
		return end * val * val * ((num + 1f) * val - num) + start;
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000B2E4 File Offset: 0x000094E4
	private static float easeOutBack(float start, float end, float val, float overshoot = 1f)
	{
		float num = 1.70158f * overshoot;
		end -= start;
		val = val / 1f - 1f;
		return end * (val * val * ((num + 1f) * val + num) + 1f) + start;
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000B328 File Offset: 0x00009528
	private static float easeInOutBack(float start, float end, float val, float overshoot = 1f)
	{
		float num = 1.70158f * overshoot;
		end -= start;
		val /= 0.5f;
		if (val < 1f)
		{
			num *= 1.525f * overshoot;
			return end / 2f * (val * val * ((num + 1f) * val - num)) + start;
		}
		val -= 2f;
		num *= 1.525f * overshoot;
		return end / 2f * (val * val * ((num + 1f) * val + num) + 2f) + start;
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000B3AC File Offset: 0x000095AC
	private static float easeInElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		if (val == 1f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f && val > 0.6f)
		{
			overshoot = 1f + (1f - val) / 0.4f * (overshoot - 1f);
		}
		val -= 1f;
		return start - num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * overshoot;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000B484 File Offset: 0x00009684
	private static float easeOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		if (val == 1f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f && val < 0.4f)
		{
			overshoot = 1f + val / 0.4f * (overshoot - 1f);
		}
		return start + end + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * overshoot;
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000B550 File Offset: 0x00009750
	private static float easeInOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		val /= 0.5f;
		if (val == 2f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f)
		{
			if (val < 0.2f)
			{
				overshoot = 1f + val / 0.2f * (overshoot - 1f);
			}
			else if (val > 0.8f)
			{
				overshoot = 1f + (1f - val) / 0.2f * (overshoot - 1f);
			}
		}
		if (val < 1f)
		{
			val -= 1f;
			return start - 0.5f * (num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period)) * overshoot;
		}
		val -= 1f;
		return end + start + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * 0.5f * overshoot;
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0000B6A4 File Offset: 0x000098A4
	public static void addListener(int eventId, Action<LTEvent> callback)
	{
		LeanTween.addListener(LeanTween.tweenEmpty, eventId, callback);
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000B6B4 File Offset: 0x000098B4
	public static void addListener(GameObject caller, int eventId, Action<LTEvent> callback)
	{
		if (LeanTween.eventListeners == null)
		{
			LeanTween.INIT_LISTENERS_MAX = LeanTween.LISTENERS_MAX;
			LeanTween.eventListeners = new Action<LTEvent>[LeanTween.EVENTS_MAX * LeanTween.LISTENERS_MAX];
			LeanTween.goListeners = new GameObject[LeanTween.EVENTS_MAX * LeanTween.LISTENERS_MAX];
		}
		LeanTween.i = 0;
		while (LeanTween.i < LeanTween.INIT_LISTENERS_MAX)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
			if (LeanTween.goListeners[num] == null || LeanTween.eventListeners[num] == null)
			{
				LeanTween.eventListeners[num] = callback;
				LeanTween.goListeners[num] = caller;
				if (LeanTween.i >= LeanTween.eventsMaxSearch)
				{
					LeanTween.eventsMaxSearch = LeanTween.i + 1;
				}
				return;
			}
			if (LeanTween.goListeners[num] == caller && object.Equals(LeanTween.eventListeners[num], callback))
			{
				return;
			}
			LeanTween.i++;
		}
		Debug.LogError("You ran out of areas to add listeners, consider increasing INIT_LISTENERS_MAX, ex: LeanTween.INIT_LISTENERS_MAX = " + LeanTween.INIT_LISTENERS_MAX * 2);
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000B7C0 File Offset: 0x000099C0
	public static bool removeListener(int eventId, Action<LTEvent> callback)
	{
		return LeanTween.removeListener(LeanTween.tweenEmpty, eventId, callback);
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000B7D0 File Offset: 0x000099D0
	public static bool removeListener(GameObject caller, int eventId, Action<LTEvent> callback)
	{
		LeanTween.i = 0;
		while (LeanTween.i < LeanTween.eventsMaxSearch)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
			if (LeanTween.goListeners[num] == caller && object.Equals(LeanTween.eventListeners[num], callback))
			{
				LeanTween.eventListeners[num] = null;
				LeanTween.goListeners[num] = null;
				return true;
			}
			LeanTween.i++;
		}
		return false;
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000B848 File Offset: 0x00009A48
	public static void dispatchEvent(int eventId)
	{
		LeanTween.dispatchEvent(eventId, null);
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000B854 File Offset: 0x00009A54
	public static void dispatchEvent(int eventId, object data)
	{
		for (int i = 0; i < LeanTween.eventsMaxSearch; i++)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + i;
			if (LeanTween.eventListeners[num] != null)
			{
				if (LeanTween.goListeners[num])
				{
					LeanTween.eventListeners[num](new LTEvent(eventId, data));
				}
				else
				{
					LeanTween.eventListeners[num] = null;
				}
			}
		}
	}

	// Token: 0x040000C6 RID: 198
	public static bool throwErrors = true;

	// Token: 0x040000C7 RID: 199
	public static float tau = 6.2831855f;

	// Token: 0x040000C8 RID: 200
	private static LTDescr[] tweens;

	// Token: 0x040000C9 RID: 201
	private static int[] tweensFinished;

	// Token: 0x040000CA RID: 202
	private static LTDescr tween;

	// Token: 0x040000CB RID: 203
	private static int tweenMaxSearch = -1;

	// Token: 0x040000CC RID: 204
	private static int maxTweens = 400;

	// Token: 0x040000CD RID: 205
	private static int frameRendered = -1;

	// Token: 0x040000CE RID: 206
	private static GameObject _tweenEmpty;

	// Token: 0x040000CF RID: 207
	private static float dtEstimated = -1f;

	// Token: 0x040000D0 RID: 208
	public static float dtManual;

	// Token: 0x040000D1 RID: 209
	private static float dt;

	// Token: 0x040000D2 RID: 210
	private static float dtActual;

	// Token: 0x040000D3 RID: 211
	private static int i;

	// Token: 0x040000D4 RID: 212
	private static int j;

	// Token: 0x040000D5 RID: 213
	private static int finishedCnt;

	// Token: 0x040000D6 RID: 214
	private static AnimationCurve punch = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.112586f, 0.9976035f),
		new Keyframe(0.3120486f, -0.1720615f),
		new Keyframe(0.4316337f, 0.07030682f),
		new Keyframe(0.5524869f, -0.03141804f),
		new Keyframe(0.6549395f, 0.003909959f),
		new Keyframe(0.770987f, -0.009817753f),
		new Keyframe(0.8838775f, 0.001939224f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x040000D7 RID: 215
	private static AnimationCurve shake = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 1f),
		new Keyframe(0.75f, -1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x040000D8 RID: 216
	private static Transform trans;

	// Token: 0x040000D9 RID: 217
	private static float timeTotal;

	// Token: 0x040000DA RID: 218
	private static TweenAction tweenAction;

	// Token: 0x040000DB RID: 219
	private static float ratioPassed;

	// Token: 0x040000DC RID: 220
	private static float from;

	// Token: 0x040000DD RID: 221
	private static float val;

	// Token: 0x040000DE RID: 222
	private static bool isTweenFinished;

	// Token: 0x040000DF RID: 223
	private static int maxTweenReached;

	// Token: 0x040000E0 RID: 224
	private static Vector3 newVect;

	// Token: 0x040000E1 RID: 225
	private static GameObject target;

	// Token: 0x040000E2 RID: 226
	private static GameObject customTarget;

	// Token: 0x040000E3 RID: 227
	public static int startSearch = 0;

	// Token: 0x040000E4 RID: 228
	public static LTDescr d;

	// Token: 0x040000E5 RID: 229
	private static Action<LTEvent>[] eventListeners;

	// Token: 0x040000E6 RID: 230
	private static GameObject[] goListeners;

	// Token: 0x040000E7 RID: 231
	private static int eventsMaxSearch = 0;

	// Token: 0x040000E8 RID: 232
	public static int EVENTS_MAX = 10;

	// Token: 0x040000E9 RID: 233
	public static int LISTENERS_MAX = 10;

	// Token: 0x040000EA RID: 234
	private static int INIT_LISTENERS_MAX = LeanTween.LISTENERS_MAX;
}
