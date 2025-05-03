using UnityEngine;

public class BuildingVisualGridEntity : BuildingVisual
{
	private const char SAVE_VARIABLE_DELIMITER = ',';

	public Point[] _footprintOffset;

	public Point _gridPoint;

	private Point[] _footprint;

	public override void refreshVisuals()
	{
		base.refreshVisuals();
		setPosition(_gridPoint);
	}

	public override string serialiseVisualData()
	{
		return base.serialiseVisualData() + _gridPoint.x + ',' + _gridPoint.y;
	}

	public override void setVisualDataFromString(string visualData)
	{
		string[] array = visualData.Split(',');
		if (array.Length == 2)
		{
			int result = 0;
			int result2 = 0;
			int.TryParse(array[0], out result);
			int.TryParse(array[1], out result2);
			setPosition(new Point(result, result2));
			base.setVisualDataFromString(visualData);
		}
	}

	public override void Dispose()
	{
		_relatedBuilding = null;
		if (BuildingGridPlacementManager.Instance != null)
		{
			BuildingGridPlacementManager.Instance.destroyVisual(this);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void setPosition(Point gridPoint)
	{
		_gridPoint = gridPoint;
		base.transform.position = GridManager.Instance.getWorldPosition(gridPoint);
		if (_footprintOffset != null && _footprintOffset.Length != 0)
		{
			_footprint = new Point[_footprintOffset.Length];
			for (int i = 0; i < _footprintOffset.Length; i++)
			{
				_footprint[i] = _gridPoint + _footprintOffset[i];
			}
		}
	}

	public bool isAtPosition(Point gridPoint)
	{
		if (_footprintOffset != null && _footprintOffset.Length != 0)
		{
			for (int i = 0; i < _footprintOffset.Length; i++)
			{
				if (!(_footprintOffset[i] + _gridPoint != gridPoint))
				{
					return true;
				}
			}
		}
		return gridPoint == _gridPoint;
	}
}
