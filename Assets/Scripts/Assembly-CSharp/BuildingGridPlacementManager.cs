using System;
using System.Collections.Generic;
using GameDefines;
using UnityEngine;

public class BuildingGridPlacementManager : MonoBehaviour
{
	[Serializable]
	public class BuildingTypeToPrefab
	{
		public BuildingDefines.EBuildingType _buildingType;

		public GameObject _prefab;
	}

	private static BuildingGridPlacementManager _instance;

	public Camera _camForRay;

	public LayerMask _layerMaskForRay;

	public BuildingTypeToPrefab[] _buildings;

	private List<BuildingVisualGridEntity> _activeGridBuildings = new List<BuildingVisualGridEntity>();

	public static BuildingGridPlacementManager Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
		BuildingManager._onBuildingLoaded = (Action<BuildingInstance>)Delegate.Combine(BuildingManager._onBuildingLoaded, new Action<BuildingInstance>(loadBuilding));
		if (!(BuildingManager.Instance == null) && BuildingManager.Instance._pHasInitialised)
		{
			List<BuildingInstance> allBuildingInstances = BuildingManager.Instance.getAllBuildingInstances();
			int i = 0;
			for (int count = allBuildingInstances.Count; i < count; i++)
			{
				loadBuilding(allBuildingInstances[i]);
			}
		}
	}

	private void OnDestroy()
	{
		_instance = null;
	}

	private void Update()
	{
		updateGridInteraction();
	}

	public void loadBuilding(BuildingInstance relatedInstance)
	{
		if (relatedInstance == null || relatedInstance._relatedVisuals != null)
		{
			return;
		}
		for (int i = 0; i < _buildings.Length; i++)
		{
			if (_buildings[i]._buildingType == relatedInstance._buildingDefinition._type)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(_buildings[i]._prefab);
				gameObject.transform.parent = base.transform;
				BuildingVisual component = gameObject.GetComponent<BuildingVisual>();
				component._relatedBuilding = relatedInstance;
				relatedInstance._relatedVisuals = component;
				component.setVisualDataFromString(relatedInstance._visualDataFromLastLoad);
				BuildingVisualGridEntity buildingVisualGridEntity = (BuildingVisualGridEntity)component;
				if (buildingVisualGridEntity != null)
				{
					_activeGridBuildings.Add(buildingVisualGridEntity);
				}
				component.refreshVisuals();
			}
		}
	}

	public void spawnBuildingAt(Point gridPoint, BuildingDefines.EBuildingType type)
	{
		if (!isActiveBuildingAt(gridPoint))
		{
			BuildingInstance buildingInstance = BuildingManager.Instance.CreateNewBuilding(type);
			loadBuilding(buildingInstance);
			BuildingVisualGridEntity buildingVisualGridEntity = (BuildingVisualGridEntity)buildingInstance._relatedVisuals;
			if (buildingVisualGridEntity != null)
			{
				buildingVisualGridEntity.setPosition(gridPoint);
			}
			buildingInstance._relatedVisuals.refreshVisuals();
		}
	}

	public void destroyVisual(BuildingVisual visualToDestroy)
	{
		BuildingVisualGridEntity buildingVisualGridEntity = (BuildingVisualGridEntity)visualToDestroy;
		if (buildingVisualGridEntity != null)
		{
			_activeGridBuildings.Remove(buildingVisualGridEntity);
		}
		UnityEngine.Object.Destroy(visualToDestroy.gameObject);
	}

	public bool isActiveBuildingAt(Point gridPoint)
	{
		for (int i = 0; i < _activeGridBuildings.Count; i++)
		{
			if (_activeGridBuildings[i].isAtPosition(gridPoint))
			{
				return true;
			}
		}
		return false;
	}

	public BuildingInstance getBuildingAt(Point gridPoint)
	{
		for (int i = 0; i < _activeGridBuildings.Count; i++)
		{
			if (_activeGridBuildings[i].isAtPosition(gridPoint))
			{
				return _activeGridBuildings[i]._relatedBuilding;
			}
		}
		return null;
	}

	private void updateGridInteraction()
	{
		if (Input.GetMouseButtonDown(0) && !(BuildingTestScreen.Instance == null))
		{
			Ray ray = _camForRay.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(ray, out hitInfo, 100f, _layerMaskForRay.value))
			{
				BuildingTestScreen.Instance.gridLocationWasClickedOn(GridManager.Instance.getNearestGridIndiciesToWorldPos(hitInfo.point));
			}
		}
	}
}
