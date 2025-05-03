using System;

namespace AmuzoEngine
{
	public class JobCounter
	{
		private int _count;

		private Action _onBegin;

		private Action _onEnd;

		public JobCounter(Action onBegin, Action onEnd)
		{
			_count = 0;
			_onBegin = onBegin;
			_onEnd = onEnd;
		}

		public JobCounter(Action onEnd)
		{
			_count = 0;
			_onBegin = null;
			_onEnd = onEnd;
		}

		public void BeginJob()
		{
			if (_count == 0 && _onBegin != null)
			{
				_onBegin();
			}
			_count++;
		}

		public void EndJob()
		{
			if (_count > 0)
			{
				_count--;
				if (_count == 0 && _onEnd != null)
				{
					_onEnd();
				}
			}
		}
	}
}
