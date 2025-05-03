using UnityEngine;

namespace AmuzoPhysics
{
	public class RigidBody3d
	{
		private float _mass;

		private float _invMass;

		private Vector3 _angMass;

		private Vector3 _invAngMass;

		private Vector3 _pos;

		private Quaternion _rot;

		private Quaternion _invRot;

		private Vector3 _linVel;

		private Vector3 _angVel;

		private float _linLambda;

		private float _angLambda;

		public float Mass
		{
			get
			{
				return _mass;
			}
		}

		public Vector3 Position
		{
			get
			{
				return _pos;
			}
			set
			{
				_SetPosition(value);
			}
		}

		public Quaternion Rotation
		{
			get
			{
				return _rot;
			}
			set
			{
				_SetRotation(value);
			}
		}

		public Vector3 Velocity
		{
			get
			{
				return _linVel;
			}
			set
			{
				_SetLinearVelocity(value);
			}
		}

		public Vector3 AngularVelocity
		{
			get
			{
				return _angVel;
			}
			set
			{
				_SetAngularVelocity(value);
			}
		}

		public float LinearLambda
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

		public float AngularLambda
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

		private void _SetMass(float mass)
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

		private void _SetAngularMass(Vector3 angMass)
		{
			if (angMass.x < 0f)
			{
				angMass.x = 0f;
			}
			_angMass.x = angMass.x;
			if (angMass.x == 0f)
			{
				_invAngMass.x = 0f;
			}
			else
			{
				_invAngMass.x = 1f / angMass.x;
			}
			if (angMass.y < 0f)
			{
				angMass.y = 0f;
			}
			_angMass.y = angMass.y;
			if (angMass.y == 0f)
			{
				_invAngMass.y = 0f;
			}
			else
			{
				_invAngMass.y = 1f / angMass.y;
			}
			if (angMass.z < 0f)
			{
				angMass.z = 0f;
			}
			_angMass.z = angMass.z;
			if (angMass.z == 0f)
			{
				_invAngMass.z = 0f;
			}
			else
			{
				_invAngMass.z = 1f / angMass.z;
			}
		}

		public void SetInfiniteMass()
		{
			_SetMass(0f);
			_SetAngularMass(Vector3.zero);
		}

		public void SetSphereMass(float mass, float radius)
		{
			_SetMass(mass);
			float num = 0.4f * mass * radius * radius;
			Vector3 angMass = new Vector3(num, num, num);
			_SetAngularMass(angMass);
		}

		public void SetCylinderMass(float mass, float radius, float height)
		{
			_SetMass(mass);
			float num = mass / 12f;
			Vector3 angMass = new Vector3(num * (3f * radius * radius + height * height), num * (3f * radius * radius + height * height), 0.5f * mass * radius * radius);
			_SetAngularMass(angMass);
		}

		public void SetBoxMass(float mass, float width, float height, float depth)
		{
			_SetMass(mass);
			float num = mass / 12f;
			Vector3 angMass = new Vector3(num * (height * height + depth * depth), num * (depth * depth + width * width), num * (width * width + height * height));
			_SetAngularMass(angMass);
		}

		private void _SetPosition(Vector3 newPos)
		{
			_pos = newPos;
		}

		private void _SetRotation(Quaternion newRot)
		{
			_rot = newRot;
			_invRot = Quaternion.Inverse(newRot);
		}

		private void _SetLinearVelocity(Vector3 newVel)
		{
			_linVel = newVel;
		}

		private void _SetAngularVelocity(Vector3 newVel)
		{
			_angVel = newVel;
		}

		public void ApplyAccel(Vector3 accel, float dt)
		{
			_SetLinearVelocity(_linVel + dt * accel);
		}

		public void ApplyForce(Vector3 force, float dt)
		{
			_SetLinearVelocity(_linVel + dt * _invMass * force);
		}

