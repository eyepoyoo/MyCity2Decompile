using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using AmuzoEngine;
using DeepThought;
using LitJson;
using UnityEngine;

public static class Extensions
{
	public delegate void DInvokedFunction();

	private const string LOG_TAG = "[JsonExtensions] ";

	private static string PREPEND_FILE_PATH = (Application.platform == RuntimePlatform.WebGLPlayer ? string.Empty : "file:///");

	private static string FULL_DATA_PATH = PREPEND_FILE_PATH + Application.dataPath;

	private static JsonWriter _jsonWriter;

	private static StringWriter _jsonStringWriter;

	private static bool _jsonIsBeginObject;

	public static string PrintFormatXML(string xml)
	{
		if (Application.isEditor)
		{
			return xml;
		}
		return xml.Replace("<", "&lt;").Replace(">", "&gt;");
	}

	public static Neuron CreateNode(string name, string type, Neuron parent, string value)
	{
		if (parent == null)
		{
			return Neuron.create(type, name, value);
		}
		return parent.addChild(type, name, value);
	}

	public static Neuron FirstChildOfType(this Neuron neuron, string type)
	{
		if (neuron == null)
		{
			return null;
		}
		foreach (Neuron item in (IEnumerable<Neuron>)neuron)
		{
			if (item != null && item.getType() == type)
			{
				return item;
			}
		}
		return null;
	}

	public static Neuron VerifyProperty(this Neuron neuron, string name, bool default_)
	{
		Neuron neuron2 = neuron.tryGetChild(name);
		if (neuron2 == null)
		{
			neuron.setValue(name, default_);
			neuron2 = neuron.getChild(name);
		}
		return neuron2;
	}

	public static Neuron VerifyProperty(this Neuron neuron, string name, float default_)
	{
		Neuron neuron2 = neuron.tryGetChild(name);
		if (neuron2 == null)
		{
			neuron.setValue(name, default_);
			neuron2 = neuron.getChild(name);
		}
		return neuron2;
	}

	public static Neuron VerifyProperty(this Neuron neuron, string name, string default_)
	{
		Neuron neuron2 = neuron.tryGetChild(name);
		if (neuron2 == null)
		{
			neuron.setValue(name, default_);
			neuron2 = neuron.getChild(name);
		}
		return neuron2;
	}

	public static Neuron VerifyProperty(this Neuron neuron, string name, Neuron default_)
	{
		Neuron neuron2 = neuron.tryGetChild(name);
		if (neuron2 == null)
		{
			neuron.setValue(name, default_);
			neuron2 = neuron.getChild(name);
		}
		return neuron2;
	}

	public static void SetRefValue(this Neuron neuron, float value)
	{
		Value value2 = neuron.getValue();
		switch (value2.getType())
		{
		case "Neuron":
			if (Debug.isDebugBuild)
			{
				Debug.LogError("Cannot set value " + neuron.getName() + " because it is a referenced property");
			}
			break;
		default:
			value2.setNumber(value);
			break;
		}
	}

	public static void SetRefValue(this Neuron neuron, string value)
	{
		Value value2 = neuron.getValue();
		switch (value2.getType())
		{
		case "Neuron":
			if (Debug.isDebugBuild)
			{
				Debug.LogError("Cannot set value " + neuron.getName() + " because it is a referenced property");
			}
			break;
		default:
			value2.setText(value);
			break;
		}
	}

	public static string DateTimeToString(DateTime value)
	{
		return value.ToBinary().ToString(CultureInfo.InvariantCulture);
	}

