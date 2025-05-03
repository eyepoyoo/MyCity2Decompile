using UnityEngine;

[RequireComponent(typeof(BezierCurveManager))]
public class StudStreak : MonoBehaviour
{
	public GameObject _prefab;

	public int _num = 5;

	public GameObject[] _objects;

	private int _prevNum = -1;

	private Vector3[] _controlPointPositions;

	private BezierCurveManager _bezierCurveManager;

	private BezierCurveManager _pBezierCurveManager
	{
		get
		{
			return _bezierCurveManager ?? (_bezierCurveManager = GetComponent<BezierCurveManager>());
		}
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (_prevNum == -1)
		{
			_prevNum = _num;
			_controlPointPositions = GetControlPointPositions();
		}
		if (_num != _prevNum)
		{
			Refresh();
			_prevNum = _num;
		}
		Vector3[] controlPointPositions = GetControlPointPositions();
		if (controlPointPositions.Length != _controlPointPositions.Length)
		{
			Refresh();
			return;
		}
		for (int i = 0; i < _controlPointPositions.Length; i++)
		{
			if (_controlPointPositions[i] != controlPointPositions[i])
			{
				Refresh();
				break;
			}
		}
	}

	public void Refresh()
	{
		if (_objects != null)
		{
			for (int num = _objects.Length - 1; num >= 0; num--)
			{
				if ((bool)_objects[num] && (bool)_objects[num].gameObject)
				{
					Object.DestroyImmediate(_objects[num].gameObject);
				}
			}
		}
		_objects = new GameObject[_num];
		if ((bool)_prefab)
		{
			for (int i = 0; i < _num; i++)
			{
				_objects[i] = Object.Instantiate(_prefab);
				_objects[i].transform.parent = base.transform;
			}
		}
		PositionStuds();
	}

	private void PositionStuds()
	{
		MyBezier myBezier = _pBezierCurveManager.CreateGizmoBezier(true);
		int num = ((!_pBezierCurveManager.IsFullLoop) ? _objects.Length : (_objects.Length + 1));
		for (int i = 0; i < _objects.Length; i++)
		{
			_objects[i].transform.position = myBezier.GetPositionAtDistance(Mathf.Lerp(0f, myBezier.MaxDistance - 0.1f, (float)i / (float)(num - 1)), _pBezierCurveManager.IsFullLoop) + Vector3.up * 1f;
		}
		_controlPointPositions = GetControlPointPositions();
	}

	private Vector3[] GetControlPointPositions()
	{
		BezierControlPoint[] componentsInChildren = GetComponentsInChildren<BezierControlPoint>();
		Vector3[] array = new Vector3[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			array[i] = componentsInChildren[i].transform.position;
		}
		return array;
	}
}
