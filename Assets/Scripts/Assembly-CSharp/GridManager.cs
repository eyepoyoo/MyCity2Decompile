using UnityEngine;

public class GridManager : MonoBehaviour
{
	private static GridManager _instance;

	public Transform topLeft;

	public Transform bottomRight;

	public int _numColumns = 10;

	public int _numRows = 10;

	public float _gizmoSphereRadius = 0.2f;

	public bool _doShowGizmos;

	private Vector3[] _gridPositions;

	private float xSeg;

	private float ySeg;

	public static GridManager Instance
	{
		get
		{
			return _instance;
		}
	}

	private void setGridPositions()
	{
		if (topLeft == null || bottomRight == null)
		{
			return;
		}
		_gridPositions = new Vector3[_numColumns * _numRows];
		Vector3 zero = Vector3.zero;
		float num = bottomRight.position.x - topLeft.position.x;
		xSeg = num / ((float)_numColumns - 1f);
		float num2 = topLeft.position.z - bottomRight.position.z;
		ySeg = num2 / ((float)_numRows - 1f);
		for (int i = 0; i < _numColumns; i++)
		{
			for (int j = 0; j < _numRows; j++)
			{
				zero.x = topLeft.position.x + xSeg * (float)i;
				zero.z = bottomRight.position.z + ySeg * (float)j;
				_gridPositions[Mathf.Clamp(j * _numColumns + i, 0, _gridPositions.Length - 1)] = zero;
			}
		}
	}

	public bool Exists(int x, int z)
	{
		if (x < 0 || z < 0)
		{
			return false;
		}
		if (x >= _numColumns)
		{
			return false;
		}
		if (z >= _numRows)
		{
			return false;
		}
		return true;
	}

	public Vector3 getWorldPosition(Point pos)
	{
		return getWorldPosition(pos.x, pos.y);
	}

	public Vector3 getWorldPosition(int x, int y)
	{
		if (!checkGridInitialised())
		{
			return Vector3.zero;
		}
		int num = Mathf.Clamp(y * _numColumns + x, 0, _gridPositions.Length - 1);
		return _gridPositions[num];
	}

	public Point getNearestGridIndiciesToWorldPos(Vector3 worldPos)
	{
		if (!checkGridInitialised())
		{
			return default(Point);
		}
		return getPointOfGridPosNearest(worldPos);
	}

	public Vector3 getNearestGridPositionToWorldPos(Vector3 worldPos)
	{
		if (!checkGridInitialised())
		{
			return Vector3.zero;
		}
		return _gridPositions[getIndexOfGridPosNearest(worldPos)];
	}

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		if (_gridPositions == null || _gridPositions.Length == 0)
		{
			setGridPositions();
		}
	}

	private void OnDrawGizmos()
	{
		if (_doShowGizmos && !(topLeft == null) && !(bottomRight == null) && checkGridInitialised())
		{
			for (int i = 0; i < _gridPositions.Length; i++)
			{
				Gizmos.DrawSphere(_gridPositions[i], _gizmoSphereRadius);
			}
		}
	}

	private bool checkGridInitialised()
	{
		if (_gridPositions != null && _gridPositions.Length >= 0)
		{
			return true;
		}
		setGridPositions();
		return _gridPositions != null && _gridPositions.Length >= 0;
	}

	private Point getPointOfGridPosNearest(Vector3 worldPos)
	{
		if (!checkGridInitialised())
		{
			return default(Point);
		}
		return new Point
		{
			x = Mathf.Clamp(Mathf.RoundToInt((worldPos.x - _gridPositions[0].x) / xSeg), 0, _numColumns - 1),
			y = Mathf.Clamp(Mathf.RoundToInt((worldPos.z - _gridPositions[0].z) / ySeg), 0, _numRows - 1)
		};
	}

	private int getIndexOfGridPosNearest(Vector3 worldPos)
	{
		Point pointOfGridPosNearest = getPointOfGridPosNearest(worldPos);
		return Mathf.Clamp(pointOfGridPosNearest.y * _numColumns + pointOfGridPosNearest.x, 0, _gridPositions.Length - 1);
	}
}
