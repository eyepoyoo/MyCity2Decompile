using System;
using UnityEngine;

public class BuildingVisual : MonoBehaviour
{
	[Serializable]
	public class BuildingObjects
	{
		public GameObject[] _buildingObjects;

		public GameObject[] _underConstructionObjects;

		public bool _doUseExclusiveConstructionObjects;
	}

	public BuildingInstance _relatedBuilding;

	public BuildingObjects[] _buildingVisualsByLevel;

	[HideInInspector]
	public Bounds _objectBounds;

	public bool _doShowGizmos = true;

	public Color _gizmosColour;

	private void OnDrawGizmos()
	{
		if (_doShowGizmos && !(_objectBounds.size == Vector3.zero))
		{
			Gizmos.color = _gizmosColour;
			Gizmos.DrawWireCube(_objectBounds.center, _objectBounds.size);
		}
	}

	private void OnDestroy()
	{
		if (_relatedBuilding != null)
		{
			_relatedBuilding.SaveState();
			_relatedBuilding._relatedVisuals = null;
			_relatedBuilding = null;
		}
	}

	public virtual void refreshVisuals()
	{
		if (_buildingVisualsByLevel == null || _buildingVisualsByLevel.Length == 0)
		{
			return;
		}
		hideAllGameObjects();
		if (_relatedBuilding == null)
		{
			return;
		}
		int num = Mathf.Clamp(_relatedBuilding._buildingLevel, 0, _buildingVisualsByLevel.Length - 1);
		bool pIsUpgrading = _relatedBuilding._pIsUpgrading;
		BuildingObjects buildingObjects = _buildingVisualsByLevel[num];
		if (buildingObjects == null)
		{
			return;
		}
		if (buildingObjects._buildingObjects != null && buildingObjects._buildingObjects.Length != 0)
		{
			int i = 0;
			for (int num2 = buildingObjects._buildingObjects.Length; i < num2; i++)
			{
				buildingObjects._buildingObjects[i].SetActive(!pIsUpgrading || !buildingObjects._doUseExclusiveConstructionObjects);
			}
		}
		if (buildingObjects._underConstructionObjects != null && buildingObjects._underConstructionObjects.Length != 0)
		{
			int j = 0;
			for (int num3 = buildingObjects._underConstructionObjects.Length; j < num3; j++)
			{
				buildingObjects._underConstructionObjects[j].SetActive(pIsUpgrading);
			}
		}
	}

	public virtual string serialiseVisualData()
	{
		return string.Empty;
	}

	public virtual void setVisualDataFromString(string visualData)
	{
		refreshVisuals();
	}

	public virtual void Dispose()
	{
		_relatedBuilding = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected virtual void hideAllGameObjects()
	{
		if (_buildingVisualsByLevel == null || _buildingVisualsByLevel.Length == 0)
		{
			return;
		}
		int i = 0;
		for (int num = _buildingVisualsByLevel.Length; i < num; i++)
		{
			if (_buildingVisualsByLevel[i]._buildingObjects != null && _buildingVisualsByLevel[i]._buildingObjects.Length != 0)
			{
				int j = 0;
				for (int num2 = _buildingVisualsByLevel[i]._buildingObjects.Length; j < num2; j++)
				{
					_buildingVisualsByLevel[i]._buildingObjects[j].SetActive(false);
				}
			}
			if (_buildingVisualsByLevel[i]._underConstructionObjects != null && _buildingVisualsByLevel[i]._underConstructionObjects.Length != 0)
			{
				int k = 0;
				for (int num3 = _buildingVisualsByLevel[i]._underConstructionObjects.Length; k < num3; k++)
				{
					_buildingVisualsByLevel[i]._underConstructionObjects[k].SetActive(false);
				}
			}
		}
	}

	private void crateCollisionBox()
	{
		if (_buildingVisualsByLevel != null && _buildingVisualsByLevel.Length != 0)
		{
			GameObject gameObject = new GameObject("Collider");
			gameObject.transform.position = base.transform.position;
			gameObject.transform.parent = base.transform;
			gameObject.layer = base.gameObject.layer;
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			boxCollider.size = _objectBounds.size;
		}
	}

	private void calcualteCollisionBoxSize()
	{
		if (_buildingVisualsByLevel == null || _buildingVisualsByLevel.Length == 0)
		{
			return;
		}
		_objectBounds = new Bounds(base.transform.position, new Vector3(0.01f, 0.01f, 0.01f));
		int i = 0;
		for (int num = _buildingVisualsByLevel.Length; i < num; i++)
		{
			if (_buildingVisualsByLevel[i]._buildingObjects == null || _buildingVisualsByLevel[i]._buildingObjects.Length == 0)
			{
				continue;
			}
			int j = 0;
			for (int num2 = _buildingVisualsByLevel[i]._buildingObjects.Length; j < num2; j++)
			{
				if (_buildingVisualsByLevel[i]._buildingObjects[j] == null)
				{
					continue;
				}
				Renderer component = _buildingVisualsByLevel[i]._buildingObjects[j].GetComponent<Renderer>();
				if (!(component == null))
				{
					MeshCollider meshCollider = _buildingVisualsByLevel[i]._buildingObjects[j].GetComponent<MeshCollider>();
					if (meshCollider == null)
					{
						meshCollider = _buildingVisualsByLevel[i]._buildingObjects[j].AddComponent<MeshCollider>();
					}
					_objectBounds.Encapsulate(meshCollider.bounds);
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(meshCollider);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(meshCollider);
					}
				}
			}
		}
	}
}
