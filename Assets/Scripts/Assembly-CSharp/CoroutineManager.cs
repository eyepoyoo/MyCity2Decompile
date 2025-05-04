using System;
using System.Collections;
using System.Collections.Generic;

public class CoroutineManager
{
	private readonly List<IEnumerator> _enumerators = new List<IEnumerator>();

	private readonly List<Action> _onComplete = new List<Action>();

	private int _currIndex = -1;

	public void AddCoroutine(IEnumerator enumerator, Action onComplete = null, bool waitForOthersToFinish = false)
	{
		if (waitForOthersToFinish)
		{
			_enumerators.Insert(0, enumerator);
			_onComplete.Insert(0, onComplete);
		}
		else
		{
			_enumerators.Add(enumerator);
			_onComplete.Add(onComplete);
		}
		_currIndex++;
	}

	public void Update()
	{
		if (_currIndex != -1 && !_enumerators[_currIndex].MoveNext())
		{
			Action action = _onComplete[_currIndex];
			_enumerators.RemoveAt(_currIndex);
			_onComplete.RemoveAt(_currIndex);
			_currIndex--;
			if (action != null)
			{
				action();
			}
		}
	}
}