		public void ApplyForceAtPosition(Vector3 force, Vector3 forcePos, float dt)
		{
			_SetLinearVelocity(_linVel + dt * _invMass * force);
			_SetAngularVelocity(_angVel + dt * (_rot * Vector3.Scale(_invAngMass, _invRot * Vector3.Cross(forcePos - _pos, force))));
		}

		public void ApplyImpulse(Vector3 impulse)
		{
			_SetLinearVelocity(_linVel + _invMass * impulse);
		}

		public void ApplyImpulseAtPosition(Vector3 impulse, Vector3 impulsePos)
		{
			_SetLinearVelocity(_linVel + _invMass * impulse);
			_SetAngularVelocity(_angVel + _rot * Vector3.Scale(_invAngMass, _invRot * Vector3.Cross(impulsePos - _pos, impulse)));
		}

		public void ApplyAngularAccel(Vector3 accel, float dt)
		{
			_SetAngularVelocity(_angVel + dt * accel);
		}

		public void ApplyTorque(Vector3 torque, float dt)
		{
			_SetAngularVelocity(_angVel + dt * (_rot * Vector3.Scale(_invAngMass, _invRot * torque)));
		}

		public void ApplyAngularImpulse(Vector3 impulse)
		{
			_SetAngularVelocity(_angVel + _rot * Vector3.Scale(_invAngMass, _invRot * impulse));
		}

		public Vector3 GetVelocityAtPosition(Vector3 velocityPos)
		{
			return _linVel + Vector3.Cross(_angVel, velocityPos - _pos);
		}

		private void _Integrate(float dt)
		{
			Vector3 newPos = _pos + dt * _linVel;
			_SetPosition(newPos);
			float magnitude = _angVel.magnitude;
			if (magnitude > 0f)
			{
				Quaternion newRot = Quaternion.AngleAxis(dt * magnitude * 57.29578f, 1f / magnitude * _angVel) * _rot;
				_SetRotation(newRot);
			}
		}

		public void Update(float dt)
		{
			if (_linLambda > 0f)
			{
				_SetLinearVelocity(_linVel * 1f / Mathf.Pow(2f, dt / _linLambda));
			}
			if (_angLambda > 0f)
			{
				_SetAngularVelocity(_angVel * 1f / Mathf.Pow(2f, dt / _angLambda));
			}
			_Integrate(dt);
		}

		private static float _CalculateRequiredImpulse(RigidBody3d bodyA, RigidBody3d bodyB, ref Vector3 currVelR, ref Vector3 impulseDir, ref Vector3 impulsePos, float wantVelR, float impulseScale)
		{
			float num = wantVelR - Vector3.Dot(currVelR, impulseDir);
			Vector3 lhs = bodyA.Position - impulsePos;
			Vector3 lhs2 = bodyB.Position - impulsePos;
			Vector3 vector = Vector3.Cross(lhs, impulseDir);
			Vector3 b = bodyA._invRot * vector;
			b = Vector3.Scale(bodyA._invAngMass, b);
			b = bodyA._rot * b;
			Vector3 vector2 = Vector3.Cross(lhs2, impulseDir);
			Vector3 b2 = bodyB._invRot * vector2;
			b2 = Vector3.Scale(bodyB._invAngMass, b2);
			b2 = bodyB._rot * b2;
			float num2 = bodyA._invMass + bodyB._invMass + Vector3.Dot(vector, b) + Vector3.Dot(vector2, b2);
			return impulseScale * num / num2;
		}

