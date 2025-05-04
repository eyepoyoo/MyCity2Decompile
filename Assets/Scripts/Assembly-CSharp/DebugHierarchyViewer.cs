using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugHierarchyViewer : MonoBehaviour
{
	private const float _defaultDpi = 80f;

	private static DebugHierarchyViewer _instance;

	private bool _isEnabled;

	public float _activeToggleSize = 32f;

	public float _indentSize = 20f;

	private Vector2 _scrollPos = Vector2.zero;

	private List<WeakReference> _rootObjects;

	public static bool IsEnabled
	{
		get
		{
			return _instance != null && _instance._isEnabled;
		}
		set
		{
			SetEnabled(value);
		}
	}

	private float _dpi
	{
		get
		{
			return (Screen.dpi != 0f) ? Screen.dpi : 80f;
		}
	}

	private float _buttonSize
	{
		get
		{
			return 0.25f * _dpi;
		}
	}

	private void Awake()
	{
	}

	private void OnEnable()
	{
		if (_instance == null)
		{
			_instance = this;
		}
	}

	private void OnDisable()
	{
		if (_instance == this)
		{
			_instance = null;
		}
	}

	private void Start()
	{
	}

	private void OnGUI()
	{
		if (_isEnabled)
		{
			RenderGUI();
		}
	}

	private static int CompareNames(WeakReference obj1, WeakReference obj2)
	{
		return string.Compare((obj1.Target as GameObject).name, (obj2.Target as GameObject).name, true);
	}

	private void RefreshRootObjects()
	{
		_rootObjects = new List<WeakReference>();
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject gameObject in array)
		{
			if (gameObject.transform.parent == null)
			{
				_rootObjects.Add(new WeakReference(gameObject, false));
			}
		}
		_rootObjects.Sort(CompareNames);
	}

	private void RenderGUI()
	{
		GUILayout.BeginVertical("box");
		_scrollPos = GUILayout.BeginScrollView(_scrollPos);
		if (GUILayout.Button("Refresh", GUILayout.MinWidth(_buttonSize), GUILayout.Height(_buttonSize), GUILayout.ExpandWidth(false)))
		{
			RefreshRootObjects();
		}
		GUILayout.Space(20f);
		foreach (WeakReference rootObject in _rootObjects)
		{
			object target = rootObject.Target;
			GameObject gameObject = target as GameObject;
			if (gameObject != null)
			{
				RecursiveRenderGUI(0, gameObject);
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void RecursiveRenderGUI(int depth, GameObject obj)
	{
		GUILayout.BeginHorizontal("label");
		bool flag = GUILayout.Button(obj.activeInHierarchy ? "X" : ((!obj.activeSelf) ? string.Empty : "/"), GUILayout.MinWidth(_buttonSize), GUILayout.Height(_buttonSize), GUILayout.ExpandWidth(false));
		GUILayout.Space(_indentSize * (float)(depth + 1));
		bool flag2 = GUILayout.Button(((obj.hideFlags & HideFlags.NotEditable) == 0) ? (obj.name + " [...]") : obj.name, GUILayout.MinWidth(_buttonSize), GUILayout.Height(_buttonSize), GUILayout.ExpandWidth(false));
		GUILayout.EndHorizontal();
		if (flag)
		{
			obj.SetActive(!obj.activeSelf);
		}
		if (flag2)
		{
			if ((obj.hideFlags & HideFlags.NotEditable) != HideFlags.None)
			{
				obj.hideFlags &= ~HideFlags.NotEditable;
			}
			else
			{
				obj.hideFlags |= HideFlags.NotEditable;
			}
		}
		if ((obj.hideFlags & HideFlags.NotEditable) == 0)
		{
			return;
		}
		Behaviour[] components = obj.GetComponents<Behaviour>();
		if (components != null)
		{
			Behaviour[] array = components;
			foreach (Behaviour comp in array)
			{
				RenderComponentGUI(depth + 1, comp);
			}
		}
		foreach (Transform item in obj.transform)
		{
			RecursiveRenderGUI(depth + 1, item.gameObject);
		}
	}

	private void RenderComponentGUI(int depth, Behaviour comp)
	{
		GUILayout.BeginHorizontal("label");
		bool flag = GUILayout.Button((comp.gameObject.activeInHierarchy && comp.enabled) ? "X" : ((!comp.enabled) ? string.Empty : "/"), GUILayout.MinWidth(_buttonSize), GUILayout.Height(_buttonSize), GUILayout.ExpandWidth(false));
		GUILayout.Space(_indentSize * (float)(depth + 1));
		GUILayout.Label(comp.GetType().Name, GUILayout.ExpandWidth(false));
		GUILayout.EndHorizontal();
		if (flag)
		{
			comp.enabled = !comp.enabled;
		}
	}

	private static void SetEnabled(bool isWantEnabled)
	{
		if (isWantEnabled && _instance == null)
		{
			GameObject gameObject = new GameObject("hierarchy_viewer");
			if (gameObject != null)
			{
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				gameObject.AddComponent<DebugHierarchyViewer>();
			}
		}
		if (_instance != null && _instance._isEnabled != isWantEnabled)
		{
			_instance._isEnabled = isWantEnabled;
			if (_instance._isEnabled)
			{
				_instance.RefreshRootObjects();
			}
		}
	}
}
