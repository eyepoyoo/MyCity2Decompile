using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
	public float rotationsPerMinute = 10f;

	private void Start()
	{
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.value * 360f, 0f));
	}

	private void Update()
	{
		base.transform.Rotate(0f, 6f * rotationsPerMinute * Time.deltaTime, 0f);
	}
}
