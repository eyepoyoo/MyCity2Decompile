using System.Collections.Generic;
using UnityEngine;

public abstract class AffectorArea : MonoBehaviour
{
	private readonly List<Rigidbody> _bodies = new List<Rigidbody>();

	private readonly List<int> _numColliders = new List<int>();

	protected virtual void FixedUpdate()
	{
		ApplyForcesToBodies();
	}

	protected void ApplyForcesToBodies()
	{
		for (int num = _bodies.Count - 1; num >= 0; num--)
		{
			if (_bodies[num] == null)
			{
				_bodies.RemoveAt(num);
			}
			else
			{
				ApplyForces(_bodies[num]);
			}
		}
	}

	protected abstract void ApplyForces(Rigidbody rb);

	private void OnTriggerEnter(Collider collider)
	{
		Rigidbody componentInParent = collider.GetComponentInParent<Rigidbody>();
		if ((bool)componentInParent)
		{
			int num = _bodies.IndexOf(componentInParent);
			if (num == -1)
			{
				_bodies.Add(componentInParent);
				_numColliders.Add(1);
				OnBodyEnter(componentInParent);
			}
			else
			{
				List<int> numColliders;
				List<int> list = (numColliders = _numColliders);
				int index2;
				int index = (index2 = num);
				index2 = numColliders[index2];
				list[index] = index2 + 1;
			}
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		Rigidbody componentInParent = collider.GetComponentInParent<Rigidbody>();
		if (!componentInParent)
		{
			return;
		}
		int num = _bodies.IndexOf(componentInParent);
		if (num != -1)
		{
			List<int> numColliders;
			List<int> list = (numColliders = _numColliders);
			int index2;
			int index = (index2 = num);
			index2 = numColliders[index2];
			list[index] = index2 - 1;
			if (_numColliders[num] == 0)
			{
				_bodies.RemoveAt(num);
				_numColliders.RemoveAt(num);
				OnBodyExit(componentInParent);
			}
		}
	}

	protected virtual void OnBodyEnter(Rigidbody rb)
	{
	}

	protected virtual void OnBodyExit(Rigidbody rb)
	{
	}
}
