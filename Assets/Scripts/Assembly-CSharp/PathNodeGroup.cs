using UnityEngine;

public class PathNodeGroup : MonoBehaviour
{
	public PathNode[] pathNodes;

	private int _headNodeIndex;

	private void Awake()
	{
		if (pathNodes == null)
		{
			return;
		}
		for (int i = 0; i < pathNodes.Length; i++)
		{
			pathNodes[i].SetPathNodeGroup(this);
			if (pathNodes[i].isHeadNode)
			{
				_headNodeIndex = i;
			}
		}
	}

	public PathNode GetHeadNode()
	{
		return pathNodes[_headNodeIndex];
	}

	public PathNode GetNearestNode(Vector3 pos)
	{
		if (pathNodes == null || pathNodes.Length == 0)
		{
			return null;
		}
		float num = float.PositiveInfinity;
		int num2 = 0;
		for (int i = 0; i < pathNodes.Length; i++)
		{
			float sqrMagnitude = (pos - pathNodes[i].WorldPos).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				num2 = i;
			}
		}
		return pathNodes[num2];
	}
}
