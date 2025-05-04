using System;
using UnityEngine;

namespace UnitySampleAssets.Vehicles.Aeroplane
{
	public class AeroplaneAudio : MonoBehaviour
	{
		[Serializable]
		public class AdvancedSetttings
		{
			public float engineMinDistance = 50f;

			public float engineMaxDistance = 1000f;

			public float engineDopplerLevel = 1f;

			[Range(0f, 1f)]
			public float engineMasterVolume = 0.5f;

			public float windMinDistance = 10f;

			public float windMaxDistance = 100f;

			public float windDopplerLevel = 1f;

			[Range(0f, 1f)]
			public float windMasterVolume = 0.5f;
		}

		[SerializeField]
		private AudioClip engineSound;

		[SerializeField]
		private float engineMinThrottlePitch = 0.4f;

		[SerializeField]
		private float engineMaxThrottlePitch = 2f;

		[SerializeField]
		private float engineFwdSpeedMultiplier = 0.002f;

		[SerializeField]
		private AudioClip windSound;

		[SerializeField]
		private float windBasePitch = 0.2f;

		[SerializeField]
		private float windSpeedPitchFactor = 0.004f;

		[SerializeField]
		private float windMaxSpeedVolume = 100f;

		private AudioSource engineSoundSource;

		private AudioSource windSoundSource;

		private AeroplaneController plane;

		[SerializeField]
		private AdvancedSetttings advanced = new AdvancedSetttings();

		private void Awake()
		{
			plane = GetComponent<AeroplaneController>();
			engineSoundSource = base.gameObject.AddComponent<AudioSource>();
			engineSoundSource.playOnAwake = false;
			windSoundSource = base.gameObject.AddComponent<AudioSource>();
			windSoundSource.playOnAwake = false;
			engineSoundSource.clip = engineSound;
			windSoundSource.clip = windSound;
			engineSoundSource.minDistance = advanced.engineMinDistance;
			engineSoundSource.maxDistance = advanced.engineMaxDistance;
			engineSoundSource.loop = true;
			engineSoundSource.dopplerLevel = advanced.engineDopplerLevel;
			windSoundSource.minDistance = advanced.windMinDistance;
			windSoundSource.maxDistance = advanced.windMaxDistance;
			windSoundSource.loop = true;
			windSoundSource.dopplerLevel = advanced.windDopplerLevel;
			Update();
			engineSoundSource.Play();
			windSoundSource.Play();
		}

		private void Update()
		{
			float t = Mathf.InverseLerp(0f, plane.MaxEnginePower, plane.EnginePower);
			engineSoundSource.pitch = Mathf.Lerp(engineMinThrottlePitch, engineMaxThrottlePitch, t);
			engineSoundSource.pitch += plane.ForwardSpeed * engineFwdSpeedMultiplier;
			engineSoundSource.volume = Mathf.InverseLerp(0f, plane.MaxEnginePower * advanced.engineMasterVolume, plane.EnginePower);
			float magnitude = GetComponent<Rigidbody>().velocity.magnitude;
			windSoundSource.pitch = windBasePitch + magnitude * windSpeedPitchFactor;
			windSoundSource.volume = Mathf.InverseLerp(0f, windMaxSpeedVolume, magnitude) * advanced.windMasterVolume;
		}
	}
}
