using System;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class Label3d : MonoBehaviour
{
	private TextMesh textMesh;

	private Camera camera;

	public float size = 1f;

	public string text
	{
		set
		{
			textMesh.text = value;
		}
	}

	public Color color
	{
		set
		{
			textMesh.color = value;
		}
	}

	private void Awake()
	{
		camera = UnityEngine.Object.FindObjectOfType<Camera>();
		textMesh = GetComponent<TextMesh>();
	}

	private void Update()
	{
		if ((bool)camera)
		{
			base.transform.LookAt(camera.transform);
			base.transform.localScale = new Vector3(-1f, 1f, 1f) * Vector3.Distance(camera.transform.position, base.transform.position) * 0.05f * Mathf.Tan(camera.fieldOfView * ((float)Math.PI / 180f) * 0.5f) * size;
		}
	}
}
