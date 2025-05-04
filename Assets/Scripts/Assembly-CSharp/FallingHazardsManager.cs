using UnityEngine;

public class FallingHazardsManager : MonoBehaviour
{
	public FallingHazard _prefab;

	public float _initialDelay = 5f;

	public float _intervalMin = 2f;

	public float _intervalMax = 5f;

	public float _minDistFromPlayer;

	public float _maxDistFromPlayer;

	public float _maxAngleInFrontOfPlayer;

	public bool _onlySpawnIfCanSeeSpawnPoint = true;

	public Transform[] _spawnPoints;

	private float _spawnTimer;

	private void Awake()
	{
		_spawnTimer = _initialDelay;
	}

	private void Update()
	{
		if (MinigameController._pInstance._pStage == MinigameController.EStage.InProgress && (_spawnTimer -= Time.deltaTime) <= 0f)
		{
			_spawnTimer = Random.Range(_intervalMin, _intervalMax);
			Transform spawnPoint = GetSpawnPoint();
			if ((bool)spawnPoint)
			{
				SpawnHazard(spawnPoint.transform.position);
			}
		}
	}

	private void SpawnHazard(Vector3 position)
	{
		FallingHazard fallingHazard = FastPoolManager.GetPool(_prefab).FastInstantiate<FallingHazard>();
		fallingHazard.transform.position = position;
		fallingHazard.Launch(_minDistFromPlayer, _maxDistFromPlayer, _maxAngleInFrontOfPlayer);
	}

	private Transform GetSpawnPoint()
	{
		float num = float.PositiveInfinity;
		Transform transform = null;
		float num2 = float.PositiveInfinity;
		Transform transform2 = null;
		for (int num3 = _spawnPoints.Length - 1; num3 >= 0; num3--)
		{
			Transform transform3 = _spawnPoints[num3];
			float num4 = MathHelper.DistXZSqrd(VehicleController_Player._pInstance.transform.position, transform3.position);
			if (num4 < num)
			{
				num = num4;
				transform = transform3;
			}
			if (num4 < num2 && MinigameController._pInstance._pCamera._pCamera.IsPointInFrustrum(transform3.position, 0f))
			{
				num2 = num4;
				transform2 = transform3;
			}
		}
		if ((bool)transform2)
		{
			return transform2;
		}
		if ((bool)transform && !_onlySpawnIfCanSeeSpawnPoint)
		{
			return transform;
		}
		return null;
	}
}
