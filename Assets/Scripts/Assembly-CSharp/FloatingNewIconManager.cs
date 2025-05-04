using System.Collections.Generic;
using UnityEngine;

public class FloatingNewIconManager : MonoBehaviour
{
	[SerializeField]
	private List<FloatingNewIcon> _locks;

	public void HideAllNewIcons()
	{
		int i = 0;
		for (int count = _locks.Count; i < count; i++)
		{
			_locks[i].StopTrackingTransform();
		}
	}

	public void MakeNewIconTrackTransform(VehiclePart partToTrack)
	{
		Debug.Log("Request to Track part [" + partToTrack.gameObject.name + "] received.");
		int i = 0;
		for (int count = _locks.Count; i < count; i++)
		{
			if (!_locks[i]._pIsTracking)
			{
				_locks[i].TrackPart(partToTrack);
				return;
			}
		}
		Debug.Log("Unable to track part [" + partToTrack.gameObject.name + "]. Ran out of new icons!");
	}
}
