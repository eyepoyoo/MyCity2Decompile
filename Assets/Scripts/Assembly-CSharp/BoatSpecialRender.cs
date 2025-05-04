using UnityEngine;

public class BoatSpecialRender : MonoBehaviour
{
	public class ReplacedPart
	{
		public readonly Transform _original;

		public readonly Transform _lower;

		public readonly Transform _upper;

		public ReplacedPart(Transform original, Transform lower, Transform upper)
		{
			_original = original;
			_lower = lower;
			_upper = upper;
		}
	}

	public GameObject[] specialLayerableObjects;

	public GameObject[] objectsToPushToUpperRender;

	public Material lowerLayerMaterial;

	public Material upperLayerMaterial;

	public void Setup()
	{
		int layer = LayerMask.NameToLayer("WaterVehicleSpecialRender1");
		int layer2 = LayerMask.NameToLayer("WaterVehicleSpecialRender2");
		if (specialLayerableObjects != null)
		{
			int num = specialLayerableObjects.Length;
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(specialLayerableObjects[i]);
				GameObject gameObject2 = Object.Instantiate(specialLayerableObjects[i]);
				gameObject.transform.parent = specialLayerableObjects[i].transform.parent;
				gameObject2.transform.parent = specialLayerableObjects[i].transform.parent;
				gameObject.transform.localScale = specialLayerableObjects[i].transform.localScale;
				gameObject2.transform.localScale = specialLayerableObjects[i].transform.localScale;
				gameObject.transform.localPosition = specialLayerableObjects[i].transform.localPosition;
				gameObject2.transform.localPosition = specialLayerableObjects[i].transform.localPosition;
				gameObject.transform.localRotation = specialLayerableObjects[i].transform.localRotation;
				gameObject2.transform.localRotation = specialLayerableObjects[i].transform.localRotation;
				gameObject.layer = layer;
				gameObject2.layer = layer2;
				gameObject2.name = specialLayerableObjects[i].name + " UpperLayer";
				gameObject.name = specialLayerableObjects[i].name + " LowerLayer";
				MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
				if (component != null)
				{
					component.sharedMaterial = lowerLayerMaterial;
				}
				component = gameObject2.GetComponent<MeshRenderer>();
				if (component != null)
				{
					component.sharedMaterial = upperLayerMaterial;
				}
				specialLayerableObjects[i].SetActive(false);
				base.gameObject.SendMessageUpwards("BoatSpecialRendererPartReplaced", new ReplacedPart(specialLayerableObjects[i].transform, gameObject.transform, gameObject2.transform), SendMessageOptions.DontRequireReceiver);
			}
		}
		if (objectsToPushToUpperRender != null)
		{
			int num2 = objectsToPushToUpperRender.Length;
			for (int j = 0; j < num2; j++)
			{
				objectsToPushToUpperRender[j].layer = layer2;
			}
		}
	}
}
