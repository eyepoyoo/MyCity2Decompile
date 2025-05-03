using UnityEngine;
using UnitySampleAssets.Vehicles.Car;

public class VehicleConstructor : MonoBehaviour
{
	public VehiclePart _body;

	public VehiclePart _wheel;

	public VehiclePart _specialAbility;

	public VehiclePart _attachment;

	public VehiclePartProperties _bodyProp;

	public VehiclePartProperties _wheelProp;

	public VehiclePartProperties _specialAbilityProp;

	public VehiclePartProperties _attachmentProp;

	public Vehicle.EType _vehicleType;

	public FollowTransform followTransformToSetUp;

	public Vehicle ConstructVehicle(bool isPlayer = false)
	{
		if (_bodyProp == null)
		{
			Debug.LogError("Body prefab not assigned!");
			return null;
		}
		if (isPlayer && (VehicleBuilder._templateAttachment != null || VehicleBuilder._templateBody != null || VehicleBuilder._templateSpecialAbility != null || VehicleBuilder._templateWheel != null))
		{
			_bodyProp = ((VehicleBuilder._templateBody != null) ? VehicleBuilder._templateBody.GetAltPartOrThis(_vehicleType) : null);
			_wheelProp = ((VehicleBuilder._templateWheel != null) ? VehicleBuilder._templateWheel.GetAltPartOrThis(_vehicleType) : null);
			_specialAbilityProp = ((VehicleBuilder._templateSpecialAbility != null) ? VehicleBuilder._templateSpecialAbility.GetAltPartOrThis(_vehicleType) : null);
			_attachmentProp = ((VehicleBuilder._templateAttachment != null) ? VehicleBuilder._templateAttachment.GetAltPartOrThis(_vehicleType) : null);
		}
		VehiclePart vehiclePart = _bodyProp.SpawnAsVehiclePart();
		Vehicle component = vehiclePart.GetComponent<Vehicle>();
		vehiclePart.transform.rotation = base.transform.rotation;
		vehiclePart.transform.parent = base.transform.parent;
		vehiclePart.transform.position = base.transform.position;
		BoatSpecialRender component2 = vehiclePart.GetComponent<BoatSpecialRender>();
		if (component2 != null && isPlayer)
		{
			component2.Setup();
		}
		if (_wheelProp != null && !string.IsNullOrEmpty(_wheelProp._resourceString))
		{
			GameObject resource = _wheelProp.GetResource();
			if (resource != null)
			{
				resource.gameObject.SetActive(false);
				VehiclePart component3 = Object.Instantiate(resource).GetComponent<VehiclePart>();
				component3.connectionType = Connectors.ECONNECTOR_TYPE.WHEEL_FL;
				component3.AttachToVehicle(component);
				component3.gameObject.SetActive(true);
				component3.GetComponentInChildren<Wheel>().SetFrontLeft();
				VehiclePart component4 = Object.Instantiate(resource).GetComponent<VehiclePart>();
				component4.connectionType = Connectors.ECONNECTOR_TYPE.WHEEL_FR;
				component4.AttachToVehicle(component);
				component4.gameObject.SetActive(true);
				component4.GetComponentInChildren<Wheel>().SetFrontRight();
				VehiclePart component5 = Object.Instantiate(resource).GetComponent<VehiclePart>();
				component5.connectionType = Connectors.ECONNECTOR_TYPE.WHEEL_BL;
				component5.AttachToVehicle(component);
				component5.gameObject.SetActive(true);
				component5.GetComponentInChildren<Wheel>().SetBackLeft();
				VehiclePart component6 = Object.Instantiate(resource).GetComponent<VehiclePart>();
				component6.connectionType = Connectors.ECONNECTOR_TYPE.WHEEL_BR;
				component6.AttachToVehicle(component);
				component6.gameObject.SetActive(true);
				component6.GetComponentInChildren<Wheel>().SetBackRight();
				resource.gameObject.SetActive(true);
				vehiclePart.transform.position += Vector3.up * component._pCentreOffsetFromBottom;
			}
		}
		if (_specialAbilityProp != null && !string.IsNullOrEmpty(_specialAbilityProp._resourceString))
		{
			VehiclePart vehiclePart2 = _specialAbilityProp.SpawnAsVehiclePart();
			vehiclePart2.AttachToVehicle(component);
			vehiclePart2.GetComponent<SpecialAbility>().AssignToVehicle(component);
			component2 = vehiclePart2.GetComponent<BoatSpecialRender>();
			if (component2 != null && isPlayer)
			{
				component2.Setup();
			}
		}
		if (_attachmentProp != null && !string.IsNullOrEmpty(_attachmentProp._resourceString))
		{
			VehiclePart vehiclePart3 = _attachmentProp.SpawnAsVehiclePart();
			vehiclePart3.AttachToVehicle(component);
			component2 = vehiclePart3.GetComponent<BoatSpecialRender>();
			if (component2 != null && isPlayer)
			{
				component2.Setup();
			}
		}
		if (followTransformToSetUp != null && isPlayer)
		{
			followTransformToSetUp.followTarget = vehiclePart.transform;
			followTransformToSetUp.gameObject.SetActive(true);
		}
		return component;
	}
}