		public static void DoCollisionResponse(RigidBody3d bodyA, RigidBody3d bodyB, Vector3 contactNorm, Vector3 contactPos, float contactPen, float bounce, float friction)
		{
			if (contactPen >= 0f)
			{
				float num = bodyA._invMass / (bodyA._invMass + bodyB._invMass) * contactPen;
				float num2 = contactPen - num;
				bodyA.Position += num * contactNorm;
				bodyB.Position -= num2 * contactNorm;
				contactPos -= num2 * contactNorm;
				Vector3 velocityAtPosition = bodyA.GetVelocityAtPosition(contactPos);
				Vector3 velocityAtPosition2 = bodyB.GetVelocityAtPosition(contactPos);
				Vector3 currVelR = velocityAtPosition - velocityAtPosition2;
				float num3 = Vector3.Dot(currVelR, contactNorm);
				if (num3 < 0f)
				{
					float num4 = _CalculateRequiredImpulse(bodyA, bodyB, ref currVelR, ref contactNorm, ref contactPos, (0f - bounce) * num3, 1f);
					Vector3 vector = num4 * contactNorm;
					bodyA.ApplyImpulseAtPosition(vector, contactPos);
					bodyB.ApplyImpulseAtPosition(-vector, contactPos);
					velocityAtPosition = bodyA.GetVelocityAtPosition(contactPos);
					velocityAtPosition2 = bodyB.GetVelocityAtPosition(contactPos);
					currVelR = velocityAtPosition - velocityAtPosition2;
					Vector3 impulseDir = -MathHelper.ClipVector3(currVelR, contactNorm).normalized;
					float value = _CalculateRequiredImpulse(bodyA, bodyB, ref currVelR, ref impulseDir, ref contactPos, 0f, 1f);
					float num5 = Mathf.Abs(friction * num4);
					value = Mathf.Clamp(value, 0f - num5, num5);
					Vector3 vector2 = value * impulseDir;
					bodyA.ApplyImpulseAtPosition(vector2, contactPos);
					bodyB.ApplyImpulseAtPosition(-vector2, contactPos);
				}
			}
		}

		public static void DoCollisionResponse2(RigidBody3d bodyA, RigidBody3d bodyB, Vector3 contactNorm, Vector3[] contactPoints, int numContactPoints, float contactPen, float bounce, float friction)
		{
			if (numContactPoints <= 0 || !(contactPen >= 0f))
			{
				return;
			}
			float num = bodyA._invMass / (bodyA._invMass + bodyB._invMass) * contactPen;
			float num2 = contactPen - num;
			bodyA.Position += num * contactNorm;
			bodyB.Position -= num2 * contactNorm;
			float impulseScale = 1f / (float)numContactPoints;
			for (int i = 0; i < numContactPoints; i++)
			{
				contactPoints[i] -= num2 * contactNorm;
				Vector3 velocityAtPosition = bodyA.GetVelocityAtPosition(contactPoints[i]);
				Vector3 velocityAtPosition2 = bodyB.GetVelocityAtPosition(contactPoints[i]);
				Vector3 currVelR = velocityAtPosition - velocityAtPosition2;
				float num3 = Vector3.Dot(currVelR, contactNorm);
				if (num3 < 0f)
				{
					float num4 = _CalculateRequiredImpulse(bodyA, bodyB, ref currVelR, ref contactNorm, ref contactPoints[i], (0f - bounce) * num3, impulseScale);
					Vector3 vector = num4 * contactNorm;
					bodyA.ApplyImpulseAtPosition(vector, contactPoints[i]);
					bodyB.ApplyImpulseAtPosition(-vector, contactPoints[i]);
					velocityAtPosition = bodyA.GetVelocityAtPosition(contactPoints[i]);
					velocityAtPosition2 = bodyB.GetVelocityAtPosition(contactPoints[i]);
					currVelR = velocityAtPosition - velocityAtPosition2;
					Vector3 impulseDir = -MathHelper.ClipVector3(currVelR, contactNorm).normalized;
					float value = _CalculateRequiredImpulse(bodyA, bodyB, ref currVelR, ref impulseDir, ref contactPoints[i], 0f, impulseScale);
					float num5 = Mathf.Abs(friction * num4);
					value = Mathf.Clamp(value, 0f - num5, num5);
					Vector3 vector2 = value * impulseDir;
					bodyA.ApplyImpulseAtPosition(vector2, contactPoints[i]);
					bodyB.ApplyImpulseAtPosition(-vector2, contactPoints[i]);
				}
			}
		}
	}
}
