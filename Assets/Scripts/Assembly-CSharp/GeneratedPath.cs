using System.Collections.Generic;
using UnityEngine;

public class GeneratedPath : MonoBehaviour
{
	public List<PathBlock> _blocks = new List<PathBlock>();

	public int _pNumBlocks
	{
		get
		{
			return _blocks.Count;
		}
	}

	public PathBlock AddBlock(PathBlock block)
	{
		block._path = this;
		block.transform.parent = base.transform;
		block._index = _blocks.Count;
		if (_blocks.Count > 0)
		{
			PathBlock pathBlock = _blocks[_blocks.Count - 1];
			block.transform.rotation = pathBlock._pGlobalEndRotation * Quaternion.Inverse(block._pLocalStartRotation);
			block.transform.position = pathBlock._endWaypoint.CurrentPosition - (block._startWaypoint.CurrentPosition - block.transform.position);
		}
		else
		{
			block.transform.position = Vector3.zero;
		}
		_blocks.Add(block);
		return block;
	}

	public void RemoveBlock(int blockIndex)
	{
		_blocks[blockIndex]._index = -999;
		_blocks[blockIndex] = null;
	}

	public Vector3 GetPos(int blockIndex, float blockProgress)
	{
		return GetBlockByIndex(blockIndex).GetPos(blockProgress);
	}

	public Vector3 GetAngles(int blockIndex, float blockProgress, bool ignoreY = false)
	{
		return GetBlockByIndex(blockIndex).GetAngles(blockProgress, ignoreY);
	}

	public void IncDist(ref float blockProgress, ref int blockIndex, float distance)
	{
		float pLength = GetBlockByIndex(blockIndex)._pLength;
		float num = pLength * blockProgress;
		float num2 = Mathf.Clamp(distance, 0f - num, pLength - num);
		blockProgress += num2 / pLength;
		distance -= num2;
		if ((distance > 0f && blockIndex < _blocks.Count - 1) || (distance < 0f && blockIndex > 0))
		{
			blockProgress = ((!(distance > 0f)) ? 1 : 0);
			blockIndex = Mathf.Clamp(blockIndex + (int)Mathf.Sign(distance), 0, _blocks.Count);
			IncDist(ref blockProgress, ref blockIndex, distance);
		}
	}

	public PathBlock GetBlockByIndex(int blockIndex)
	{
		if (_blocks.Count == 0)
		{
			return null;
		}
		return _blocks[Mathf.Clamp(blockIndex, 0, _blocks.Count - 1)];
	}
}
