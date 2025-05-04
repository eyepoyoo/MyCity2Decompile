using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class LTGUI
{
	// Token: 0x060001C5 RID: 453 RVA: 0x0000CAE0 File Offset: 0x0000ACE0
	public static void init()
	{
		if (LTGUI.levels == null)
		{
			LTGUI.levels = new LTRect[LTGUI.RECT_LEVELS * LTGUI.RECTS_PER_LEVEL];
			LTGUI.levelDepths = new int[LTGUI.RECT_LEVELS];
		}
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000CB1C File Offset: 0x0000AD1C
	public static void initRectCheck()
	{
		if (LTGUI.buttons == null)
		{
			LTGUI.buttons = new Rect[LTGUI.BUTTONS_MAX];
			LTGUI.buttonLevels = new int[LTGUI.BUTTONS_MAX];
			LTGUI.buttonLastFrame = new int[LTGUI.BUTTONS_MAX];
			for (int i = 0; i < LTGUI.buttonLevels.Length; i++)
			{
				LTGUI.buttonLevels[i] = -1;
			}
		}
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000CB80 File Offset: 0x0000AD80
	public static void reset()
	{
		if (LTGUI.isGUIEnabled)
		{
			LTGUI.isGUIEnabled = false;
			for (int i = 0; i < LTGUI.levels.Length; i++)
			{
				LTGUI.levels[i] = null;
			}
			for (int j = 0; j < LTGUI.levelDepths.Length; j++)
			{
				LTGUI.levelDepths[j] = 0;
			}
		}
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0000CBE0 File Offset: 0x0000ADE0
	public static void update(int updateLevel)
	{
		if (LTGUI.isGUIEnabled)
		{
			LTGUI.init();
			if (LTGUI.levelDepths[updateLevel] > 0)
			{
				LTGUI.color = GUI.color;
				int num = updateLevel * LTGUI.RECTS_PER_LEVEL;
				int num2 = num + LTGUI.levelDepths[updateLevel];
				for (int i = num; i < num2; i++)
				{
					LTGUI.r = LTGUI.levels[i];
					if (LTGUI.r != null)
					{
						if (LTGUI.r.useColor)
						{
							GUI.color = LTGUI.r.color;
						}
						if (LTGUI.r.type == LTGUI.Element_Type.Label)
						{
							if (LTGUI.r.style != null)
							{
								GUI.skin.label = LTGUI.r.style;
							}
							if (LTGUI.r.useSimpleScale)
							{
								GUI.Label(new Rect((LTGUI.r.rect.x + LTGUI.r.margin.x + LTGUI.r.relativeRect.x) * LTGUI.r.relativeRect.width, (LTGUI.r.rect.y + LTGUI.r.margin.y + LTGUI.r.relativeRect.y) * LTGUI.r.relativeRect.height, LTGUI.r.rect.width * LTGUI.r.relativeRect.width, LTGUI.r.rect.height * LTGUI.r.relativeRect.height), LTGUI.r.labelStr);
							}
							else
							{
								GUI.Label(new Rect(LTGUI.r.rect.x + LTGUI.r.margin.x, LTGUI.r.rect.y + LTGUI.r.margin.y, LTGUI.r.rect.width, LTGUI.r.rect.height), LTGUI.r.labelStr);
							}
						}
						else if (LTGUI.r.type == LTGUI.Element_Type.Texture && LTGUI.r.texture != null)
						{
							Vector2 vector = ((!LTGUI.r.useSimpleScale) ? new Vector2(LTGUI.r.rect.width, LTGUI.r.rect.height) : new Vector2(0f, LTGUI.r.rect.height * LTGUI.r.relativeRect.height));
							if (LTGUI.r.sizeByHeight)
							{
								vector.x = (float)LTGUI.r.texture.width / (float)LTGUI.r.texture.height * vector.y;
							}
							if (LTGUI.r.useSimpleScale)
							{
								GUI.DrawTexture(new Rect((LTGUI.r.rect.x + LTGUI.r.margin.x + LTGUI.r.relativeRect.x) * LTGUI.r.relativeRect.width, (LTGUI.r.rect.y + LTGUI.r.margin.y + LTGUI.r.relativeRect.y) * LTGUI.r.relativeRect.height, vector.x, vector.y), LTGUI.r.texture);
							}
							else
							{
								GUI.DrawTexture(new Rect(LTGUI.r.rect.x + LTGUI.r.margin.x, LTGUI.r.rect.y + LTGUI.r.margin.y, vector.x, vector.y), LTGUI.r.texture);
							}
						}
					}
				}
				GUI.color = LTGUI.color;
			}
		}
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0000D018 File Offset: 0x0000B218
	public static bool checkOnScreen(Rect rect)
	{
		bool flag = rect.x + rect.width < 0f;
		bool flag2 = rect.x > (float)Screen.width;
		bool flag3 = rect.y > (float)Screen.height;
		bool flag4 = rect.y + rect.height < 0f;
		return !flag && !flag2 && !flag3 && !flag4;
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0000D08C File Offset: 0x0000B28C
	public static void destroy(int id)
	{
		int num = id & 65535;
		int num2 = id >> 16;
		if (id >= 0 && LTGUI.levels[num] != null && LTGUI.levels[num].hasInitiliazed && LTGUI.levels[num].counter == num2)
		{
			LTGUI.levels[num] = null;
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000D0E4 File Offset: 0x0000B2E4
	public static void destroyAll(int depth)
	{
		int num = depth * LTGUI.RECTS_PER_LEVEL + LTGUI.RECTS_PER_LEVEL;
		int num2 = depth * LTGUI.RECTS_PER_LEVEL;
		while (LTGUI.levels != null && num2 < num)
		{
			LTGUI.levels[num2] = null;
			num2++;
		}
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0000D12C File Offset: 0x0000B32C
	public static LTRect label(Rect rect, string label, int depth)
	{
		return LTGUI.label(new LTRect(rect), label, depth);
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000D13C File Offset: 0x0000B33C
	public static LTRect label(LTRect rect, string label, int depth)
	{
		rect.type = LTGUI.Element_Type.Label;
		rect.labelStr = label;
		return LTGUI.element(rect, depth);
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0000D154 File Offset: 0x0000B354
	public static LTRect texture(Rect rect, Texture texture, int depth)
	{
		return LTGUI.texture(new LTRect(rect), texture, depth);
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000D164 File Offset: 0x0000B364
	public static LTRect texture(LTRect rect, Texture texture, int depth)
	{
		rect.type = LTGUI.Element_Type.Texture;
		rect.texture = texture;
		return LTGUI.element(rect, depth);
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x0000D17C File Offset: 0x0000B37C
	public static LTRect element(LTRect rect, int depth)
	{
		LTGUI.isGUIEnabled = true;
		LTGUI.init();
		int num = depth * LTGUI.RECTS_PER_LEVEL + LTGUI.RECTS_PER_LEVEL;
		int num2 = 0;
		if (rect != null)
		{
			LTGUI.destroy(rect.id);
		}
		if (rect.type == LTGUI.Element_Type.Label && rect.style != null && rect.style.normal.textColor.a <= 0f)
		{
			Debug.LogWarning("Your GUI normal color has an alpha of zero, and will not be rendered.");
		}
		if (rect.relativeRect.width == float.PositiveInfinity)
		{
			rect.relativeRect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		}
		for (int i = depth * LTGUI.RECTS_PER_LEVEL; i < num; i++)
		{
			LTGUI.r = LTGUI.levels[i];
			if (LTGUI.r == null)
			{
				LTGUI.r = rect;
				LTGUI.r.rotateEnabled = true;
				LTGUI.r.alphaEnabled = true;
				LTGUI.r.setId(i, LTGUI.global_counter);
				LTGUI.levels[i] = LTGUI.r;
				if (num2 >= LTGUI.levelDepths[depth])
				{
					LTGUI.levelDepths[depth] = num2 + 1;
				}
				LTGUI.global_counter++;
				return LTGUI.r;
			}
			num2++;
		}
		Debug.LogError("You ran out of GUI Element spaces");
		return null;
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0000D2CC File Offset: 0x0000B4CC
	public static bool hasNoOverlap(Rect rect, int depth)
	{
		LTGUI.initRectCheck();
		bool flag = true;
		bool flag2 = false;
		for (int i = 0; i < LTGUI.buttonLevels.Length; i++)
		{
			if (LTGUI.buttonLevels[i] >= 0)
			{
				if (LTGUI.buttonLastFrame[i] + 1 < Time.frameCount)
				{
					LTGUI.buttonLevels[i] = -1;
				}
				else if (LTGUI.buttonLevels[i] > depth && LTGUI.pressedWithinRect(LTGUI.buttons[i]))
				{
					flag = false;
				}
			}
			if (!flag2 && LTGUI.buttonLevels[i] < 0)
			{
				flag2 = true;
				LTGUI.buttonLevels[i] = depth;
				LTGUI.buttons[i] = rect;
				LTGUI.buttonLastFrame[i] = Time.frameCount;
			}
		}
		return flag;
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x0000D38C File Offset: 0x0000B58C
	public static bool pressedWithinRect(Rect rect)
	{
		Vector2 vector = LTGUI.firstTouch();
		if (vector.x < 0f)
		{
			return false;
		}
		float num = (float)Screen.height - vector.y;
		return vector.x > rect.x && vector.x < rect.x + rect.width && num > rect.y && num < rect.y + rect.height;
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0000D414 File Offset: 0x0000B614
	public static bool checkWithinRect(Vector2 vec2, Rect rect)
	{
		vec2.y = (float)Screen.height - vec2.y;
		return vec2.x > rect.x && vec2.x < rect.x + rect.width && vec2.y > rect.y && vec2.y < rect.y + rect.height;
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000D494 File Offset: 0x0000B694
	public static Vector2 firstTouch()
	{
		if (Input.touchCount > 0)
		{
			return Input.touches[0].position;
		}
		if (Input.GetMouseButton(0))
		{
			return Input.mousePosition;
		}
		return new Vector2(float.NegativeInfinity, float.NegativeInfinity);
	}

	// Token: 0x0400011B RID: 283
	public static int RECT_LEVELS = 5;

	// Token: 0x0400011C RID: 284
	public static int RECTS_PER_LEVEL = 10;

	// Token: 0x0400011D RID: 285
	public static int BUTTONS_MAX = 24;

	// Token: 0x0400011E RID: 286
	private static LTRect[] levels;

	// Token: 0x0400011F RID: 287
	private static int[] levelDepths;

	// Token: 0x04000120 RID: 288
	private static Rect[] buttons;

	// Token: 0x04000121 RID: 289
	private static int[] buttonLevels;

	// Token: 0x04000122 RID: 290
	private static int[] buttonLastFrame;

	// Token: 0x04000123 RID: 291
	private static LTRect r;

	// Token: 0x04000124 RID: 292
	private static Color color = Color.white;

	// Token: 0x04000125 RID: 293
	private static bool isGUIEnabled = false;

	// Token: 0x04000126 RID: 294
	private static int global_counter = 0;

	// Token: 0x0200001E RID: 30
	public enum Element_Type
	{
		// Token: 0x04000128 RID: 296
		Texture,
		// Token: 0x04000129 RID: 297
		Label
	}
}
