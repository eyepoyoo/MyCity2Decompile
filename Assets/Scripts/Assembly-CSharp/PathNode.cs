using System;
using UnityEngine;

[Serializable]
public class PathNode
{
	public Vector3 position = Vector3.zero;

	public int[] nodeLinks;

	public bool isHeadNode;

	public NodeLink[] nodeLinkData;

	public int atNodeBehaviour = 8;

	public float stopTimeMin;

	public float stopTimeMax;

	private PathNodeGroup _group;

	public bool _pShouldStopAtNode
	{
		get
		{
			return stopTimeMax != 0f;
		}
	}

	public Vector3 WorldPos
	{
		get
		{
			if (_group == null)
			{
				return position;
			}
			return _group.transform.TransformPoint(position);
		}
	}

	public void SetPathNodeGroup(PathNodeGroup groupRef)
	{
		_group = groupRef;
	}

	public int GetLinkCount()
	{
		if (nodeLinkData == null)
		{
			return 0;
		}
		return nodeLinkData.Length;
	}

	public PathNode GetLinkNode(int i)
	{
		if (i < 0 || i > nodeLinkData.Length - 1)
		{
			return null;
		}
		return _group.pathNodes[nodeLinkData[i].linkToNodeId];
	}

	public PathNode GetNextNode()
	{
		if (nodeLinkData == null || nodeLinkData.Length == 0)
		{
			return null;
		}
		return _group.pathNodes[nodeLinkData[0].linkToNodeId];
	}

	public PathNode GetRandomNextNode()
	{
		return GetRandomNextNode(true, null);
	}

	public PathNode GetRandomNextNode(bool allowBacklinks, PathNode previousNode)
	{
		if (nodeLinkData == null || nodeLinkData.Length == 0)
		{
			return null;
		}
		int num = UnityEngine.Random.Range(0, nodeLinkData.Length - 1);
		if (!allowBacklinks && previousNode != null && _group.pathNodes[nodeLinkData[num].linkToNodeId] == previousNode)
		{
			if (nodeLinkData.Length == 1)
			{
				return null;
			}
			for (int i = 0; i < nodeLinkData.Length; i++)
			{
				num++;
				if (num >= nodeLinkData.Length)
				{
					num = 0;
				}
				if (nodeLinkData[num] != null && _group.pathNodes[nodeLinkData[num].linkToNodeId] != null)
				{
					return _group.pathNodes[nodeLinkData[num].linkToNodeId];
				}
			}
			return null;
		}
		return _group.pathNodes[nodeLinkData[num].linkToNodeId];
	}

	public PathNode GetRandomNextNodeWithLink(bool allowBacklinks, PathNode previousNode, out NodeLink link)
	{
		link = null;
		if (nodeLinkData == null || nodeLinkData.Length == 0)
		{
			return null;
		}
		int num = UnityEngine.Random.Range(0, nodeLinkData.Length - 1);
		if (!allowBacklinks && previousNode != null && _group.pathNodes[nodeLinkData[num].linkToNodeId] == previousNode)
		{
			if (nodeLinkData.Length == 1)
			{
				return null;
			}
			for (int i = 0; i < nodeLinkData.Length; i++)
			{
				num++;
				if (num >= nodeLinkData.Length)
				{
					num = 0;
				}
				if (nodeLinkData[num] != null && _group.pathNodes[nodeLinkData[num].linkToNodeId] != null)
				{
					link = nodeLinkData[num];
					return _group.pathNodes[nodeLinkData[num].linkToNodeId];
				}
			}
			return null;
		}
		link = nodeLinkData[num];
		return _group.pathNodes[nodeLinkData[num].linkToNodeId];
	}

	public PathNode GetNodeAndLinkNearestToDirection(bool allowBacklinks, PathNode previousNode, Vector3 direction, out NodeLink link)
	{
		link = null;
		if (nodeLinkData == null || nodeLinkData.Length == 0)
		{
			return null;
		}
		int num = -1;
		float num2 = float.PositiveInfinity;
		for (int i = 0; i < nodeLinkData.Length; i++)
		{
			if (nodeLinkData[i] != null && _group.pathNodes[nodeLinkData[i].linkToNodeId] != null && (allowBacklinks || previousNode == null || _group.pathNodes[nodeLinkData[i].linkToNodeId] != previousNode))
			{
				Vector3 normalized = (_group.pathNodes[nodeLinkData[i].linkToNodeId].WorldPos - position).normalized;
				float num3 = Vector3.Angle(direction, normalized);
				if (num3 < num2)
				{
					num2 = num3;
					num = i;
				}
			}
		}
		if (num == -1)
		{
			return null;
		}
		link = nodeLinkData[num];
		return _group.pathNodes[nodeLinkData[num].linkToNodeId];
	}

	public float GetSqrDist(Vector3 pos)
	{
		return (WorldPos - pos).sqrMagnitude;
	}
}
