using UnityEngine;

public class SimpleRotateZ : MonoBehaviour
{
	public float rotationsPerMinute = 10f;

	private float rotationsPerSecond;

	private void Start()
	{
		rotationsPerSecond = rotationsPerMinute / 60f;
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.value * 360f));
	}

	private void Update()
	{
		base.transform.Rotate(0f, 0f, 360f * rotationsPerSecond * RealTime.deltaTime);
	}
}
