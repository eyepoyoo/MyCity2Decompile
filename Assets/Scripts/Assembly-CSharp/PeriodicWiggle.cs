using UnityEngine;

public class PeriodicWiggle : MonoBehaviour
{
	public float wiggleRate = 15f;

	public float wiggleMagnitude = 9f;

	public float idleTime = 3f;

	public float wiggleDuration = 0.5f;

	private float _idleTimer;

	private float _wiggleTimer;

	private void Update()
	{
		if (_idleTimer <= 0f)
		{
			_wiggleTimer += Time.deltaTime;
			base.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(_wiggleTimer * wiggleRate) * wiggleMagnitude);
			if (_wiggleTimer >= wiggleDuration)
			{
				_idleTimer = idleTime;
				_wiggleTimer = 0f;
			}
		}
		else
		{
			if (base.transform.localEulerAngles.z != 0f)
			{
				base.transform.localEulerAngles = Vector3.zero;
			}
			_wiggleTimer = 0f;
			_idleTimer -= Time.deltaTime;
		}
	}
}
