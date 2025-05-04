using UnityEngine;

namespace AmuzoPhysics
{
	public class RigidBody2d
	{
		private const string LOG_TAG = "[RigidBody2d] ";

		private float _mass;

		private float _invMass;

		private float _angMass;

		private float _invAngMass;

		private Transform2d _transform = default(Transform2d);

		private Vector2 _linVel;

		private float _angVel;

		private float _linLambda;

		private float _angLambda;

		private Vector2 _pos
		{
			get
			{
				return _transform.Position;
			}
			set
			{
				_transform.Position = value;
			}
		}

		private float _rot
		{
			get
			{
				return _transform.Rotation;
			}
			set
			{
				_transform.Rotation = value;
			}
		}

		public float _pMass
		{
			get
			{
				return _mass;
			}
		}

		public float _pAngMass
		{
			get
			{
				return _angMass;
			}
		}

		public Transform2d _pTransform
		{
			get
			{
				return _transform;
			}
			set
			{
				_transform = value;
			}
		}

		public Vector2 _pPosition
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

		public float _pRotation
		{
			get
			{
				return _rot;
			}
			set
			{
				SetRotation(value);
			}
		}

		public Vector2 _pVelocity
		{
			get
			{
				return _linVel;
			}
			set
			{
				SetLinearVelocity(value);
			}
		}

		public float _pAngularVelocity
		{
			get
			{
				return _angVel;
			}
			set
			{
				SetAngularVelocity(value);
			}
		}

		public float _pLinearLambda
		{
			get
			{
				return _linLambda;
			}
			set
			{
				_linLambda = value;
			}
		}

		public float _pAngularLambda
		{
			get
			{
				return _angLambda;
			}
			set
			{
				_angLambda = value;
			}
		}

		public static void DoCollisionResponse(RigidBody2d bodyA, RigidBody2d bodyB, Vector2 contactNorm, Vector2 contactPos, float contactPen, float bounce, float friction)
		{
			if (!(contactPen < 0f))
			{
				float num = bodyA._invMass / (bodyA._invMass + bodyB._invMass) * contactPen;
				float num2 = contactPen - num;
				bodyA._pPosition += num * contactNorm;
				bodyB._pPosition -= num2 * contactNorm;
				contactPos -= num2 * contactNorm;
				Vector2 velocityAtPosition = bodyA.GetVelocityAtPosition(contactPos);
				Vector2 velocityAtPosition2 = bodyB.GetVelocityAtPosition(contactPos);
				Vector2 currVelR = velocityAtPosition - velocityAtPosition2;
				float num3 = Vector2.Dot(currVelR, contactNorm);
				if (!(num3 >= 0f))
				{
					float num4 = CalculateRequiredImpulse(bodyA, bodyB, ref currVelR, ref contactNorm, ref contactPos, (0f - bounce) * num3, 1f);
					Vector2 vector = num4 * contactNorm;
					bodyA.ApplyImpulseAtPosition(vector, contactPos);
					bodyB.ApplyImpulseAtPosition(-vector, contactPos);
					velocityAtPosition = bodyA.GetVelocityAtPosition(contactPos);
					velocityAtPosition2 = bodyB.GetVelocityAtPosition(contactPos);
					currVelR = velocityAtPosition - velocityAtPosition2;
					Vector2 impulseDir = -MathHelper.ClipVector2(currVelR, contactNorm).normalized;
					float value = CalculateRequiredImpulse(bodyA, bodyB, ref currVelR, ref impulseDir, ref contactPos, 0f, 1f);
					float num5 = Mathf.Abs(friction * num4);
					value = Mathf.Clamp(value, 0f - num5, num5);
					Vector2 vector2 = value * impulseDir;
					bodyA.ApplyImpulseAtPosition(vector2, contactPos);
					bodyB.ApplyImpulseAtPosition(-vector2, contactPos);
				}
			}
		}

		public static void DoCollisionResponse2(RigidBody2d bodyA, RigidBody2d bodyB, Vector2 contactNorm, Vector2[] contactPoints, int numContactPoints, float contactPen, float bounce, float friction)
		{
			if (numContactPoints <= 0 || contactPen < 0f)
			{
				return;
			}
			float num = bodyA._invMass / (bodyA._invMass + bodyB._invMass) * contactPen;
			float num2 = contactPen - num;
			bodyA._pPosition += num * contactNorm;
			bodyB._pPosition -= num2 * contactNorm;
			float impulseScale = 1f / (float)numContactPoints;
			for (int i = 0; i < numContactPoints; i++)
			{
				contactPoints[i] -= num2 * contactNorm;
				Vector2 velocityAtPosition = bodyA.GetVelocityAtPosition(contactPoints[i]);
				Vector2 velocityAtPosition2 = bodyB.GetVelocityAtPosition(contactPoints[i]);
				Vector2 currVelR = velocityAtPosition - velocityAtPosition2;
				float num3 = Vector2.Dot(currVelR, contactNorm);
				if (num3 < 0f)
				{
					float num4 = CalculateRequiredImpulse(bodyA, bodyB, ref currVelR, ref contactNorm, ref contactPoints[i], (0f - bounce) * num3, impulseScale);
					Vector2 vector = num4 * contactNorm;
					bodyA.ApplyImpulseAtPosition(vector, contactPoints[i]);
					bodyB.ApplyImpulseAtPosition(-vector, contactPoints[i]);
					velocityAtPosition = bodyA.GetVelocityAtPosition(contactPoints[i]);
					velocityAtPosition2 = bodyB.GetVelocityAtPosition(contactPoints[i]);
					currVelR = velocityAtPosition - velocityAtPosition2;
					Vector2 impulseDir = -MathHelper.ClipVector2(currVelR, contactNorm).normalized;
					float value = CalculateRequiredImpulse(bodyA, bodyB, ref currVelR, ref impulseDir, ref contactPoints[i], 0f, impulseScale);
					float num5 = Mathf.Abs(friction * num4);
					value = Mathf.Clamp(value, 0f - num5, num5);
					Vector2 vector2 = value * impulseDir;
					bodyA.ApplyImpulseAtPosition(vector2, contactPoints[i]);
					bodyB.ApplyImpulseAtPosition(-vector2, contactPoints[i]);
				}
			}
		}

