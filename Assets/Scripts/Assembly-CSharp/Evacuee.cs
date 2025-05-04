using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Evacuee : MonoBehaviour
{
	public Transform _grappleAnchor;

	public Rigidbody _pRigidbody { get; private set; }

	private void Awake()
	{
		_pRigidbody = GetComponent<Rigidbody>();
	}
}
