using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogData : IEnumerable, IEnumerable<LogData.ILine>
{
	public interface ILine
	{
		string _pFullMessage { get; }

		string _pStackTrace { get; }

		LogType _pLogType { get; }

		string _pPartMessage { get; }
	}

	private class Line : ILine, IObjectPoolFFItem<Line>
	{
		public string _fullMessage;

		public string _stackTrace;

		public LogType _logType;

		public string _partMessage;

		public LinkedListNode<Line> _listNode;

		string ILine._pFullMessage
		{
			get
			{
				return _fullMessage;
			}
		}

		string ILine._pStackTrace
		{
			get
			{
				return _stackTrace;
			}
		}

		LogType ILine._pLogType
		{
			get
			{
				return _logType;
			}
		}

		string ILine._pPartMessage
		{
			get
			{
				return _partMessage;
			}
		}

		LinkedListNode<Line> IObjectPoolFFItem<Line>._pObjectPoolListNode
		{
			get
			{
				return _listNode;
			}
		}

		public Line()
		{
			_listNode = new LinkedListNode<Line>(this);
		}
	}

	public delegate bool DLineFilter(string message, string stackTrace, LogType logType);

	private const string LOG_TAG = "[LogData] ";

	private ObjectPoolFF<Line> _lines;

	private int _partMessageLength;

	private bool _isLoggingPaused;

	private DLineFilter _lineFilter;

	public bool _pIsLoggingPaused
	{
		get
		{
			return _isLoggingPaused;
		}
		set
		{
			_isLoggingPaused = value;
		}
	}

	public DLineFilter _pLineFilter
	{
		set
		{
			_lineFilter = value;
		}
	}

	private event Action<ILine> _onLineRemoved;

	public event Action<ILine> _pOnLineRemoved
	{
		add
		{
			this._onLineRemoved = (Action<ILine>)Delegate.Combine(this._onLineRemoved, value);
		}
		remove
		{
			this._onLineRemoved = (Action<ILine>)Delegate.Remove(this._onLineRemoved, value);
		}
	}

	public LogData(int poolSize, int partMessageLength)
	{
		_lines = new ObjectPoolFF<Line>(poolSize, () => new Line(), null);
		_partMessageLength = partMessageLength;
	}

	IEnumerator<ILine> IEnumerable<ILine>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Reset()
	{
		if (_lines == null)
		{
			return;
		}
		if (this._onLineRemoved != null)
		{
			foreach (Line activeObject in _lines.ActiveObjects)
			{
				this._onLineRemoved(activeObject);
			}
		}
		_lines.FreeAll();
	}

	public void OnLog(string message, string stackTrace, LogType logType)
	{
		if (!_isLoggingPaused && (_lineFilter == null || _lineFilter(message, stackTrace, logType)))
		{
			AddLine(message, stackTrace, logType);
		}
	}

	public void OnGUI()
	{
	}

	private Line GetNewLine()
	{
		Line line = _lines.Allocate();
		if (line == null)
		{
			_lines.FreeOldest();
			line = _lines.Allocate();
			if (line != null && this._onLineRemoved != null)
			{
				this._onLineRemoved(line);
			}
		}
		return line;
	}

	private void AddLine(string message, string stackTrace, LogType logType)
	{
		if (_lines == null)
		{
			return;
		}
		Line newLine = GetNewLine();
		if (newLine != null)
		{
			newLine._fullMessage = message;
			newLine._stackTrace = stackTrace;
			newLine._logType = logType;
			if (message.Length <= _partMessageLength)
			{
				newLine._partMessage = message;
			}
			else
			{
				newLine._partMessage = message.Substring(0, _partMessageLength);
			}
		}
	}

	private IEnumerator<ILine> GetEnumerator()
	{
		foreach (Line active in _lines.ActiveList)
		{
			yield return active;
		}
	}
}
