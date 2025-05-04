using System;
using UnityEngine;

namespace UnitySampleAssets.Vehicles.Aeroplane
{
	[RequireComponent(typeof(ParticleSystem))]
	public class JetParticleEffect : MonoBehaviour
	{
		public Color minColour;

		private AeroplaneController jet;

		private ParticleSystem system;

		private float originalStartSize;

		private float originalLifetime;

		private Color originalStartColor;

		private void Start()
		{
			jet = FindAeroplaneParent();
			system = GetComponent<ParticleSystem>();
			originalLifetime = system.startLifetime;
			originalStartSize = system.startSize;
			originalStartColor = system.startColor;
		}

		private void Update()
		{
			system.startLifetime = Mathf.Lerp(0f, originalLifetime, jet.Throttle);
			system.startSize = Mathf.Lerp(originalStartSize * 0.3f, originalStartSize, jet.Throttle);
			system.startColor = Color.Lerp(minColour, originalStartColor, jet.Throttle);
		}

		private AeroplaneController FindAeroplaneParent()
		{
			Transform parent = base.transform;
			while (parent != null)
			{
				AeroplaneController component = parent.GetComponent<AeroplaneController>();
				if (component == null)
				{
					parent = parent.parent;
					continue;
				}
				return component;
			}
			throw new Exception(" AeroplaneContoller not found in object hierarchy");
		}
	}
}
