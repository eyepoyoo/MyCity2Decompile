using System.Collections.Generic;
using UnityEngine;

namespace AmuzoPhysics
{
	public class Gjk2dSolver
	{
		private class MinkVertex
		{
			public Vector2 _pos;

			public int _indexA;

			public int _indexB;

			public float _weight;
		}

		private class Result
		{
			public List<int> _vertexIndicesA;

			public List<float> _vertexWeightsA;

			public List<int> _vertexIndicesB;

			public List<float> _vertexWeightsB;

			public Vector2 _pointA;

			public Vector2 _pointB;
		}

		private List<Vector2> _verticesA;

		private List<Vector2> _verticesB;

		private LinkedList<MinkVertex> _minkVertices;

		private LinkedList<MinkVertex> _cleanVertices;

		private LinkedList<MinkVertex> _dirtyVertices;

		private LinkedList<MinkVertex> _simplexVertices;

		private Result _result;

		public Vector2 ResultPointA
		{
			get
			{
				return _result._pointA;
			}
		}

		public Vector2 ResultPointB
		{
			get
			{
				return _result._pointB;
			}
		}

		public Gjk2dSolver()
		{
			_verticesA = new List<Vector2>();
			_verticesB = new List<Vector2>();
			_minkVertices = new LinkedList<MinkVertex>();
			_cleanVertices = new LinkedList<MinkVertex>();
			_dirtyVertices = new LinkedList<MinkVertex>();
			_simplexVertices = new LinkedList<MinkVertex>();
			_result = new Result();
			_result._vertexIndicesA = new List<int>(3);
			_result._vertexWeightsA = new List<float>(3);
			_result._vertexIndicesB = new List<int>(3);
			_result._vertexWeightsB = new List<float>(3);
		}

		public void Reset()
		{
			_verticesA.Clear();
			_verticesB.Clear();
		}

		public void AddVertexA(Vector2 vert)
		{
			_verticesA.Add(vert);
		}

		public void AddVertexB(Vector2 vert)
		{
			_verticesB.Add(vert);
		}

		public void Solve()
		{
			PreSolve();
			do
			{
				if (_simplexVertices.Count == 1)
				{
					ClosestPointToOrigin1();
					continue;
				}
				if (_simplexVertices.Count == 2)
				{
					ClosestPointToOrigin2();
					continue;
				}
				if (_simplexVertices.Count == 3)
				{
					ClosestPointToOrigin3();
					continue;
				}
				Debug.LogWarning("[Gjk2d] Bad simplex vertex count: " + _simplexVertices.Count);
				break;
			}
			while (UpdateSimplex());
			ComputeResult();
		}

