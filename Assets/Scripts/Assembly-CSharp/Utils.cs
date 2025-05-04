using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using LitJson;
using UnityEngine;

public class Utils
{
	public class SimpleIntegerIdGenerator
	{
		private int _next;

		public int _pNew
		{
			get
			{
				return _next++;
			}
		}

		public SimpleIntegerIdGenerator()
		{
			Reset();
		}

		public void Reset()
		{
			_next = 0;
		}
	}

	private static List<string> _scenes;

	[Obsolete("This workaround is now no longer needed as the issue it fixes has been resolved in Unity")]
	public static int _pTouchCount_NoDoubles
	{
		get
		{
			UnityEngine.Debug.LogError("Obsolete: This workaround is now no longer needed as the issue it fixes has been resolved in Unity");
			return Input.touchCount;
		}
	}

	public static bool tryParseJson(string elementKey, ref string stringOut, JsonData dataToParse)
	{
		try
		{
			stringOut = dataToParse[elementKey].ToString();
			return true;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Utils:  Exception caught while getting element [" + elementKey + "]. Error [" + ex.Message + "]");
			return false;
		}
	}

	public static int Wrap(int kX, int kLowerBound, int kUpperBound)
	{
		int num = kUpperBound - kLowerBound + 1;
		if (kX < kLowerBound)
		{
			kX += num * ((kLowerBound - kX) / num + 1);
		}
		return kLowerBound + (kX - kLowerBound) % num;
	}

	public static float Wrap(float kX, float kLowerBound, float kUpperBound)
	{
		float num = kUpperBound - kLowerBound;
		if (kX < kLowerBound)
		{
			do
			{
				kX += num;
			}
			while (kX < kLowerBound);
		}
		return kLowerBound + (kX - kLowerBound) % num;
	}

	public static string ColorToHex(Color32 color, bool prefixHash = false)
	{
		string text = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return (!prefixHash) ? text : ("#" + text);
	}

	public static Color HexToColor(string hex)
	{
		if (hex == null || hex.Length == 0)
		{
			return Color.magenta;
		}
		int num = 0;
		if (hex[0] == '#')
		{
			num = 1;
		}
		if (hex.Length < num + 6)
		{
			return Color.magenta;
		}
		byte r = byte.Parse(hex.Substring(num, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(num + 2, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(num + 4, 2), NumberStyles.HexNumber);
		return new Color32(r, g, b, byte.MaxValue);
	}

	public static void ForAllChildren(GameObject obj, Action<GameObject> action, bool isRecursive)
	{
		if (!(obj != null) || action == null)
		{
			return;
		}
		foreach (Transform item in obj.transform)
		{
			action(item.gameObject);
			if (isRecursive)
			{
				ForAllChildren(item.gameObject, action, true);
			}
		}
	}

	public static void DestroyGameObject(UnityEngine.Object objectToDestroy)
	{
		if (!(objectToDestroy == null))
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(objectToDestroy);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(objectToDestroy);
			}
		}
	}

	public static void EnsureActiveState(GameObject objectToCheck, bool isEnabled)
	{
		if (!(objectToCheck == null) && objectToCheck.activeInHierarchy != isEnabled)
		{
			objectToCheck.SetActive(isEnabled);
		}
	}

	[Conditional("DEBUG_MESSAGES")]
	public static void Log(Color color, string message, UnityEngine.Object o = null)
	{
		UnityEngine.Debug.Log(message, o);
	}

	[Conditional("DEBUG_MESSAGES")]
	public static void LogBold(Color color, string message, UnityEngine.Object o = null)
	{
		UnityEngine.Debug.Log(message, o);
	}

	[Conditional("DEBUG_MESSAGES")]
	public static void LogBold(string message, UnityEngine.Object o = null)
	{
		UnityEngine.Debug.Log(message, o);
	}

	[Conditional("DEBUG_MESSAGES")]
	public static void LogSignal(string function, MonoBehaviour sender)
	{
		string message = function + ": from " + sender.GetFullName();
		UnityEngine.Debug.Log(message, sender);
	}

