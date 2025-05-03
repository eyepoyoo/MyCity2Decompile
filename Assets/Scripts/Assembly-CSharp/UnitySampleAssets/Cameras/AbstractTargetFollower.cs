using UnityEngine;

namespace UnitySampleAssets.Cameras
{
	public abstract class AbstractTargetFollower : MonoBehaviour
	{
		public enum UpdateType
		{
			FixedUpdate = 0,
			LateUpdate = 1
		}

		[SerializeField]
		protected Transform target;

		[SerializeField]
		private bool autoTargetPlayer = true;

		[SerializeField]
		private UpdateType updateType;

		public Transform Target
		{
			get
			{
				return target;
			}
		}

		protected virtual void Start()
		{
			if (autoTargetPlayer)
			{
				FindAndTargetPlayer();
			}
		}

		private void FixedUpdate()
		{
			if (autoTargetPlayer && (target == null || !target.gameObject.activeSelf))
			{
				FindAndTargetPlayer();
			}
			if (updateType == UpdateType.FixedUpdate)
			{
				FollowTarget(Time.deltaTime);
			}
		}

		private void LateUpdate()
		{
			if (autoTargetPlayer && (target == null || !target.gameObject.activeSelf))
			{
				FindAndTargetPlayer();
			}
			if (updateType == UpdateType.LateUpdate)
			{
				FollowTarget(Time.deltaTime);
			}
		}

		protected abstract void FollowTarget(float deltaTime);

		public void FindAndTargetPlayer()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if ((bool)gameObject)
			{
				SetTarget(gameObject.transform);
			}
		}

		public virtual void SetTarget(Transform newTransform)
		{
			target = newTransform;
		}
	}
}
