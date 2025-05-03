using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Connectors : MonoBehaviour
{
	public enum ECONNECTOR_TYPE
	{
		INVALID = 0,
		TOP_FRONT = 1,
		TOP_BACK = 2,
		BACK = 3,
		WHEEL_FL = 4,
		WHEEL_FR = 5,
		WHEEL_BR = 6,
		WHEEL_BL = 7,
		FRONT = 8,
		WING = 9,
		FRONT_SIDES = 10,
		BACK_SIDES = 11,
		TOP_CENTRE = 12
	}

	[Serializable]
	public class PartOverride
	{
		public VehiclePart.EUNIQUE_ID _part;

		public Transform _connector;
	}

	private const float ANIMATE_DISTANCE = 3f;

	private const float ANIMATE_DURATION = 0.15f;

	public Transform _wheelFL;

	public Transform _wheelFR;

	public Transform _wheelBL;

	public Transform _wheelBR;

	[FormerlySerializedAs("_top")]
	public Transform _topFront;

	public Transform _topCentre;

	public Transform _topBack;

	public Transform _back;

	public Transform _front;

	public Transform _wingAttachL;

	public Transform _wingAttachR;

	[FormerlySerializedAs("_extraAttachL")]
	public Transform _frontSidesL;

	[FormerlySerializedAs("_extraAttachR")]
	public Transform _frontSidesR;

	public Transform _backSidesL;

	public Transform _backSidesR;

	public bool _wheelFLVisible = true;

	public bool _wheelFRVisible = true;

	public bool _wheelBLVisible = true;

	public bool _wheelBRVisible = true;

	public PartOverride[] _partOverrides;

	public void Attach(VehiclePart vehiclePart)
	{
		_Attach(vehiclePart.transform, vehiclePart.uniqueID, vehiclePart.connectionType);
		if (vehiclePart.secondaryFlippedPart != null)
		{
			vehiclePart.secondaryFlippedPart.SetActive(true);
			_Attach(vehiclePart.secondaryFlippedPart.transform, vehiclePart.uniqueID, vehiclePart.connectionType, true);
		}
		switch (vehiclePart.connectionType)
		{
		case ECONNECTOR_TYPE.WHEEL_BL:
			if (!_wheelBLVisible)
			{
				vehiclePart.SetMeshesVisible(false);
			}
			break;
		case ECONNECTOR_TYPE.WHEEL_BR:
			if (!_wheelBRVisible)
			{
				vehiclePart.SetMeshesVisible(false);
			}
			break;
		case ECONNECTOR_TYPE.WHEEL_FL:
			if (!_wheelFLVisible)
			{
				vehiclePart.SetMeshesVisible(false);
			}
			break;
		case ECONNECTOR_TYPE.WHEEL_FR:
			if (!_wheelFRVisible)
			{
				vehiclePart.SetMeshesVisible(false);
			}
			break;
		}
	}

	private void _Attach(Transform part, VehiclePart.EUNIQUE_ID partId, ECONNECTOR_TYPE connectorType, bool isSecondary = false)
	{
		Transform connector = GetConnector(connectorType, partId, isSecondary);
		if (connector == null)
		{
			Debug.LogError(string.Concat("Tried to attach item ", part.name, " to connection point ", connectorType, " but there is no assigned transform for that point"), this);
		}
		else
		{
			part.parent = connector;
			part.localPosition = Vector3.zero;
			part.localRotation = Quaternion.identity;
		}
	}

	public void Animate(VehiclePart vehiclePart, float delay = 0f, Action<Transform> onComplete = null)
	{
		Vector3 vector = Vector3.zero;
		switch (vehiclePart.connectionType)
		{
		case ECONNECTOR_TYPE.BACK:
			vector = Vector3.forward;
			break;
		case ECONNECTOR_TYPE.TOP_FRONT:
			vector = Vector3.up;
			break;
		case ECONNECTOR_TYPE.TOP_CENTRE:
			vector = Vector3.up;
			break;
		case ECONNECTOR_TYPE.TOP_BACK:
			vector = Vector3.up;
			break;
		case ECONNECTOR_TYPE.WHEEL_BL:
			vector = Vector3.left;
			break;
		case ECONNECTOR_TYPE.WHEEL_BR:
			vector = Vector3.right;
			break;
		case ECONNECTOR_TYPE.WHEEL_FL:
			vector = Vector3.left;
			break;
		case ECONNECTOR_TYPE.WHEEL_FR:
			vector = Vector3.right;
			break;
		case ECONNECTOR_TYPE.FRONT:
			vector = Vector3.forward;
			break;
		case ECONNECTOR_TYPE.WING:
			vector = Vector3.left;
			break;
		case ECONNECTOR_TYPE.FRONT_SIDES:
			vector = Vector3.left;
			break;
		case ECONNECTOR_TYPE.BACK_SIDES:
			vector = Vector3.left;
			break;
		}
		vehiclePart.transform.localPosition = vector * 3f;
		float delay2 = delay;
		vehiclePart.transform.TweenToPos(Vector3.zero, 0.15f, delegate
		{
			if (onComplete != null && !vehiclePart.secondaryFlippedPart)
			{
				onComplete(vehiclePart.transform);
			}
		}, Easing.EaseType.EaseIn, true, true, delay2);
		if (!vehiclePart.secondaryFlippedPart)
		{
			return;
		}
		Vector3 localPosition = vehiclePart.secondaryFlippedPart.transform.localPosition;
		vector.x *= -1f;
		vehiclePart.secondaryFlippedPart.transform.localPosition += vector * 3f;
		delay2 = delay + 0.2f;
		vehiclePart.secondaryFlippedPart.transform.TweenToPos(localPosition, 0.15f, delegate
		{
			if (onComplete != null)
			{
				onComplete(vehiclePart.secondaryFlippedPart.transform);
			}
		}, Easing.EaseType.EaseIn, true, true, delay2);
	}

	private Transform GetConnector(ECONNECTOR_TYPE type, VehiclePart.EUNIQUE_ID partId = VehiclePart.EUNIQUE_ID.INVALID, bool isSecondary = false)
	{
		if (isSecondary)
		{
			switch (type)
			{
			case ECONNECTOR_TYPE.WING:
				return _wingAttachR;
			case ECONNECTOR_TYPE.FRONT_SIDES:
				return _frontSidesR;
			case ECONNECTOR_TYPE.BACK_SIDES:
				return _backSidesR;
			default:
				return null;
			}
		}
		if (_partOverrides != null)
		{
			for (int num = _partOverrides.Length - 1; num >= 0; num--)
			{
				if (_partOverrides[num]._part == partId)
				{
					return _partOverrides[num]._connector;
				}
			}
		}
		switch (type)
		{
		case ECONNECTOR_TYPE.BACK:
			return _back;
		case ECONNECTOR_TYPE.TOP_FRONT:
			return _topFront;
		case ECONNECTOR_TYPE.TOP_CENTRE:
			return _topCentre;
		case ECONNECTOR_TYPE.TOP_BACK:
			return _topBack;
		case ECONNECTOR_TYPE.WHEEL_BL:
			return _wheelBL;
		case ECONNECTOR_TYPE.WHEEL_BR:
			return _wheelBR;
		case ECONNECTOR_TYPE.WHEEL_FL:
			return _wheelFL;
		case ECONNECTOR_TYPE.WHEEL_FR:
			return _wheelFR;
		case ECONNECTOR_TYPE.FRONT:
			return _front;
		case ECONNECTOR_TYPE.WING:
			return _wingAttachL;
		case ECONNECTOR_TYPE.FRONT_SIDES:
			return _frontSidesL;
		case ECONNECTOR_TYPE.BACK_SIDES:
			return _backSidesL;
		default:
			return null;
		}
	}
}
