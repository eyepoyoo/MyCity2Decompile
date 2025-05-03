using System.Collections.Generic;
using UnityEngine;

namespace AmuzoPhysics
{
	public class Contact2d
	{
		private Collider2d _colliderA;

		private Collider2d _colliderB;

		private LinkedListNode<Contact2d> _colliderListNodeA;

		private LinkedListNode<Contact2d> _colliderListNodeB;

		private Vector2 _normal;

		private Vector2 _position;

		private float _penetration;

		private float _time;

		private ContactState _state;

		public Collider2d ColliderA
		{
			get
			{
				return _colliderA;
			}
		}

		public Collider2d ColliderB
		{
			get
			{
				return _colliderB;
			}
		}

		public float Penetration
		{
			get
			{
				return _penetration;
			}
		}

		public float _pTime
		{
			get
			{
				return _time;
			}
		}

		public ContactState State
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value;
			}
		}

		private bool _isSolid
		{
			get
			{
				return _colliderA != null && _colliderA.IsSolid && _colliderB != null && _colliderB.IsSolid;
			}
		}

		public bool IsSolid
		{
			get
			{
				return _isSolid;
			}
		}

		public Contact2d()
		{
			_colliderListNodeA = new LinkedListNode<Contact2d>(this);
			_colliderListNodeB = new LinkedListNode<Contact2d>(this);
			Reset();
		}

		public void Reset()
		{
			if (_colliderListNodeA.List != null)
			{
				_colliderListNodeA.List.Remove(_colliderListNodeA);
			}
			if (_colliderListNodeB.List != null)
			{
				_colliderListNodeB.List.Remove(_colliderListNodeB);
			}
			_colliderA = (_colliderB = null);
			_normal = (_position = Vector2.zero);
			_penetration = (_time = 0f);
			_state = ContactState.Null;
		}

		public void SetColliders(Collider2d collA, Collider2d collB)
		{
			if (_colliderA == null && _colliderB == null)
			{
				_colliderA = collA;
				collA.ContactList.AddLast(_colliderListNodeA);
				_colliderB = collB;
				collB.ContactList.AddLast(_colliderListNodeB);
			}
			else
			{
				Debug.LogError("[Contact2d] Attempting to change contact colliders...must Reset first");
			}
		}

		public void SetProperties(Vector2 normal, Vector2 position, float penetration, float time, Collider2d wrtCollider = null)
		{
			_normal = ((wrtCollider != _colliderB) ? normal : (-normal));
			_position = ((wrtCollider != _colliderB) ? position : (position + penetration * normal));
			_penetration = penetration;
			_time = time;
		}

		public Collider2d GetOtherCollider(Collider2d wrtCollider)
		{
			return (wrtCollider != _colliderB) ? _colliderB : _colliderA;
		}

		public Vector2 GetNormal(Collider2d wrtCollider)
		{
			return (wrtCollider != _colliderB) ? _normal : (-_normal);
		}

		public Vector2 GetPosition(Collider2d wrtCollider)
		{
			return (wrtCollider != _colliderB) ? _position : (_position + _penetration * _normal);
		}

		public bool IsOccluded(Collider2d wrtCollider)
		{
			Vector2 position = GetPosition(wrtCollider);
			Vector2 zero = Vector2.zero;
			foreach (Contact2d contact in wrtCollider.ContactList)
			{
				if (contact != this && contact._isSolid)
				{
					zero = position - contact.GetPosition(wrtCollider);
					if (Vector2.Dot(zero, contact.GetNormal(wrtCollider)) < 0f)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