		public void SetPointMass(float mass)
		{
			SetMass(mass);
			SetAngularMass(0f);
		}

		public void SetSphereMass(float mass, float radius)
		{
			SetMass(mass);
			float angularMass = 0.4f * mass * radius * radius;
			SetAngularMass(angularMass);
		}

		public void SetBoxMass(float mass, float width, float height)
		{
			SetMass(mass);
			float angularMass = 1f / 12f * mass * (width * width + height * height);
			SetAngularMass(angularMass);
		}

		public void ApplyAccel(Vector2 accel, float dt)
		{
			SetLinearVelocity(_linVel + dt * accel);
		}

		public void ApplyForce(Vector2 force, float dt)
		{
			SetLinearVelocity(_linVel + dt * _invMass * force);
		}

		public void ApplyForceAtPosition(Vector2 force, Vector2 forcePos, float dt)
		{
			SetLinearVelocity(_linVel + dt * _invMass * force);
			SetAngularVelocity(_angVel + dt * _invAngMass * MathHelper.Vector2Cross(forcePos - _pos, force));
		}

		public void ApplyImpulse(Vector2 impulse)
		{
			SetLinearVelocity(_linVel + _invMass * impulse);
		}

		public void ApplyImpulseAtPosition(Vector2 impulse, Vector2 impulsePos)
		{
			SetLinearVelocity(_linVel + _invMass * impulse);
			SetAngularVelocity(_angVel + _invAngMass * MathHelper.Vector2Cross(impulsePos - _pos, impulse));
		}

		public void ApplyAngularAccel(float accel, float dt)
		{
			SetAngularVelocity(_angVel + dt * accel);
		}

		public void ApplyTorque(float torque, float dt)
		{
			SetAngularVelocity(_angVel + dt * _invAngMass * torque);
		}

		public void ApplyAngularImpulse(float impulse)
		{
			SetAngularVelocity(_angVel + _invAngMass * impulse);
		}

		public Vector2 GetVelocityAtPosition(Vector2 velocityPos)
		{
			return _linVel + _angVel * MathHelper.Vector2Perp(velocityPos - _pos);
		}

		public void Update(float dt)
		{
			if (_linLambda > 0f)
			{
				SetLinearVelocity(_linVel * 1f / Mathf.Pow(2f, dt / _linLambda));
			}
			if (_angLambda > 0f)
			{
				SetAngularVelocity(_angVel * 1f / Mathf.Pow(2f, dt / _angLambda));
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

		private void SetAngularMass(float angMass)
		{
			if (angMass < 0f)
			{
				angMass = 0f;
			}
			_angMass = angMass;
			if (angMass == 0f)
			{
				_invAngMass = 0f;
			}
			else
			{
				_invAngMass = 1f / angMass;
			}
		}

		private void SetPosition(Vector2 newPos)
		{
			_pos = newPos;
		}

		private void SetRotation(float newRot)
		{
			_rot = WrapAngle(newRot);
		}

		private void SetLinearVelocity(Vector2 newVel)
		{
			_linVel = newVel;
		}

		private void SetAngularVelocity(float newVel)
		{
			_angVel = newVel;
		}

		private static float WrapAngle(float angle)
		{
			while (angle > 180f)
			{
				angle -= 360f;
			}
			while (angle < -180f)
			{
				angle += 360f;
			}
			return angle;
		}

		private void Integrate(float dt)
		{
			Vector2 position = _pos + dt * _linVel;
			SetPosition(position);
			float rotation = _rot + dt * _angVel;
			SetRotation(rotation);
		}

		private static float CalculateRequiredImpulse(RigidBody2d bodyA, RigidBody2d bodyB, ref Vector2 currVelR, ref Vector2 impulseDir, ref Vector2 impulsePos, float wantVelR, float impulseScale)
		{
			float num = wantVelR - Vector2.Dot(currVelR, impulseDir);
			Vector2 vector = bodyA._pPosition - impulsePos;
			Vector2 vector2 = bodyB._pPosition - impulsePos;
			float num2 = vector.x * impulseDir.y - vector.y * impulseDir.x;
			float num3 = bodyA._invAngMass * num2;
			float num4 = vector2.x * impulseDir.y - vector2.y * impulseDir.x;
			float num5 = bodyB._invAngMass * num4;
			float num6 = bodyA._invMass + bodyB._invMass + num2 * num3 + num4 * num5;
			return impulseScale * num / num6;
		}
	}
}
