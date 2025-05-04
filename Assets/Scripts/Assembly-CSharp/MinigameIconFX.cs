using UnityEngine;

public class MinigameIconFX : MonoBehaviour
{
	public float alpha = 1f;

	private MeshRenderer _meshRenderer;

	private MeshRenderer _pMeshRenderer
	{
		get
		{
			if (_meshRenderer == null)
			{
				_meshRenderer = GetComponent<MeshRenderer>();
			}
			return _meshRenderer;
		}
	}

	private void Update()
	{
		_pMeshRenderer.material.SetFloat("_Alpha", alpha);
	}
}
