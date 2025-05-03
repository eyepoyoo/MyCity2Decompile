using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestAvailabilityTable
{
	private Dictionary<string, Func<bool>> _availableLookup = new Dictionary<string, Func<bool>>();

	public bool IsAvailable(string achiName)
	{
		return _availableLookup[achiName]();
	}

	public void Initialise()
	{
		_availableLookup["CatchTheCrooksWithPoliceCar"] = AlwaysAvailable;
		_availableLookup["CatchTheCrooksWithPoliceBuggy"] = AlwaysAvailable;
		_availableLookup["Collect100StudsSingleMission"] = AlwaysAvailable;
		_availableLookup["FireFrenzyWithFireEngine"] = AlwaysAvailable;
		_availableLookup["FireFrenzyWithFireQuadBike"] = AlwaysAvailable;
		_availableLookup["Collect50StudsSingleMission"] = AlwaysAvailable;
		_availableLookup["WaterDumpWithLargeFireHeli"] = AlwaysAvailable;
		_availableLookup["WaterDumpWithSmallFireHeli"] = AlwaysAvailable;
		_availableLookup["Play3PoliceMissions"] = AlwaysAvailable;
		_availableLookup["Play3FireMissions"] = AlwaysAvailable;
		_availableLookup["Play3VolcanoMissions"] = AlwaysAvailable;
		_availableLookup["RollinRocksWithExplorerTruck"] = AlwaysAvailable;
		_availableLookup["RollinRocksWithMiniTractor"] = AlwaysAvailable;
		_availableLookup["ExplorerEvacWithExplorerHeli"] = AlwaysAvailable;
		_availableLookup["ExplorerEvacWithExplorerChinook"] = AlwaysAvailable;
		_availableLookup["Collect500Studs"] = AlwaysAvailable;
		_availableLookup["Collect1000Studs"] = AlwaysAvailable;
		_availableLookup["Collect5000Studs"] = AlwaysAvailable;
		_availableLookup["PlayWithPortaloo"] = AlwaysAvailable;
		_availableLookup["PlayWithFloatingTyre"] = AlwaysAvailable;
		_availableLookup["PlayWithExplorerUAV"] = AlwaysAvailable;
		Debug.Log("Initialised Quest Availability Lookup Table");
	}

	private bool AlwaysAvailable()
	{
		return true;
	}

	private bool AlwaysDisabled()
	{
		return false;
	}
}
