using UnityEngine;

public class TextureScroller : MonoBehaviour
{
	public Vector2 scrollSpeed;

	private void Update()
	{
		Vector2 offset = new Vector2(Time.time * scrollSpeed.x, Time.time * scrollSpeed.y);
		GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);
	}
}