	public static void ProcessTriangles(IList<int> triangles, Func<int, int, int, int, bool> func)
	{
		if (func == null || triangles == null || triangles.Count == 0)
		{
			return;
		}
		bool flag = true;
		for (int i = 0; i < triangles.Count - 2; i += 3)
		{
			if (!flag)
			{
				break;
			}
			flag = func(i / 3, triangles[i], triangles[i + 1], triangles[i + 2]);
		}
	}

	public static void DoFixedTimeAction(Action<float> action, float timeElapsed, float timeStep, ref float timeRemaining)
	{
		timeRemaining += timeElapsed;
		while (timeRemaining >= timeStep)
		{
			action(timeStep);
			timeRemaining -= timeStep;
		}
	}

	public static void DictionaryAdd<KeyT, ValueT>(ref Dictionary<KeyT, ValueT> dict, bool isClearDict, int count, Func<int, KeyT> getKey, Func<int, ValueT> getValue, Predicate<ValueT> checkValue = null, Action<string> errorHandler = null)
	{
		if (dict == null)
		{
			dict = new Dictionary<KeyT, ValueT>(count);
		}
		else if (isClearDict)
		{
			dict.Clear();
		}
		Action<string> action = delegate(string error)
		{
			if (errorHandler != null)
			{
				errorHandler(error);
			}
		};
		if (getKey == null)
		{
			action("Null getKey function");
			return;
		}
		if (getValue == null)
		{
			action("Null getValue function");
			return;
		}
		for (int num = 0; num < count; num++)
		{
			KeyT key = getKey(num);
			if (dict.ContainsKey(key))
			{
				action("Key " + num + " ('" + key.ToString() + "') is duplicate");
				continue;
			}
			ValueT val = getValue(num);
			if (checkValue != null && !checkValue(val))
			{
				action("Value " + num + " ('" + val.ToString() + " is invalid");
			}
			else
			{
				dict.Add(key, val);
			}
		}
	}

	public static void Shuffle<T>(IList<T> list)
	{
		for (int num = list.Count - 1; num > 0; num--)
		{
			int index = UnityEngine.Random.Range(0, num);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	public static Texture2D GetSolidTexture(Color colour, int width = 1, int height = 1)
	{
		Texture2D texture2D = new Texture2D(width, height);
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				texture2D.SetPixel(i, j, colour);
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	public static string FormatLikeInspectorVariable(object str)
	{
		string text = string.Empty;
		bool flag = true;
		bool flag2 = false;
		string text2 = str.ToString();
		foreach (char c in text2)
		{
			if (!flag || char.IsLetter(c) || char.IsNumber(c))
			{
				bool flag3 = char.IsUpper(c) || char.IsNumber(c);
				text += ((!flag) ? (((!flag3 || flag2) ? string.Empty : " ") + c) : char.ToUpper(c).ToString());
				flag2 = flag3 || c == '_';
				flag = false;
			}
		}
		return text;
	}

	public static Vector2 GetReflectedVelocity(Vector2 velocity, Vector2 serfaceNormal)
	{
		velocity.Normalize();
		serfaceNormal.Normalize();
		Vector2 vector = Vector2.Dot(velocity, serfaceNormal) / Vector2.Dot(serfaceNormal, serfaceNormal) * serfaceNormal;
		Vector2 vector2 = velocity - vector;
		return vector2 - vector;
	}

	public static string arrayToCSV<T>(T[] array)
	{
		if (array == null)
		{
			return "NULL";
		}
		StringBuilder stringBuilder = new StringBuilder();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			stringBuilder.Append(array[i].ToString());
			if (i != num - 1)
			{
				stringBuilder.Append(",");
			}
		}
		return stringBuilder.ToString();
	}

	public static bool isBase64Encoded(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return false;
		}
		str = Regex.Replace(str, "\\s+", string.Empty);
		if (str.Length % 4 != 0)
		{
			return false;
		}
		try
		{
			byte[] array = Convert.FromBase64String(str);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
