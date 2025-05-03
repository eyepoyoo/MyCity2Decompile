using System;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
	public bool _rotateToCurve = true;

	public bool fixedAngleX;

	[NonSerialized]
	public int _blockIndex;

	[NonSerialized]
	public float _blockProgress;

	private GeneratedPath _path;

	public virtual GeneratedPath _pPath
	{
		get
		{
			return _path;
		}
		set
		{
			_path = value;
			UpdatePos();
		}
	}

	public virtual void Move(float distance)
	{
		_pPath.IncDist(ref _blockProgress, ref _blockIndex, distance);
		UpdatePos();
	}

	public void SetPos(int blockIndex, float blockProgress)
	{
		_blockIndex = blockIndex;
		_blockProgress = blockProgress;
		UpdatePos();
	}

	public void SetPos(float pathProgress)
	{
		SetPos((int)pathProgress, pathProgress % 1f);
	}

	private void UpdatePos()
	{
		base.transform.position = _pPath.GetPos(_blockIndex, _blockProgress);
		if (_rotateToCurve)
		{
			base.transform.eulerAngles = _pPath.GetAngles(_blockIndex, _blockProgress, fixedAngleX);
		}
	}
}
