using System;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceOnCollide : MonoBehaviour
{
	public Transform _replaceWith;

	public static readonly List<ReplaceOnCollide> _all = new List<ReplaceOnCollide>();

	public event Action<Collider> _onReplace;

	private void Awake()
	{
		_all.Add(this);
	}

	private void OnTriggerEnter(Collider other)
	{
		Replace(other);
	}

	public Transform Replace(Collider other = null)
	{
		Transform transform = FastPoolManager.GetPool(_replaceWith).FastInstantiate<Transform>();
		transform.parent = base.transform.parent;
		transform.position = base.transform.position;
		transform.rotation = base.transform.rotation;
		base.gameObject.SetActive(false);
		if (this._onReplace != null)
		{
			this._onReplace(other);
		}
		_all.Remove(this);
		return transform;
	}

	private void OnDestroy()
	{
		_all.Remove(this);
	}
}
