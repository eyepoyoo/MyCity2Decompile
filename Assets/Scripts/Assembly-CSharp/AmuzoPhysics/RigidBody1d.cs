using UnityEngine;

namespace AmuzoPhysics
{
	public class RigidBody1d
	{
		private const string LOG_TAG = "[RigidBody1d] ";

		public static RigidBody1d _staticWorldBody = new RigidBody1d();

		private float _mass;

		private float _invMass;

		private float _pos;

		private float _vel;

		private float _lambda;

		public float _pMass
		{
			get
			{
				return _mass;
			}
			set
			{
				SetMass(value);
			}
		}

		public float _pPosition
		{
			get
			{
				return _pos;
			}
			set
			{
				SetPosition(value);
			}
		}

		public float _pVelocity
		{
			get
			{
				return _vel;
			}
			set
			{
				SetLinearVelocity(value);
			}
		}

		public float _pLambda
		{
			get
			{
				return _lambda;
			}
			set
			{
				_lambda = value;
			}
		}

		public static void DoCollisionResponse(RigidBody1d bodyA, RigidBody1d bodyB, float contactNorm, float contactPen, float bounce)
		{
			if (!(contactPen < 0f))
			{
				float num = bodyA._invMass / (bodyA._invMass + bodyB._invMass) * contactPen;
				float num2 = contactPen - num;
				bodyA._pPosition += num * contactNorm;
				bodyB._pPosition -= num2 * contactNorm;
				float pVelocity = bodyA._pVelocity;
				float pVelocity2 = bodyB._pVelocity;
				float currVelR = pVelocity - pVelocity2;
				float num3 = currVelR * contactNorm;
				if (!(num3 >= 0f))
				{
					float num4 = CalculateRequiredImpulse(bodyA, bodyB, ref currVelR, ref contactNorm, (0f - bounce) * num3, 1f);
					float num5 = num4 * contactNorm;
					bodyA.ApplyImpulse(num5);
					bodyB.ApplyImpulse(0f - num5);
				}
			}
		}

		public void ApplyAccel(float accel, float dt)
		{
			SetLinearVelocity(_vel + dt * accel);
		}

		public void ApplyForce(float force, float dt)
		{
			SetLinearVelocity(_vel + dt * _invMass * force);
		}

		public void ApplyImpulse(float impulse)
		{
			SetLinearVelocity(_vel + _invMass * impulse);
		}

		public void Update(float dt)
		{
			if (_lambda > 0f)
			{
				SetLinearVelocity(_vel * 1f / Mathf.Pow(2f, dt / _lambda));
			}
			Integrate(dt);
		}

		private void SetMass(float mass)
		{
			if (mass < 0f)
			{
				mass = 0f;
			}
			_mass = mass;
			if (mass == 0f)
			{
				_invMass = 0f;
			}
			else
			{
				_invMass = 1f / mass;
			}
		}

		private void SetPosition(float newPos)
		{
			_pos = newPos;
		}

		private void SetLinearVelocity(float newVel)
		{
			_vel = newVel;
		}

		private void Integrate(float dt)
		{
			float position = _pos + dt * _vel;
			SetPosition(position);
		}

		private static float CalculateRequiredImpulse(RigidBody1d bodyA, RigidBody1d bodyB, ref float currVelR, ref float impulseDir, float wantVelR, float impulseScale)
		{
			float num = wantVelR - currVelR * impulseDir;
			float num2 = bodyA._invMass + bodyB._invMass;
			return impulseScale * num / num2;
		}
	}
}
