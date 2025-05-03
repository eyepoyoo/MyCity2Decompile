using UnityEngine;

[AddComponentMenu("Debug/Arrow")]
public class Arrow : MonoBehaviour
{
	public enum ColorName
	{
		Blue = 0,
		Black = 1,
		Green = 2,
		Cyan = 3,
		White = 4,
		Red = 5,
		Yellow = 6,
		Magenta = 7,
		Grey = 8
	}

	public ColorName arrowColor = ColorName.Cyan;

	public float arrowLength = 2f;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
		switch (arrowColor)
		{
		case ColorName.Blue:
			Gizmos.color = Color.blue;
			break;
		case ColorName.Black:
			Gizmos.color = Color.black;
			break;
		case ColorName.Green:
			Gizmos.color = Color.green;
			break;
		case ColorName.Cyan:
			Gizmos.color = Color.cyan;
			break;
		case ColorName.White:
			Gizmos.color = Color.white;
			break;
		case ColorName.Red:
			Gizmos.color = Color.red;
			break;
		case ColorName.Yellow:
			Gizmos.color = Color.yellow;
			break;
		case ColorName.Magenta:
			Gizmos.color = Color.magenta;
			break;
		case ColorName.Grey:
			Gizmos.color = Color.grey;
			break;
		}
		Gizmos.DrawSphere(base.transform.position, 0.2f);
		GizmosPlus.DrawArrow(base.transform.position, base.transform.forward, base.transform.right, 0.6f, 0.9f, 1.2f, arrowLength);
	}
}