		private void PreSolve()
		{
			while (_cleanVertices.First != null)
			{
				LinkedListNode<MinkVertex> first = _cleanVertices.First;
				_cleanVertices.RemoveFirst();
				_minkVertices.AddLast(first);
			}
			while (_dirtyVertices.First != null)
			{
				LinkedListNode<MinkVertex> first = _dirtyVertices.First;
				_dirtyVertices.RemoveFirst();
				_minkVertices.AddLast(first);
			}
			while (_simplexVertices.First != null)
			{
				LinkedListNode<MinkVertex> first = _simplexVertices.First;
				_simplexVertices.RemoveFirst();
				_minkVertices.AddLast(first);
			}
			int count = _verticesA.Count;
			int count2 = _verticesB.Count;
			int num = count * count2;
			while (_minkVertices.Count < num)
			{
				_minkVertices.AddLast(new MinkVertex());
			}
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < count2; j++)
				{
					LinkedListNode<MinkVertex> first = _minkVertices.First;
					_minkVertices.RemoveFirst();
					if (_simplexVertices.Count < 3)
					{
						_simplexVertices.AddLast(first);
					}
					else
					{
						_cleanVertices.AddLast(first);
					}
					first.Value._pos = _verticesA[i] - _verticesB[j];
					first.Value._indexA = i;
					first.Value._indexB = j;
					first.Value._weight = 0f;
				}
			}
			_result._vertexIndicesA.Clear();
			_result._vertexWeightsA.Clear();
			_result._vertexIndicesB.Clear();
			_result._vertexWeightsB.Clear();
			_result._pointA = Vector2.zero;
			_result._pointB = Vector2.zero;
		}

		private bool UpdateSimplex()
		{
			Vector2 zero = Vector2.zero;
			LinkedListNode<MinkVertex> linkedListNode = _simplexVertices.First;
			while (linkedListNode != null)
			{
				LinkedListNode<MinkVertex> next = linkedListNode.Next;
				if (linkedListNode.Value._weight > 0f)
				{
					zero += linkedListNode.Value._weight * linkedListNode.Value._pos;
				}
				else
				{
					_simplexVertices.Remove(linkedListNode);
					_dirtyVertices.AddLast(linkedListNode);
				}
				linkedListNode = next;
			}
			if (_simplexVertices.Count < 3)
			{
				float num = Vector2.Dot(zero, zero);
				linkedListNode = _cleanVertices.First;
				LinkedListNode<MinkVertex> next = null;
				while (linkedListNode != null)
				{
					float num2 = Vector2.Dot(linkedListNode.Value._pos, zero);
					if (num2 < num)
					{
						num = num2;
						next = linkedListNode;
					}
					linkedListNode = linkedListNode.Next;
				}
				if (next != null)
				{
					_cleanVertices.Remove(next);
					_simplexVertices.AddLast(next);
					return true;
				}
			}
			return false;
		}

		private void ComputeResult()
		{
			for (LinkedListNode<MinkVertex> linkedListNode = _simplexVertices.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				int num = _result._vertexIndicesA.IndexOf(linkedListNode.Value._indexA);
				if (num < 0)
				{
					_result._vertexIndicesA.Add(linkedListNode.Value._indexA);
					_result._vertexWeightsA.Add(linkedListNode.Value._weight);
				}
				else
				{
					List<float> vertexWeightsA;
					List<float> list = (vertexWeightsA = _result._vertexWeightsA);
					int index2;
					int index = (index2 = num);
					float num2 = vertexWeightsA[index2];
					list[index] = num2 + linkedListNode.Value._weight;
				}
				num = _result._vertexIndicesB.IndexOf(linkedListNode.Value._indexB);
				if (num < 0)
				{
					_result._vertexIndicesB.Add(linkedListNode.Value._indexB);
					_result._vertexWeightsB.Add(linkedListNode.Value._weight);
				}
				else
				{
					List<float> vertexWeightsB;
					List<float> list2 = (vertexWeightsB = _result._vertexWeightsB);
					int index2;
					int index3 = (index2 = num);
					float num2 = vertexWeightsB[index2];
					list2[index3] = num2 + linkedListNode.Value._weight;
				}
			}
			for (int i = 0; i < _result._vertexIndicesA.Count; i++)
			{
				_result._pointA += _result._vertexWeightsA[i] * _verticesA[_result._vertexIndicesA[i]];
			}
			for (int j = 0; j < _result._vertexIndicesB.Count; j++)
			{
				_result._pointB += _result._vertexWeightsB[j] * _verticesB[_result._vertexIndicesB[j]];
			}
		}

		private void ClosestPointToOrigin1()
		{
			_simplexVertices.First.Value._weight = 1f;
		}

		private void ClosestPointToOrigin2()
		{
			LinkedListNode<MinkVertex> first = _simplexVertices.First;
			LinkedListNode<MinkVertex> next = first.Next;
			Vector2 pos = first.Value._pos;
			Vector2 pos2 = next.Value._pos;
			float num = 0f - Vector2.Dot(pos2, pos - pos2);
			float num2 = 0f - Vector2.Dot(pos, pos2 - pos);
			float num3 = 0f;
			float weight = 0f;
			if (num > 0f && num2 > 0f)
			{
				num3 = num / (num + num2);
				weight = 1f - num3;
			}
			else if (num2 <= 0f)
			{
				num3 = 1f;
				weight = 0f;
			}
			else if (num <= 0f)
			{
				num3 = 0f;
				weight = 1f;
			}
			else
			{
				Debug.LogError("[Gjk2d] This shouldn't happen!");
			}
			first.Value._weight = num3;
			next.Value._weight = weight;
		}

		private void ClosestPointToOrigin3()
		{
			LinkedListNode<MinkVertex> first = _simplexVertices.First;
			LinkedListNode<MinkVertex> next = first.Next;
			LinkedListNode<MinkVertex> next2 = next.Next;
			Vector2 pos = first.Value._pos;
			Vector2 pos2 = next.Value._pos;
			Vector2 pos3 = next2.Value._pos;
			float num = Vector2.Dot(pos, pos);
			float num2 = Vector2.Dot(pos, pos2);
			float num3 = Vector2.Dot(pos, pos3);
			float num4 = num2;
			float num5 = Vector2.Dot(pos2, pos2);
			float num6 = Vector2.Dot(pos2, pos3);
			float num7 = num3;
			float num8 = num6;
			float num9 = Vector2.Dot(pos3, pos3);
			float num10 = 0f - num4 + num5;
			float num11 = 0f - num2 + num;
			float num12 = 0f - num8 + num9;
			float num13 = 0f - num6 + num5;
			float num14 = 0f - num3 + num;
			float num15 = 0f - num7 + num9;
			float num16 = (0f - num2 + num5) * (0f - num3 + num9) - (0f - num3 + num6) * (0f - num2 + num8);
			float num17 = (0f - num6 + num9) * (0f - num4 + num) - (0f - num4 + num7) * (0f - num6 + num3);
			float num18 = (0f - num7 + num) * (0f - num8 + num5) - (0f - num8 + num2) * (0f - num7 + num4);
			float num19 = 0f;
			float num20 = 0f;
			float num21 = 0f;
			if (num16 > 0f && num17 > 0f && num18 > 0f)
			{
				num18 += num16 + num17;
				num19 = num16 / num18;
				num20 = num17 / num18;
				num21 = 1f - num19 - num20;
			}
			else if (num10 > 0f && num11 > 0f && num18 <= 0f)
			{
				num19 = num10 / (num10 + num11);
				num20 = 1f - num19;
				num21 = 0f;
			}
			else if (num12 > 0f && num13 > 0f && num16 <= 0f)
			{
				num20 = num12 / (num12 + num13);
				num21 = 1f - num20;
				num19 = 0f;
			}
			else if (num14 > 0f && num15 > 0f && num17 <= 0f)
			{
				num21 = num14 / (num14 + num15);
				num19 = 1f - num21;
				num20 = 0f;
			}
			else if (num11 <= 0f && num14 <= 0f)
			{
				num19 = 1f;
				num20 = 0f;
				num21 = 0f;
			}
			else if (num13 <= 0f && num10 <= 0f)
			{
				num19 = 0f;
				num20 = 1f;
				num21 = 0f;
			}
			else if (num15 <= 0f && num12 <= 0f)
			{
				num19 = 0f;
				num20 = 0f;
				num21 = 1f;
			}
			else
			{
				Debug.LogError("[Gjk2d] This shouldn't happen!");
			}
			first.Value._weight = num19;
			next.Value._weight = num20;
			next2.Value._weight = num21;
		}

		private static Vector2 ClosestPoint2(Vector2 a, Vector2 b, Vector2 p)
		{
			Vector2 zero = Vector2.zero;
			float num = Vector2.Dot(p - b, a - b);
			float num2 = Vector2.Dot(p - a, b - a);
			if (num > 0f && num2 > 0f)
			{
				zero.x = num / (num + num2);
				zero.y = 1f - zero.x;
			}
			else if (num2 <= 0f)
			{
				zero.x = 1f;
				zero.y = 0f;
			}
			else if (num <= 0f)
			{
				zero.x = 0f;
				zero.y = 1f;
			}
			else
			{
				Debug.LogError("[Gjk2d] This shouldn't happen!");
			}
			return zero;
		}

		private static Vector3 ClosestPoint3(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
		{
			Vector3 zero = Vector3.zero;
			float num = Vector2.Dot(p, a);
			float num2 = Vector2.Dot(p, b);
			float num3 = Vector2.Dot(p, c);
			float num4 = num;
			float num5 = num2;
			float num6 = num3;
			float num7 = Vector2.Dot(a, a);
			float num8 = Vector2.Dot(a, b);
			float num9 = Vector2.Dot(a, c);
			float num10 = num8;
			float num11 = Vector2.Dot(b, b);
			float num12 = Vector2.Dot(b, c);
			float num13 = num9;
			float num14 = num12;
			float num15 = Vector2.Dot(c, c);
			float num16 = num - num2 - num10 + num11;
			float num17 = num2 - num - num8 + num7;
			float num18 = num2 - num3 - num14 + num15;
			float num19 = num3 - num2 - num12 + num11;
			float num20 = num3 - num - num9 + num7;
			float num21 = num - num3 - num13 + num15;
			float num22 = (num4 - num8 - num5 + num11) * (num4 - num9 - num6 + num15) - (num4 - num9 - num5 + num12) * (num4 - num8 - num6 + num14);
			float num23 = (num5 - num12 - num6 + num15) * (num5 - num10 - num4 + num7) - (num5 - num10 - num6 + num13) * (num5 - num12 - num4 + num9);
			float num24 = (num6 - num13 - num4 + num7) * (num6 - num14 - num5 + num11) - (num6 - num14 - num4 + num8) * (num6 - num13 - num5 + num10);
			if (num22 > 0f && num23 > 0f && num24 > 0f)
			{
				num24 += num22 + num23;
				zero.x = num22 / num24;
				zero.y = num23 / num24;
				zero.z = 1f - zero.x - zero.y;
			}
			else if (num16 > 0f && num17 > 0f && num24 <= 0f)
			{
				zero.x = num16 / (num16 + num17);
				zero.y = 1f - zero.x;
				zero.z = 0f;
			}
			else if (num18 > 0f && num19 > 0f && num22 <= 0f)
			{
				zero.y = num18 / (num18 + num19);
				zero.z = 1f - zero.y;
				zero.x = 0f;
			}
			else if (num20 > 0f && num21 > 0f && num23 <= 0f)
			{
				zero.z = num20 / (num20 + num21);
				zero.x = 1f - zero.z;
				zero.y = 0f;
			}
			else if (num17 <= 0f && num20 <= 0f)
			{
				zero.x = 1f;
				zero.y = 0f;
				zero.z = 0f;
			}
			else if (num19 <= 0f && num16 <= 0f)
			{
				zero.x = 0f;
				zero.y = 1f;
				zero.z = 0f;
			}
			else if (num21 <= 0f && num18 <= 0f)
			{
				zero.x = 0f;
				zero.y = 0f;
				zero.z = 1f;
			}
			else
			{
				Debug.LogError("[Gjk2d] This shouldn't happen!");
			}
			return zero;
		}
	}
}
