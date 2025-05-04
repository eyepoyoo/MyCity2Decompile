using UnityEngine;

public class ScrollSharedMaterial : MonoBehaviour
{
	public Vector2 scroll;

	private Material _mat;

	private Material _pMaterial
	{
		get
		{
			if (_mat == null)
			{
				MeshRenderer component = GetComponent<MeshRenderer>();
				if (component != null)
				{
					_mat = component.sharedMaterial;
				}
			}
			return _mat;
		}
	}

	private void Update()
	{
		_pMaterial.SetTextureOffset("_MainTex", Frac(scroll * Time.time));
	}

	private Vector2 Frac(Vector2 v)
	{
		Vector2 result = v;
		result.x = v.x - Mathf.Floor(v.x);
		result.y = v.y - Mathf.Floor(v.y);
		return result;
	}
}
