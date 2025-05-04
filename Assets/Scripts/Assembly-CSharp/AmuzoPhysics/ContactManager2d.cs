using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmuzoPhysics
{
	public class ContactManager2d
	{
		private ObjectPool<Contact2d> _contactPool;

		private bool _isOcclusionCullingEnabled = true;

		private bool _isSortContacts = true;

		public bool _isDebugDump;

		private int _debugDumpCount;

		private int _contactPoolSize
		{
			get
			{
				return (_contactPool != null) ? _contactPool.Size : 0;
			}
		}

		public IEnumerable<Contact2d> ActiveContacts
		{
			get
			{
				return _contactPool.ActiveObjects;
			}
		}

		public bool IsOcclusionCullingEnabled
		{
			get
			{
				return _isOcclusionCullingEnabled;
			}
			set
			{
				_isOcclusionCullingEnabled = value;
			}
		}

		public bool IsSortContacts
		{
			get
			{
				return _isSortContacts;
			}
			set
			{
				_isSortContacts = value;
			}
		}

		public ContactManager2d(int maxContacts)
		{
			_contactPool = new ObjectPool<Contact2d>(maxContacts, () => new Contact2d(), null);
		}

		private Contact2d CreateContact()
		{
			if (_contactPool != null)
			{
				Contact2d contact2d = _contactPool.Allocate();
				if (contact2d != null)
				{
					contact2d.Reset();
					return contact2d;
				}
			}
			return null;
		}

		private void DestroyContact(Contact2d contact)
		{
			contact.Reset();
			if (_contactPool != null)
			{
				_contactPool.Free(contact);
			}
		}

		public void OnBeginCollisionDetection()
		{
			foreach (Contact2d activeObject in _contactPool.ActiveObjects)
			{
				activeObject.State = ContactState.Old;
			}
		}

		public void OnEndCollisionDetection()
		{
			foreach (Contact2d activeObject in _contactPool.ActiveObjects)
			{
				if (activeObject.State == ContactState.Old)
				{
					activeObject.ColliderA.OnEndContact(activeObject);
					activeObject.ColliderB.OnEndContact(activeObject);
					DestroyContact(activeObject);
				}
			}
			if (_isOcclusionCullingEnabled)
			{
				DoOcclusionCulling();
			}
			if (_isSortContacts)
			{
				_SortContacts();
			}
			foreach (Contact2d activeObject2 in _contactPool.ActiveObjects)
			{
				if (activeObject2.State == ContactState.New)
				{
					activeObject2.ColliderA.OnBeginContact(activeObject2);
					activeObject2.ColliderB.OnBeginContact(activeObject2);
				}
			}
			if (!_isDebugDump)
			{
				return;
			}
			foreach (Contact2d activeObject3 in _contactPool.ActiveObjects)
			{
				if (Collider2d.DebugInstance == null || activeObject3.ColliderA == Collider2d.DebugInstance || activeObject3.ColliderB == Collider2d.DebugInstance)
				{
					Debug.Log(_debugDumpCount.ToString("D4") + ":pos=" + activeObject3.GetPosition(Collider2d.DebugInstance).ToString() + ", norm=" + activeObject3.GetNormal(Collider2d.DebugInstance).ToString() + ", pen=" + activeObject3.Penetration);
				}
			}
			_debugDumpCount++;
		}

		public bool OnCollisionDetected(Collider2d colliderA, Collider2d colliderB, Collision2d.Result collisionResult)
		{
			bool result = false;
			Contact2d contact2d = colliderA.FindContactWith(colliderB);
			if (contact2d != null)
			{
				contact2d.SetProperties(collisionResult._normal, collisionResult._position, collisionResult._penetration, collisionResult._time, colliderA);
				contact2d.State = ContactState.Persist;
				result = true;
			}
			else
			{
				contact2d = CreateContact();
				if (contact2d != null)
				{
					contact2d.SetColliders(colliderA, colliderB);
					contact2d.SetProperties(collisionResult._normal, collisionResult._position, collisionResult._penetration, collisionResult._time, colliderA);
					contact2d.State = ContactState.New;
					result = true;
				}
				else
				{
					Debug.LogWarning("[ContactManager2d] Failed to allocate contact.  Consider increasing contact buffer size (currently " + _contactPoolSize + ")");
				}
			}
			return result;
		}

		private void DoOcclusionCulling()
		{
			CollisionProfiler.BeginSection("contact_occlusion_culling");
			foreach (Contact2d activeObject in _contactPool.ActiveObjects)
			{
				if (activeObject.State == ContactState.New)
				{
					bool isDynamic = activeObject.ColliderA.IsDynamic;
					bool isDynamic2 = activeObject.ColliderB.IsDynamic;
					if (isDynamic && !isDynamic2 && activeObject.IsOccluded(activeObject.ColliderA))
					{
						DestroyContact(activeObject);
					}
					else if (isDynamic2 && !isDynamic && activeObject.IsOccluded(activeObject.ColliderB))
					{
						DestroyContact(activeObject);
					}
				}
			}
			CollisionProfiler.EndSection();
		}

		private void _SortContacts()
		{
			LinkedList<Contact2d> activeList = _contactPool.ActiveList;
			LinkedListNode<Contact2d> linkedListNode = activeList.First;
			LinkedListNode<Contact2d> linkedListNode2 = null;
			while (linkedListNode != null)
			{
				linkedListNode2 = linkedListNode.Next;
				if (linkedListNode.Value.ColliderA.IsDynamic && linkedListNode.Value.ColliderB.IsDynamic)
				{
					activeList.Remove(linkedListNode);
					activeList.AddFirst(linkedListNode);
				}
				linkedListNode = linkedListNode2;
			}
		}

		public void DebugRenderContacts(Func<Vector3, Vector3> transformPoint, float normalLength)
		{
			Color red = Color.red;
			Color green = Color.green;
			foreach (Contact2d activeContact in ActiveContacts)
			{
				Vector3 vector = activeContact.GetPosition(activeContact.ColliderA);
				Vector3 arg = vector + activeContact.Penetration * (Vector3)activeContact.GetNormal(activeContact.ColliderA);
				vector = transformPoint(vector);
				arg = transformPoint(arg);
				DebugDraw.DrawLine(vector, arg, red);
				float num = normalLength - (arg - vector).magnitude;
				if (num > 0f)
				{
					DebugDraw.DrawLine(arg, arg + num * (arg - vector).normalized, green);
				}
			}
		}
	}
}
