using UnityEngine;
using UnityEngine.Rendering;

public class PathCharacterFollower : PathFollower
{
	private const float MAX_DIST_ERROR = 0.01f;

	private const float WIDTH = 20f;

	private const float HEIGHT = 20f;

	private const float BLOCKER_DIST_FORWARD = 2f;

	public Transform _target;

	public float _desiredDistFromTarget = 3f;

	private void Awake()
	{
		GameObject gameObject = new GameObject("PathBackBlocker");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = new Vector3(0f, 0f, 2f);
		gameObject.layer = LayerMask.NameToLayer("PlayerOnly");
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[4]
		{
			new Vector3(-10f, -10f),
			new Vector3(-10f, 10f),
			new Vector3(10f, 10f),
			new Vector3(10f, -10f)
		};
		mesh.triangles = new int[6] { 0, 3, 1, 1, 3, 2 };
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
		meshCollider.sharedMesh = mesh;
		if (Application.isEditor)
		{
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			meshFilter.mesh = mesh;
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.material = new Material(Shader.Find("Diffuse"));
			meshRenderer.material.color = Color.red;
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
		}
	}
}
