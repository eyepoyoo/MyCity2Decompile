using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TerrainEffectArea : MonoBehaviour
{
	public enum ETerrainEffect
	{
		Uneven = 0,
		Mud = 1,
		Water = 2,
		Lava = 3
	}

	private readonly List<Vehicle> _vehiclesInside = new List<Vehicle>();

	public ETerrainEffect _terrainEffect;

	private static readonly Dictionary<ETerrainEffect, VehiclePart.EObstacleToNegate[]> _negatedBy = new Dictionary<ETerrainEffect, VehiclePart.EObstacleToNegate[]>
	{
		{
			ETerrainEffect.Mud,
			new VehiclePart.EObstacleToNegate[1]
		},
		{
			ETerrainEffect.Lava,
			new VehiclePart.EObstacleToNegate[0]
		}
	};

	private Type _pTerrainEffectType
	{
		get
		{
			switch (_terrainEffect)
			{
			case ETerrainEffect.Mud:
				return typeof(TerrainEffect_Mud);
			case ETerrainEffect.Lava:
				return typeof(TerrainEffect_Lava);
			default:
				return null;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Vehicle componentInParent = other.GetComponentInParent<Vehicle>();
		if ((bool)componentInParent)
		{
			if (!_vehiclesInside.Contains(componentInParent))
			{
				VehicleEntered(componentInParent);
			}
			_vehiclesInside.Add(componentInParent);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Vehicle componentInParent = other.GetComponentInParent<Vehicle>();
		if ((bool)componentInParent)
		{
			_vehiclesInside.Remove(componentInParent);
			if (!_vehiclesInside.Contains(componentInParent))
			{
				VehicleExited(componentInParent);
			}
		}
	}

	private void VehicleEntered(Vehicle vehicle)
	{
		if (!vehicle.NegatesObstacle(_negatedBy[_terrainEffect]))
		{
			if ((bool)GetExistingTerrainEffect(vehicle))
			{
				GetExistingTerrainEffect(vehicle).Reset();
			}
			else
			{
				vehicle.gameObject.AddComponent(_pTerrainEffectType);
			}
		}
	}

	private void VehicleExited(Vehicle vehicle)
	{
		if ((bool)GetExistingTerrainEffect(vehicle))
		{
			GetExistingTerrainEffect(vehicle).StartRemove();
		}
	}

	private TerrainEffect GetExistingTerrainEffect(Vehicle vehicle)
	{
		return (TerrainEffect)vehicle.gameObject.GetComponent(_pTerrainEffectType);
	}
}
