using System;
using UnityEngine;

namespace UnitySampleAssets.Utility
{
	public class WaypointCircuit : MonoBehaviour
	{
		[Serializable]
		public class WaypointList
		{
			public WaypointCircuit circuit;

			public Transform[] items = new Transform[0];
		}

		public struct RoutePoint
		{
			public Vector3 position;

			public Vector3 direction;

			public RoutePoint(Vector3 position, Vector3 direction)
			{
				this.position = position;
				this.direction = direction;
			}
		}

		public WaypointList waypointList = new WaypointList();

		[SerializeField]
		private bool smoothRoute = true;

		private int numPoints;

		private Vector3[] points;

		private float[] distances;

		public float editorVisualisationSubsteps = 100f;

		private int p0n;

		private int p1n;

		private int p2n;

		private int p3n;

		private float i;

		private Vector3 P0;

		private Vector3 P1;

		private Vector3 P2;

		private Vector3 P3;

		public float Length { get; private set; }

		public Transform[] Waypoints
		{
			get
			{
				return waypointList.items;
			}
		}

		private void Awake()
		{
			if (Waypoints.Length > 1)
			{
				CachePositionsAndDistances();
			}
			numPoints = Waypoints.Length;
		}

		public RoutePoint GetRoutePoint(float dist)
		{
			Vector3 routePosition = GetRoutePosition(dist);
			Vector3 routePosition2 = GetRoutePosition(dist + 0.1f);
			return new RoutePoint(routePosition, (routePosition2 - routePosition).normalized);
		}

		public Vector3 GetRoutePosition(float dist)
		{
			int i = 0;
			if (Length == 0f)
			{
				Length = distances[distances.Length - 1];
			}
			for (dist = Mathf.Repeat(dist, Length); distances[i] < dist; i++)
			{
			}
			p1n = (i - 1 + numPoints) % numPoints;
			p2n = i;
			this.i = Mathf.InverseLerp(distances[p1n], distances[p2n], dist);
			if (smoothRoute)
			{
				p0n = (i - 2 + numPoints) % numPoints;
				p3n = (i + 1) % numPoints;
				p2n %= numPoints;
				P0 = points[p0n];
				P1 = points[p1n];
				P2 = points[p2n];
				P3 = points[p3n];
				return CatmullRom(P0, P1, P2, P3, this.i);
			}
			p1n = (i - 1 + numPoints) % numPoints;
			p2n = i;
			return Vector3.Lerp(points[p1n], points[p2n], this.i);
		}

		private Vector3 CatmullRom(Vector3 _P0, Vector3 _P1, Vector3 _P2, Vector3 _P3, float _i)
		{
			return 0.5f * (2f * _P1 + (-_P0 + _P2) * _i + (2f * _P0 - 5f * _P1 + 4f * _P2 - _P3) * _i * _i + (-_P0 + 3f * _P1 - 3f * _P2 + _P3) * _i * _i * _i);
		}

		private void CachePositionsAndDistances()
		{
			points = new Vector3[Waypoints.Length + 1];
			distances = new float[Waypoints.Length + 1];
			float num = 0f;
			for (int i = 0; i < points.Length; i++)
			{
				Transform transform = Waypoints[i % Waypoints.Length];
				Transform transform2 = Waypoints[(i + 1) % Waypoints.Length];
				if (transform != null && transform2 != null)
				{
					Vector3 position = transform.position;
					Vector3 position2 = transform2.position;
					points[i] = Waypoints[i % Waypoints.Length].position;
					distances[i] = num;
					num += (position - position2).magnitude;
				}
			}
		}

		private void OnDrawGizmos()
		{
			DrawGizmos(false);
		}

		private void OnDrawGizmosSelected()
		{
			DrawGizmos(true);
		}

		private void DrawGizmos(bool selected)
		{
			waypointList.circuit = this;
			if (Waypoints.Length <= 1)
			{
				return;
			}
			numPoints = Waypoints.Length;
			CachePositionsAndDistances();
			Length = distances[distances.Length - 1];
			Gizmos.color = ((!selected) ? new Color(1f, 1f, 0f, 0.5f) : Color.yellow);
			Vector3 vector = Waypoints[0].position;
			if (smoothRoute)
			{
				for (float num = 0f; num < Length; num += Length / editorVisualisationSubsteps)
				{
					Vector3 routePosition = GetRoutePosition(num + 1f);
					Gizmos.DrawLine(vector, routePosition);
					vector = routePosition;
				}
				Gizmos.DrawLine(vector, Waypoints[0].position);
			}
			else
			{
				for (int i = 0; i < Waypoints.Length; i++)
				{
					Vector3 position = Waypoints[(i + 1) % Waypoints.Length].position;
					Gizmos.DrawLine(vector, position);
					vector = position;
				}
			}
		}
	}
}