	public static DateTime DateTimeFromString(string value)
	{
		long result;
		return (!long.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result)) ? DateTime.MinValue : DateTime.FromBinary(result);
	}

	public static void SetDateTime(this Neuron neuron, DateTime value)
	{
		neuron.Text = DateTimeToString(value);
	}

	public static DateTime GetDateTime(this Neuron neuron)
	{
		return DateTimeFromString(neuron.Text);
	}

	public static Neuron CreateMessage(string name, string direction)
	{
		Neuron neuron = CreateNode(name, "node", null, string.Empty);
		if (direction != null)
		{
			neuron.setValue("propagate", direction);
		}
		return neuron;
	}

	public static bool Start(this TimelineNeuron timeline)
	{
		if (timeline != null)
		{
			timeline.reset();
			timeline.start();
			return true;
		}
		return false;
	}

	public static void Stop(this TimelineNeuron timeline)
	{
		if (timeline != null)
		{
			timeline.reset();
			timeline.setValue("startTime", -1f);
			Neuron neuron = timeline.tryGetChild("exit");
			if (neuron != null)
			{
				TimelineNeuron timeline2 = neuron.NeuronRef as TimelineNeuron;
				timeline2.Start();
			}
		}
	}

	public static string[] GetStringValues(this Neuron node)
	{
		List<string> list = new List<string>(node.numChildren);
		AddStringValuesToList(node, list);
		return list.ToArray();
	}

	public static void AddStringValuesToList(Neuron root, List<string> list)
	{
		int num = 0;
		while (true)
		{
			Neuron neuron = root.tryGetChild(string.Format("value_{0}", num++));
			if (neuron != null)
			{
				list.Add(neuron.Text);
				continue;
			}
			break;
		}
	}

	public static void SetStringValues(this Neuron node, string[] list)
	{
		Neuron neuron = null;
		int i;
		for (i = 0; i < list.Length; i++)
		{
			node.setValue(string.Format("value_{0}", i), list[i]);
			node.getChild(string.Format("value_{0}", i)).makeLeaf();
		}
		while (neuron != null)
		{
			neuron = node.tryGetChild(string.Format("value_{0}", i));
			if (neuron != null)
			{
				neuron.kill();
			}
			i++;
		}
	}

	public static void addJsonWithRoot(this Neuron node, string jsonRootName, string json)
	{
		node.addChild(jsonRootName).add(json);
	}

	public static void EnumerateDictionary<T1, T2>(Dictionary<T1, T2> dict, Action<T1, T2> action)
	{
		foreach (KeyValuePair<T1, T2> item in dict)
		{
			action(item.Key, item.Value);
		}
	}

	public static void hardDestroyGameObject(ref GameObject objectToDestroy)
	{
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(objectToDestroy);
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(objectToDestroy);
		}
		objectToDestroy = null;
		objectToDestroy = new GameObject();
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(objectToDestroy);
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(objectToDestroy);
		}
		objectToDestroy = null;
	}

	public static string GetBootParam(string value, bool doValidate = true)
	{
		return null;
	}

	public static string ValidateURL(string url)
	{
		url = WWW.UnEscapeURL(url);
		if (!url.StartsWith("http"))
		{
			return string.Format("{0}/{1}", FULL_DATA_PATH.Replace("%20", " "), url);
		}
		return url;
	}

	public static void SafeClear<T>(this List<T> list)
	{
		if (list != null && list.Count > 0)
		{
			list.Clear();
		}
	}

	public static string GetFullName(this GameObject gob, char separator = '/')
	{
		return (!(gob.transform.parent != null)) ? (separator + gob.name) : (gob.transform.parent.gameObject.GetFullName(separator) + separator + gob.name);
	}

	public static string GetFullName(this MonoBehaviour bhv, char separator = '/')
	{
		return bhv.gameObject.GetFullName(separator);
	}

	public static ComponentT EnsureComponent<ComponentT>(this GameObject obj, Action<ComponentT> onNewComponent = null) where ComponentT : Component
	{
		ComponentT val = obj.GetComponent<ComponentT>();
		if (val == null)
		{
			val = obj.AddComponent<ComponentT>();
			if (onNewComponent != null)
			{
				onNewComponent(val);
			}
		}
		return val;
	}

	public static ComponentT EnsureComponent<ComponentT>(this MonoBehaviour bhv, Action<ComponentT> onNewComponent = null) where ComponentT : Component
	{
		return bhv.gameObject.EnsureComponent(onNewComponent);
	}

	public static GameObjectState GetState(this GameObject obj)
	{
		return GameObjectState.GetState(obj);
	}

	public static void SetState(this GameObject obj, GameObjectState state)
	{
		GameObjectState.SetState(obj, state);
	}

	public static void Add<T>(this List<T> list, params T[] elements)
	{
		list.AddRange(elements);
	}

	public static TComponentToReturn Instantiate<TComponentToReturn>(this UnityEngine.Object o, GameObject original) where TComponentToReturn : Component
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(original);
		TComponentToReturn component = gameObject.GetComponent<TComponentToReturn>();
		if (!component)
		{
			Debug.LogError(string.Format("Object \"{0}\" does not have component \"{1}\" attached", gameObject, typeof(TComponentToReturn).ToString()));
			return (TComponentToReturn)null;
		}
		return component;
	}

	public static TComponentToReturn Instantiate<TComponentToReturn>(this UnityEngine.Object o, string prefabName) where TComponentToReturn : Component
	{
		GameObject gameObject = Resources.Load<GameObject>(prefabName);
		if (!gameObject)
		{
			Debug.LogError(string.Format("Couldn't load prefab \"{0}\" from resources", prefabName));
			return (TComponentToReturn)null;
		}
		return o.Instantiate<TComponentToReturn>(gameObject);
	}

	public static string ToRGBHash(this Color color)
	{
		int num = (int)(color.r * 255f);
		int num2 = (int)(color.g * 255f);
		int num3 = (int)(color.b * 255f);
		return "#" + num.ToString("X2") + num2.ToString("X2") + num3.ToString("X2");
	}

	public static void Invoke(this MonoBehaviour mb, DInvokedFunction method, float time)
	{
		mb.Invoke(method.Method.Name, time);
	}

	public static void Invoke_TimeScaleIndependent(this MonoBehaviour mb, DInvokedFunction method, float time)
	{
		mb.StartCoroutine(_Invoke_TimeScaleIndependent(method, time));
	}

	private static IEnumerator _Invoke_TimeScaleIndependent(DInvokedFunction method, float time)
	{
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < startTime + time)
		{
			yield return null;
		}
		method();
	}

	public static void Invoke_AfterFrames(this MonoBehaviour mb, DInvokedFunction method, int frames)
	{
		mb.StartCoroutine(_Invoke_AfterFrames(method, frames));
	}

	private static IEnumerator _Invoke_AfterFrames(DInvokedFunction method, int frames)
	{
		int startFrame = Time.frameCount;
		while (Time.frameCount < startFrame + frames)
		{
			yield return null;
		}
		method();
	}

	public static void InvokeRepeating(this MonoBehaviour mb, DInvokedFunction method, float time, float repeatRate)
	{
		mb.InvokeRepeating(method.Method.Name, time, repeatRate);
	}

	public static void InvokeRepeating(this MonoBehaviour mb, DInvokedFunction method, float interval)
	{
		mb.InvokeRepeating(method, interval, interval);
	}

	public static void CancelInvoke(this MonoBehaviour mb, DInvokedFunction method)
	{
		mb.CancelInvoke(method.Method.Name);
	}

	public static WWW HttpPost(string url, Dictionary<string, string> post)
	{
		WWWForm wWWForm = new WWWForm();
		foreach (KeyValuePair<string, string> item in post)
		{
			wWWForm.AddField(item.Key, item.Value);
		}
		return new WWW(url, wWWForm);
	}

	public static bool HasFieldOrProperty(this object target, string name, Type type = null)
	{
		FieldInfo field = target.GetType().GetField(name);
		if (field != null && (type == null || field.FieldType == type))
		{
			return true;
		}
		PropertyInfo property = target.GetType().GetProperty(name);
		if (property != null && (type == null || property.PropertyType == type))
		{
			return true;
		}
		return false;
	}

	public static void SetFieldOrProperty(this object target, string name, object value, bool logErrorIfNotFound = false)
	{
		FieldInfo field = target.GetType().GetField(name);
		if (field != null)
		{
			field.SetValue(target, value);
			return;
		}
		PropertyInfo property = target.GetType().GetProperty(name);
		if (property != null)
		{
			property.SetValue(target, value, null);
		}
		else if (logErrorIfNotFound)
		{
			Debug.LogError(string.Format("Property \"{0}\" not found on {1}!", name, target));
		}
	}

	public static object GetFieldOrProperty(this object target, string name, bool logErrorIfNotFound = false)
	{
		FieldInfo field = target.GetType().GetField(name);
		if (field != null)
		{
			return field.GetValue(target);
		}
		PropertyInfo property = target.GetType().GetProperty(name);
		if (property != null)
		{
			return property.GetValue(target, null);
		}
		if (logErrorIfNotFound)
		{
			Debug.LogError(string.Format("Property \"{0}\" not found on {1}!", name, target));
		}
		return null;
	}

	public static Type GetFieldOrPropertyType(this object target, string name, bool logErrorIfNotFound = false)
	{
		FieldInfo field = target.GetType().GetField(name);
		if (field != null)
		{
			return field.FieldType;
		}
		PropertyInfo property = target.GetType().GetProperty(name);
		if (property != null)
		{
			return property.PropertyType;
		}
		if (logErrorIfNotFound)
		{
			Debug.LogError(string.Format("Property \"{0}\" not found on {1}!", name, target));
		}
		return null;
	}

	public static Rect Translate(this Rect target, float x, float y)
	{
		return new Rect(target.xMin + x, target.yMin + y, target.width, target.height);
	}

	public static Rect Translate(this Rect target, Vector2 t)
	{
		return new Rect(target.xMin + t.x, target.yMin + t.y, target.width, target.height);
	}

	public static bool Is(this Type target, Type type)
	{
		if (target.BaseType == type)
		{
			return true;
		}
		if (target.BaseType == null)
		{
			return false;
		}
		return target.BaseType.Is(type);
	}

	public static Tween TweenToPos(this Transform target, Vector3 to, float duration, Action<Tween> onComplete = null, Easing.EaseType interpFunc = Easing.EaseType.Linear, bool timeScaleIndependent = true, bool isLocal = false, float delay = 0f)
	{
		Tween tween = Array.Find(target.gameObject.GetComponents<Tween>(), (Tween t) => t._doTweenPos) ?? target.gameObject.AddComponent<Tween>();
		tween.Init(duration, onComplete, null, to, null, null, interpFunc, timeScaleIndependent, isLocal, delay);
		return tween;
	}

	public static void TweenToRot(this Transform target, Quaternion to, float duration, Action<Tween> onComplete = null, Easing.EaseType interpFunc = Easing.EaseType.Linear, bool timeScaleIndependent = true, bool isLocal = false, float delay = 0f)
	{
		Tween tween = Array.Find(target.gameObject.GetComponents<Tween>(), (Tween t) => t._doTweenRot) ?? target.gameObject.AddComponent<Tween>();
		tween.Init(duration, onComplete, null, null, null, to, interpFunc, timeScaleIndependent, isLocal, delay);
	}

	public static void TweenTo(this Transform target, Vector3 toPos, Quaternion toRot, float duration, Action<Tween> onComplete = null, Easing.EaseType interpFunc = Easing.EaseType.Linear, bool timeScaleIndependent = true, bool isLocal = false, float delay = 0f)
	{
		Array.ForEach(target.gameObject.GetComponents<Tween>(), delegate(Tween t)
		{
			t.enabled = false;
		});
		Tween tween = target.gameObject.GetComponent<Tween>() ?? target.gameObject.AddComponent<Tween>();
		tween.Init(duration, onComplete, null, toPos, null, toRot, interpFunc, timeScaleIndependent, isLocal, delay);
	}

	public static void RemoveTweens(this Transform target)
	{
		Array.ForEach(target.GetComponents<Tween>(), delegate(Tween t)
		{
			UnityEngine.Object.Destroy(t);
		});
	}

	public static void SetDoPersistBetweenSceneLoads(this Transform target, bool doPersist, bool onceOnly = false)
	{
		if (doPersist)
		{
			DontDestroyOnLoadBunker._pInstance.Add(target, onceOnly);
		}
		else
		{
			DontDestroyOnLoadBunker._pInstance.Remove(target);
		}
	}

	public static T First<T>(this IList<T> targ)
	{
		return (targ.Count != 0) ? targ[0] : default(T);
	}

	public static T Last<T>(this IList<T> targ)
	{
		return (targ.Count != 0) ? targ[targ.Count - 1] : default(T);
	}

	public static void Randomise<T>(this IList<T> targ)
	{
		List<T> list = new List<T>(targ);
		for (int i = 0; i < targ.Count; i++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			targ[i] = list[index];
			list.RemoveAt(index);
		}
	}

	public static T[] Push<T>(this T[] targ, T item)
	{
		T[] array = new T[targ.Length + 1];
		targ.CopyTo(array, 0);
		array[array.Length - 1] = item;
		return array;
	}

	public static T GetRandom<T>(this IList<T> list)
	{
		if (list.Count == 0)
		{
			return default(T);
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public static T GetRandom<T>(this IList<T> list, bool doRemove)
	{
		if (list.Count == 0)
		{
			return default(T);
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		T result = list[index];
		if (doRemove)
		{
			list.RemoveAt(index);
		}
		return result;
	}

	public static T[] Concat<T>(this IList<T> targ, IList<T> other)
	{
		T[] array = new T[targ.Count + other.Count];
		targ.CopyTo(array, 0);
		other.CopyTo(array, targ.Count);
		return array;
	}

	public static T[] Add<T>(this IList<T> targ, params T[] others)
	{
		T[] array = new T[targ.Count + others.Length];
		targ.CopyTo(array, 0);
		others.CopyTo(array, targ.Count);
		return array;
	}

	public static bool Contains<T>(this IList<T> targ, T item)
	{
		return targ.IndexOf(item) != -1;
	}

	public static void Remove<T>(this IList<T> targ, IList<T> items)
	{
		foreach (T item in items)
		{
			targ.Remove(item);
		}
	}

	public static bool IsPointInFrustrum(this Camera targ, Vector3 point, float normBorder = 0f)
	{
		Vector3 vector = targ.WorldToViewportPoint(point);
		return vector.x > normBorder && vector.x < 1f - normBorder && vector.y > normBorder && vector.y < 1f - normBorder && vector.z > 0f;
	}

	public static bool AreBoundsInView(this Camera targ, Bounds bounds)
	{
		return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(targ), bounds);
	}

	public static float SqrMagnitudeXZ(this Vector3 targ)
	{
		return targ.x * targ.x + targ.z * targ.z;
	}

	public static float MagnitudeXZ(this Vector3 targ)
	{
		return Mathf.Sqrt(targ.x * targ.x + targ.z * targ.z);
	}

	public static Vector2 RotateRight90(this Vector2 targ)
	{
		return new Vector2(targ.y, 0f - targ.x);
	}

	public static Vector2 RotateLeft90(this Vector2 targ)
	{
		return new Vector2(0f - targ.y, targ.x);
	}

	public static void EaseToAngle(this Rigidbody targ, float angleTarget, Vector3 localAxis, float acceleration, float damping)
	{
		Vector3 vector = targ.transform.TransformDirection(localAxis);
		float num = MathHelper.Wrap(Vector3.Dot(targ.transform.eulerAngles, localAxis) - angleTarget, -180f, 180f);
		float num2 = Vector3.Dot(targ.angularVelocity, vector) * 57.29578f;
		targ.AddTorque(vector * ((0f - num) * acceleration - num2 * damping), ForceMode.Acceleration);
	}

	public static void EaseToY(this Rigidbody targ, float y, float acceleration, float damping)
	{
		float num = targ.transform.position.y - y;
		float y2 = targ.velocity.y;
		targ.AddForce(Vector3.up * ((0f - num) * acceleration - y2 * damping), ForceMode.Acceleration);
	}

	public static void SetMeshesVisible(this Component targ, bool visible)
	{
		MeshRenderer[] componentsInChildren = targ.GetComponentsInChildren<MeshRenderer>();
		for (int num = componentsInChildren.Length - 1; num >= 0; num--)
		{
			componentsInChildren[num].enabled = visible;
		}
	}

	public static JsonData JsonAtPath(this JsonData root, string path)
	{
		path = path.Replace(".", "/");
		return JsonAtPath(root, path.Split('/'), 0);
	}

	public static JsonData JsonAtPath(JsonData root, string[] nodes, int startIndex)
	{
		if (root == null)
		{
			return null;
		}
		JsonData jsonData = root;
		for (int i = startIndex; i < nodes.Length; i++)
		{
			jsonData = jsonData.TryGet(nodes[i]);
			if (jsonData == null)
			{
				return null;
			}
		}
		return jsonData;
	}

	public static JsonData LoadJson(string json)
	{
		if (json == null || json == string.Empty)
		{
			return null;
		}
		JsonReader reader = new JsonReader(json);
		return JsonMapper.ToObject(reader);
	}

	public static string PrettyPrint(JsonData data)
	{
		StringWriter stringWriter = new StringWriter();
		JsonWriter jsonWriter = new JsonWriter(stringWriter);
		jsonWriter.Validate = true;
		jsonWriter.PrettyPrint = true;
		data.ToJson(jsonWriter);
		return stringWriter.ToString();
	}

	public static string ToDebug(this JsonData data, string title = null)
	{
		StringWriter stringWriter = new StringWriter();
		if (title != null)
		{
			stringWriter.WriteLine(title + ":");
		}
		DebugJson(data, stringWriter, 0);
		return stringWriter.ToString();
	}

	public static JsonWriter BeginJson(bool isBeginObject = true, bool isWantValidate = true)
	{
		if (_jsonStringWriter != null)
		{
			Debug.LogError("BeginJson: need to EndJson() first!");
			return null;
		}
		_jsonStringWriter = new StringWriter();
		_jsonWriter = new JsonWriter(_jsonStringWriter);
		_jsonWriter.Validate = isWantValidate;
		_jsonIsBeginObject = isBeginObject;
		if (_jsonIsBeginObject)
		{
			_jsonWriter.WriteObjectStart();
		}
		return _jsonWriter;
	}

	public static string EndJson()
	{
		if (_jsonStringWriter == null)
		{
			Debug.LogError("EndJson: need to BeginJson() first!");
			return null;
		}
		if (_jsonIsBeginObject)
		{
			_jsonWriter.WriteObjectEnd();
		}
		string result = _jsonStringWriter.ToString();
		_jsonStringWriter = null;
		_jsonWriter = null;
		return result;
	}

	public static void WriteValue(this JsonWriter writer, string propertyName, object obj)
	{
		writer.WritePropertyName(propertyName);
		if (obj is bool)
		{
			writer.Write((bool)obj);
		}
		else if (obj is decimal)
		{
			writer.Write((decimal)obj);
		}
		else if (obj is double)
		{
			writer.Write((double)obj);
		}
		else if (obj is int)
		{
			writer.Write((int)obj);
		}
		else if (obj is long)
		{
			writer.Write((long)obj);
		}
		else if (obj is ulong)
		{
			writer.Write((ulong)obj);
		}
		else
		{
			writer.Write(obj.ToString());
		}
	}

	public static void WriteValue(this JsonWriter writer, string propertyName, bool boolean)
	{
		writer.WritePropertyName(propertyName);
		writer.Write(boolean);
	}

	public static void WriteValue(this JsonWriter writer, string propertyName, decimal number)
	{
		writer.WritePropertyName(propertyName);
		writer.Write(number);
	}

	public static void WriteValue(this JsonWriter writer, string propertyName, double number)
	{
		writer.WritePropertyName(propertyName);
		writer.Write(number);
	}

	public static void WriteValue(this JsonWriter writer, string propertyName, int number)
	{
		writer.WritePropertyName(propertyName);
		writer.Write(number);
	}

	public static void WriteValue(this JsonWriter writer, string propertyName, long number)
	{
		writer.WritePropertyName(propertyName);
		writer.Write(number);
	}

	public static void WriteValue(this JsonWriter writer, string propertyName, ulong number)
	{
		writer.WritePropertyName(propertyName);
		writer.Write(number);
	}

	public static void WriteValue(this JsonWriter writer, string propertyName, string str)
	{
		writer.WritePropertyName(propertyName);
		writer.Write(str);
	}

	public static void WriteArray(this JsonWriter writer, string propertyName, int[] array)
	{
		writer.WritePropertyName(propertyName);
		writer.WriteArrayStart();
		foreach (int number in array)
		{
			writer.Write(number);
		}
		writer.WriteArrayEnd();
	}

	public static void WriteArray(this JsonWriter writer, string propertyName, string[] array)
	{
		writer.WritePropertyName(propertyName);
		writer.WriteArrayStart();
		foreach (string str in array)
		{
			writer.Write(str);
		}
		writer.WriteArrayEnd();
	}

	public static void WriteList(this JsonWriter writer, string propertyName, List<int> list)
	{
		writer.WritePropertyName(propertyName);
		writer.WriteArrayStart();
		for (int i = 0; i < list.Count; i++)
		{
			int number = list[i];
			writer.Write(number);
		}
		writer.WriteArrayEnd();
	}

	public static void WriteArray(this JsonWriter writer, string propertyName, bool[] array)
	{
		writer.WritePropertyName(propertyName);
		writer.WriteArrayStart();
		foreach (bool boolean in array)
		{
			writer.Write(boolean);
		}
		writer.WriteArrayEnd();
	}

	public static void WriteList(this JsonWriter writer, string propertyName, List<bool> list)
	{
		writer.WritePropertyName(propertyName);
		writer.WriteArrayStart();
		for (int i = 0; i < list.Count; i++)
		{
			bool boolean = list[i];
			writer.Write(boolean);
		}
		writer.WriteArrayEnd();
	}

	public static bool WriteJsonData(this JsonWriter writer, JsonData jd, Func<JsonData, string, bool> errorHandler = null)
	{
		Func<string, bool> func = (string msg) => errorHandler != null && errorHandler(jd, msg);
		if (jd == null)
		{
			return func("Null JsonData");
		}
		switch (jd.GetJsonType())
		{
		default:
			return func("Invalid JsonData");
		case JsonType.Object:
			writer.WriteObjectStart();
			foreach (DictionaryEntry item in (IOrderedDictionary)jd)
			{
				if (item.Key == null || !(item.Key is string))
				{
					return func("Null or invalid object key");
				}
				if (item.Value == null || !(item.Value is JsonData))
				{
					return func("Null or invalid object value");
				}
				writer.WritePropertyName((string)item.Key);
				if (!writer.WriteJsonData((JsonData)item.Value, errorHandler))
				{
					return false;
				}
			}
			writer.WriteObjectEnd();
			break;
		case JsonType.Array:
		{
			writer.WriteArrayStart();
			for (int num = 0; num < jd.Count; num++)
			{
				if (jd[num] == null)
				{
					return func("Null array element");
				}
				if (!writer.WriteJsonData(jd[num], errorHandler))
				{
					return false;
				}
			}
			writer.WriteArrayEnd();
			break;
		}
		case JsonType.String:
			writer.Write((string)jd);
			break;
		case JsonType.Int:
			writer.Write((int)jd);
			break;
		case JsonType.Long:
			writer.Write((long)jd);
			break;
		case JsonType.Double:
			writer.Write((double)jd);
			break;
		case JsonType.Boolean:
			writer.Write((bool)jd);
			break;
		}
		return true;
	}

	public static string SafeToJson(this JsonData jd, Action<string> errorHandler = null)
	{
		if (jd == null)
		{
			return string.Empty;
		}
		Action<string> handleError = delegate(string msg)
		{
			if (errorHandler != null)
			{
				errorHandler(msg);
			}
		};
		Func<JsonData, string, bool> errorHandler2 = delegate(JsonData jsonData, string msg)
		{
			handleError(msg);
			return false;
		};
		JsonWriter jsonWriter = BeginJson(false, false);
		if (jsonWriter == null)
		{
			handleError("Cannot write json");
			return string.Empty;
		}
		if (!jsonWriter.WriteJsonData(jd, errorHandler2))
		{
			EndJson();
			handleError("Failed to write json");
			return string.Empty;
		}
		return EndJson();
	}

	public static JsonData TryGet(this JsonData data, string key)
	{
		if (data != null && data.Contains(key))
		{
			return data[key];
		}
		return null;
	}

	public static JsonData TryGet(this JsonData data, string key, JsonType type)
	{
		JsonData jsonData = data.TryGet(key);
		if (jsonData == null || jsonData.GetJsonType() != type)
		{
			return null;
		}
		return jsonData;
	}

	public static string TryGetString(this JsonData data, string key, string defaultValue = "")
	{
		if (data != null && data.Contains(key))
		{
			JsonData jsonData = data[key];
			if (jsonData.GetJsonType() == JsonType.String)
			{
				return (string)jsonData;
			}
		}
		return defaultValue;
	}

	public static int TryGetInt(this JsonData data, string key, int defaultValue = 0)
	{
		if (data != null && data.Contains(key))
		{
			JsonData jsonData = data[key];
			if (jsonData.GetJsonType() == JsonType.Int)
			{
				return (int)jsonData;
			}
		}
		return defaultValue;
	}

	public static double TryGetDouble(this JsonData data, string key, double defaultValue = 0.0)
	{
		if (data != null && data.Contains(key))
		{
			JsonData jsonData = data[key];
			if (jsonData.GetJsonType() == JsonType.Double)
			{
				return (double)jsonData;
			}
		}
		return defaultValue;
	}

	public static bool TryGetBool(this JsonData data, string key, bool defaultValue = false)
	{
		if (data != null && data.Contains(key))
		{
			JsonData jsonData = data[key];
			if (jsonData.GetJsonType() == JsonType.Boolean)
			{
				return (bool)jsonData;
			}
		}
		return defaultValue;
	}

	public static void AddChildren(this JsonData data, JsonData root)
	{
		foreach (DictionaryEntry item in (IOrderedDictionary)root)
		{
			string prop_name = (string)item.Key;
			data[prop_name] = item.Value as JsonData;
		}
	}

	public static void Enumerate(this JsonData data, Action<string, JsonData> action)
	{
		foreach (DictionaryEntry item in (IOrderedDictionary)data)
		{
			action((string)item.Key, item.Value as JsonData);
		}
	}

	public static JsonData DeepCopyJson(JsonData src)
	{
		JsonData jsonData = null;
		if (src != null)
		{
			switch (src.GetJsonType())
			{
			case JsonType.Array:
			{
				jsonData = new JsonData();
				jsonData.SetJsonType(JsonType.Array);
				for (int i = 0; i < src.Count; i++)
				{
					jsonData.Add(DeepCopyJson(src[i]));
				}
				break;
			}
			case JsonType.Boolean:
				jsonData = new JsonData((bool)src);
				break;
			case JsonType.Double:
				jsonData = new JsonData((double)src);
				break;
			case JsonType.Int:
				jsonData = new JsonData((int)src);
				break;
			case JsonType.Long:
				jsonData = new JsonData((long)src);
				break;
			case JsonType.None:
				jsonData = new JsonData();
				break;
			case JsonType.Object:
				jsonData = new JsonData();
				jsonData.SetJsonType(JsonType.Object);
				foreach (DictionaryEntry item in (IOrderedDictionary)src)
				{
					jsonData[(string)item.Key] = DeepCopyJson((JsonData)item.Value);
				}
				break;
			case JsonType.String:
				jsonData = new JsonData((string)src);
				break;
			}
		}
		return jsonData;
	}

	public static JsonData MergeJsonObjects(JsonData objA, JsonData objB)
	{
		JsonData jsonData = null;
		if (objA != null && objA.IsObject)
		{
			jsonData = DeepCopyJson(objA);
			if (objB != null && objB.IsObject)
			{
				MergeJsonObjectChildren(jsonData, objB);
			}
		}
		else if (objB != null && objB.IsObject)
		{
			jsonData = DeepCopyJson(objB);
		}
		else
		{
			jsonData = new JsonData();
			jsonData.SetJsonType(JsonType.Object);
		}
		return jsonData;
	}

	public static bool MergeJsonObjectChildren(JsonData objA, JsonData objB)
	{
		if (objA == null)
		{
			return false;
		}
		if (!objA.IsObject)
		{
			return false;
		}
		if (objB == null)
		{
			return false;
		}
		if (!objB.IsObject)
		{
			return false;
		}
		foreach (DictionaryEntry item in (IOrderedDictionary)objB)
		{
			string text = (string)item.Key;
			JsonData jsonData = (JsonData)item.Value;
			if (jsonData.IsObject && objA.Contains(text))
			{
				JsonData jsonData2 = objA[text];
				if (jsonData2.IsObject)
				{
					MergeJsonObjectChildren(jsonData2, jsonData);
				}
				else
				{
					objA[text] = DeepCopyJson(jsonData);
				}
			}
			else
			{
				objA[text] = DeepCopyJson(jsonData);
			}
		}
		return true;
	}

	public static void TryRemove(this JsonData thisData, string key)
	{
		if (thisData != null && thisData.IsObject)
		{
			if (((IDictionary)thisData).Contains((object)key))
			{
				((IDictionary)thisData).Remove((object)key);
			}
		}
	}

	public static void TryRemove(this JsonData thisData, JsonData childData)
	{
		if (thisData == null || (!thisData.IsArray && !thisData.IsObject))
		{
			return;
		}
		int count = thisData.Count;
		int i;
		for (i = 0; i < count && thisData[i] != childData; i++)
		{
		}
		if (i != count)
		{
			if (thisData.IsArray)
			{
				((IList)thisData).RemoveAt(i);
			}
			else if (thisData.IsObject)
			{
				((IOrderedDictionary)thisData).RemoveAt(i);
			}
		}
	}

	public static object GetValue(this JsonData data, bool castDoubleToFloat = false)
	{
		switch (data.GetJsonType())
		{
		case JsonType.Array:
			return (IList<JsonData>)data;
		case JsonType.Boolean:
			return (bool)data;
		case JsonType.Double:
			if (castDoubleToFloat)
			{
				return (float)(double)data;
			}
			return (double)data;
		case JsonType.Int:
			return (int)data;
		case JsonType.Long:
			return (long)data;
		case JsonType.Object:
			return (IDictionary<string, JsonData>)data;
		case JsonType.String:
			return (string)data;
		default:
			return null;
		}
	}

	public static Type GetSystemType(this JsonData data, bool castDoubleToFloat = false)
	{
		switch (data.GetJsonType())
		{
		case JsonType.Array:
			return typeof(IList<JsonData>);
		case JsonType.Boolean:
			return typeof(bool);
		case JsonType.Double:
			return (!castDoubleToFloat) ? typeof(double) : typeof(float);
		case JsonType.Int:
			return typeof(int);
		case JsonType.Long:
			return typeof(long);
		case JsonType.Object:
			return typeof(IDictionary<string, JsonData>);
		case JsonType.String:
			return typeof(string);
		default:
			return null;
		}
	}

	private static void DebugJson(IJsonWrapper obj, StringWriter sw, int indent)
	{
		sw.WriteLine("{0,14}", obj.GetJsonType());
		if (obj.IsArray)
		{
			foreach (object item in (IEnumerable)obj)
			{
				DebugJson((JsonData)item, sw, indent + 1);
			}
			return;
		}
		if (!obj.IsObject)
		{
			return;
		}
		foreach (DictionaryEntry item2 in (IDictionary)obj)
		{
			DebugJson((JsonData)item2.Value, sw, indent + 1);
		}
	}
}
