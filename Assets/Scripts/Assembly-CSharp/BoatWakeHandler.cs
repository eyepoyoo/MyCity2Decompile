using UnityEngine;

public class BoatWakeHandler : MonoBehaviour
{
	public GameObject boatWakePrefab;

	public Vehicle_Boat boatRef;

	private BoatWake _wake;

	private void Start()
	{
		if (MinigameController._pInstance != null)
		{
			GameObject gameObject = Object.Instantiate(boatWakePrefab);
			_wake = gameObject.GetComponent<BoatWake>();
		}
	}

	private void Update()
	{
		if (_wake != null)
		{
			_wake._pIsInWater = boatRef._pIsGrounded;
			if (_wake._pIsInWater)
			{
				float num = Mathf.Sign(boatRef._pForwardSpeed) * boatRef._pSpeedFactor;
				_wake._pSpeed = Mathf.Clamp(num * _wake.maxSpeed, 0f - _wake.maxSpeed, _wake.maxSpeed);
				Vector3 position = base.transform.position;
				position.y = 0.1f;
				_wake.transform.position = position;
				_wake.transform.rotation = Quaternion.Euler(0f, base.transform.eulerAngles.y, 0f);
			}
		}
	}
}
