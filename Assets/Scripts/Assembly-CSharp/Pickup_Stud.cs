using UnityEngine;

public class Pickup_Stud : Pickup
{
	public const float RADIUS = 0.5f;

	public const float Y_OFFSET = 1f;

	public static Vector3 _floatToPos = new Vector3(-3.4f, 2.4f, 4.9f);

	public override float _pRadius
	{
		get
		{
			return 0.5f;
		}
	}

	private void Awake()
	{
		MeshRenderer componentInChildren = GetComponentInChildren<MeshRenderer>();
		if (componentInChildren != null && ScenarioManager._pInstance != null && ScenarioManager._pInstance._pCurrentScenario != null)
		{
			componentInChildren.sharedMaterial = ScenarioManager._pInstance._pCurrentScenario.inGameStudMaterial;
		}
	}

	protected override void OnCollected(Vector3 pos)
	{
		MinigameController._pInstance.AwardFloatingStud(pos, base.transform.rotation);
	}
}
