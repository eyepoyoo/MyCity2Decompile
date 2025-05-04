using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugProfiler
{
	private class Section
	{
		public string _label;

		public float _startTime;

		public float _duration;

		public Dictionary<string, int> _intData = new Dictionary<string, int>();

		public Dictionary<string, float> _floatData = new Dictionary<string, float>();

		public float _durationAcc;

		public int _numIters;

		public Section _parent;

		public Section _firstChild;

		public Section _nextSibling;

		public Section _nextChild
		{
			set
			{
				AddChild(value);
			}
		}

		public string _fullLabel
		{
			get
			{
				return (_parent == null) ? _label : (_parent._fullLabel + "/" + _label);
			}
		}

		public void Init(string label, Section parent)
		{
			_label = label;
			_startTime = 0f;
			_duration = 0f;
			_intData.Clear();
			_floatData.Clear();
			_durationAcc = 0f;
			_numIters = 0;
			_parent = parent;
			if (parent != null)
			{
				parent._nextChild = this;
			}
			_firstChild = null;
			_nextSibling = null;
		}

		public void OnBegin()
		{
			_startTime = Time.realtimeSinceStartup;
			_duration = 0f;
			_intData.Clear();
			_floatData.Clear();
		}

		public void OnEnd()
		{
			_duration = Time.realtimeSinceStartup - _startTime;
			_durationAcc += _duration;
			_numIters++;
		}

		public void SetInt(string data, int value)
		{
			_intData[data] = value;
		}

		public void Increment(string data)
		{
			if (_intData.ContainsKey(data))
			{
				Dictionary<string, int> intData;
				Dictionary<string, int> dictionary = (intData = _intData);
				string key2;
				string key = (key2 = data);
				int num = intData[key2];
				dictionary[key] = num + 1;
			}
			else
			{
				_intData[data] = 1;
			}
		}

		private void AddChild(Section newChild)
		{
			newChild._parent = this;
			newChild._firstChild = null;
			newChild._nextSibling = null;
			if (_firstChild == null)
			{
				_firstChild = newChild;
				return;
			}
			Section section = _firstChild;
			while (section._nextSibling != null)
			{
				section = section._nextSibling;
			}
			section._nextSibling = newChild;
		}
	}

	private static GUIStyle _defaultGuiStyle;

	private bool _isShowAccumulated;

	private Section _rootSection;

	private Section _currSection;

	private ObjectPool<Section> _sectionPool;

	private StringBuilder _outputString;

	public bool IsShowAccumulated
	{
		get
		{
			return _isShowAccumulated;
		}
		set
		{
			_isShowAccumulated = value;
		}
	}

	public DebugProfiler(int maxSections)
	{
		_sectionPool = new ObjectPool<Section>(maxSections, () => new Section(), null);
		_outputString = new StringBuilder();
	}

	static DebugProfiler()
	{
		_defaultGuiStyle = new GUIStyle();
		if (_defaultGuiStyle != null)
		{
			_defaultGuiStyle.normal.textColor = Color.white;
			_defaultGuiStyle.richText = false;
		}
	}

	public void RenderGui(Rect rect, GUIStyle style = null)
	{
		string text = Dump();
		GUI.Box(rect, text, (style == null) ? _defaultGuiStyle : style);
	}

	public void Reset()
	{
		_sectionPool.FreeAll();
		_rootSection = null;
		_currSection = null;
		BeginSection("root");
	}

	public void FinalizeFrame()
	{
		if (_currSection != null)
		{
			if (_currSection == _rootSection)
			{
				EndSection();
			}
			else
			{
				Debug.LogWarning("[DebugProfiler] Finalize error");
			}
		}
	}

	private Section _FindSection(string label)
	{
		Section section = null;
		if (_currSection != null)
		{
			section = _currSection._firstChild;
			while (section != null && section._label != label)
			{
				section = section._nextSibling;
			}
		}
		return section;
	}

	private Section _NewSection(string label)
	{
		Section section = _sectionPool.Allocate();
		if (section != null)
		{
			section.Init(label, _currSection);
			if (_rootSection == null)
			{
				_rootSection = section;
			}
		}
		return section;
	}

	public void BeginSection(string label)
	{
		Section section = _FindSection(label);
		if (section == null)
		{
			section = _NewSection(label);
		}
		if (section != null)
		{
			section.OnBegin();
			_currSection = section;
		}
	}

	public void EndSection(bool isDumpToConsole = false)
	{
		if (_currSection == null)
		{
			return;
		}
		_currSection.OnEnd();
		if (isDumpToConsole)
		{
			_DumpSection(_currSection, 0, delegate(string text)
			{
				Debug.Log(text);
			});
		}
		_currSection = _currSection._parent;
	}

	public void SetInt(string data, int value)
	{
		if (_currSection != null)
		{
			_currSection.SetInt(data, value);
		}
	}

	public void Increment(string data)
	{
		if (_currSection != null)
		{
			_currSection.Increment(data);
		}
	}

	public string Dump()
	{
		_outputString.Length = 0;
		if (_rootSection != null)
		{
			_DumpSection(_rootSection, 0, _DumpTextToOutputString);
		}
		return _outputString.ToString();
	}

	private void _DumpTextToOutputString(string text)
	{
		_outputString.AppendLine(text);
	}

	private void _DumpSection(Section section, int depth, Action<string> fnDumpText)
	{
		string text = new string('\t', depth);
		float num = ((!_isShowAccumulated) ? section._duration : section._durationAcc);
		fnDumpText(text + section._label + ":" + (num * 1000f).ToString("F2") + "ms");
		foreach (string key in section._intData.Keys)
		{
			fnDumpText(text + "\t" + key + ":" + section._intData[key]);
		}
		foreach (string key2 in section._floatData.Keys)
		{
			fnDumpText(text + "\t" + key2 + ":" + section._floatData[key2]);
		}
		for (Section section2 = section._firstChild; section2 != null; section2 = section2._nextSibling)
		{
			_DumpSection(section2, depth + 1, fnDumpText);
		}
	}
}
