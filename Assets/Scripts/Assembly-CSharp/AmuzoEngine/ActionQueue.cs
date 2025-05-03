using System;
using System.Collections.Generic;

namespace AmuzoEngine
{
	public class ActionQueue
	{
		private class Node
		{
			public Action _action;

			public string _name;

			public Node(Action action)
			{
				_action = action;
				_name = string.Empty;
			}

			public Node(Action action, string name)
			{
				_action = action;
				_name = name;
			}
		}

		private const string LOG_TAG = "[ActionQueue] ";

		private string _name;

		private List<Node> _nodes;

		private int _nextNode;

		private int _numBatches;

		private string _pLogTag
		{
			get
			{
				return "[ActionQueue:" + _name + "] ";
			}
		}

		private bool _pIsNextAction
		{
			get
			{
				return _nextNode < _nodes.Count;
			}
		}

		public bool _pIsEmpty
		{
			get
			{
				return _nodes == null || _nextNode == _nodes.Count;
			}
		}

		public int _pTotalCount
		{
			get
			{
				return (_nodes != null) ? _nodes.Count : 0;
			}
		}

		public int _pDoneCount
		{
			get
			{
				return _nextNode;
			}
		}

		public int _pBatchCount
		{
			get
			{
				return _numBatches;
			}
		}

		public ActionQueue()
		{
			_name = string.Empty;
			_nodes = new List<Node>();
			_nextNode = 0;
			_numBatches = 0;
		}

		public ActionQueue(string name)
		{
			_name = name;
			_nodes = new List<Node>();
			_nextNode = 0;
			_numBatches = 0;
		}

		public void AddAction(Action action)
		{
			if (_nodes != null && action != null)
			{
				_nodes.Add(new Node(action));
			}
		}

		public void AddAction(Action action, string name)
		{
			if (_nodes != null && action != null)
			{
				_nodes.Add(new Node(action, name));
			}
		}

		public void DoActions(Func<bool> isNextActionAllowed = null)
		{
			if (_nodes == null)
			{
				return;
			}
			if (isNextActionAllowed == null)
			{
				isNextActionAllowed = () => true;
			}
			while (_pIsNextAction && isNextActionAllowed())
			{
				DoNextAction();
			}
			_numBatches++;
		}

		public void DoActions(int maxActions)
		{
			int actionCounter = 0;
			DoActions(() => actionCounter++ < maxActions);
		}

		public void DoActions(float maxSeconds, Func<float> getSystemSeconds)
		{
			float actionMaxTime = 0.01f;
			float actionStartTime = getSystemSeconds();
			DoActions(() => getSystemSeconds() - actionStartTime < actionMaxTime);
		}

		public void Restart()
		{
			_nextNode = 0;
			_numBatches = 0;
		}

		public void Clear()
		{
			_nodes = new List<Node>();
			_nextNode = 0;
			_numBatches = 0;
		}

		private void DoNextAction()
		{
			_nodes[_nextNode++]._action();
		}
	}
}
