using System;
using UnityEngine;
using UnityEngine.Events;

namespace CodeStage.AntiCheat.Detectors
{
	[AddComponentMenu("Code Stage/Anti-Cheat Toolkit/Speed Hack Detector")]
	public class SpeedHackDetector : ActDetectorBase
	{
		internal const string COMPONENT_NAME = "Speed Hack Detector";

		internal const string FINAL_LOG_PREFIX = "[ACTk] Speed Hack Detector: ";

		private const long TICKS_PER_SECOND = 10000000L;

		private const int THRESHOLD = 5000000;

		private static int instancesInScene;

		[Tooltip("Time (in seconds) between detector checks.")]
		public float interval = 1f;

		[Tooltip("Maximum false positives count allowed before registering speed hack.")]
		public byte maxFalsePositives = 3;

		[Tooltip("Amount of sequential successful checks before clearing internal false positives counter.\nSet 0 to disable Cool Down feature.")]
		public int coolDown = 30;

		private byte currentFalsePositives;

		private int currentCooldownShots;

		private long ticksOnStart;

		private long vulnerableTicksOnStart;

		private long prevTicks;

		private long prevIntervalTicks;

		public static SpeedHackDetector Instance { get; private set; }

		private static SpeedHackDetector GetOrCreateInstance
		{
			get
			{
				if (Instance != null)
				{
					return Instance;
				}
				if (ActDetectorBase.detectorsContainer == null)
				{
					ActDetectorBase.detectorsContainer = new GameObject("Anti-Cheat Toolkit Detectors");
				}
				Instance = ActDetectorBase.detectorsContainer.AddComponent<SpeedHackDetector>();
				return Instance;
			}
		}

		private SpeedHackDetector()
		{
		}

		public static void StartDetection()
		{
			if (Instance != null)
			{
				Instance.StartDetectionInternal(null, Instance.interval, Instance.maxFalsePositives, Instance.coolDown);
			}
			else
			{
				Debug.LogError("[ACTk] Speed Hack Detector: can't be started since it doesn't exists in scene or not yet initialized!");
			}
		}

		public static void StartDetection(UnityAction callback)
		{
			StartDetection(callback, GetOrCreateInstance.interval);
		}

		public static void StartDetection(UnityAction callback, float interval)
		{
			StartDetection(callback, interval, GetOrCreateInstance.maxFalsePositives);
		}

		public static void StartDetection(UnityAction callback, float interval, byte maxFalsePositives)
		{
			StartDetection(callback, interval, maxFalsePositives, GetOrCreateInstance.coolDown);
		}

		public static void StartDetection(UnityAction callback, float interval, byte maxFalsePositives, int coolDown)
		{
			GetOrCreateInstance.StartDetectionInternal(callback, interval, maxFalsePositives, coolDown);
		}

		public static void StopDetection()
		{
			if (Instance != null)
			{
				Instance.StopDetectionInternal();
			}
		}

		public static void Dispose()
		{
			if (Instance != null)
			{
				Instance.DisposeInternal();
			}
		}

		private void Awake()
		{
			instancesInScene++;
			if (Init(Instance, "Speed Hack Detector"))
			{
				Instance = this;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			instancesInScene--;
		}

		private void OnLevelWasLoaded(int index)
		{
			if (instancesInScene < 2)
			{
				if (!keepAlive)
				{
					DisposeInternal();
				}
			}
			else if (!keepAlive && Instance != this)
			{
				DisposeInternal();
			}
		}

		private void OnApplicationPause(bool pause)
		{
			if (!pause)
			{
				ResetStartTicks();
			}
		}

		private void Update()
		{
			if (!isRunning)
			{
				return;
			}
			long num = 0L;
			num = DateTime.UtcNow.Ticks;
			long num2 = num - prevTicks;
			if (num2 < 0 || num2 > 10000000)
			{
				ResetStartTicks();
				return;
			}
			prevTicks = num;
			long num3 = (long)(interval * 10000000f);
			if (num - prevIntervalTicks < num3)
			{
				return;
			}
			long num4 = 0L;
			num4 = (long)Environment.TickCount * 10000L;
			if (Mathf.Abs(num4 - vulnerableTicksOnStart - (num - ticksOnStart)) > 5000000f)
			{
				currentFalsePositives++;
				if (currentFalsePositives > maxFalsePositives)
				{
					OnCheatingDetected();
				}
				else
				{
					currentCooldownShots = 0;
					ResetStartTicks();
				}
			}
			else if (currentFalsePositives > 0 && coolDown > 0)
			{
				currentCooldownShots++;
				if (currentCooldownShots >= coolDown)
				{
					currentFalsePositives = 0;
				}
			}
			prevIntervalTicks = num;
		}

		private void StartDetectionInternal(UnityAction callback, float checkInterval, byte falsePositives, int shotsTillCooldown)
		{
			if (isRunning)
			{
				Debug.LogWarning("[ACTk] Speed Hack Detector: already running!", this);
				return;
			}
			if (!base.enabled)
			{
				Debug.LogWarning("[ACTk] Speed Hack Detector: disabled but StartDetection still called from somewhere (see stack trace for this message)!", this);
				return;
			}
			if (callback != null && detectionEventHasListener)
			{
				Debug.LogWarning("[ACTk] Speed Hack Detector: has properly configured Detection Event in the inspector, but still get started with Action callback. Both Action and Detection Event will be called on detection. Are you sure you wish to do this?", this);
			}
			if (callback == null && !detectionEventHasListener)
			{
				Debug.LogWarning("[ACTk] Speed Hack Detector: was started without any callbacks. Please configure Detection Event in the inspector, or pass the callback Action to the StartDetection method.", this);
				base.enabled = false;
				return;
			}
			detectionAction = callback;
			interval = checkInterval;
			maxFalsePositives = falsePositives;
			coolDown = shotsTillCooldown;
			ResetStartTicks();
			currentFalsePositives = 0;
			currentCooldownShots = 0;
			started = true;
			isRunning = true;
		}

		protected override void StartDetectionAutomatically()
		{
			StartDetectionInternal(null, interval, maxFalsePositives, coolDown);
		}

		protected override void PauseDetector()
		{
			isRunning = false;
		}

		protected override void ResumeDetector()
		{
			if (detectionAction != null || detectionEventHasListener)
			{
				isRunning = true;
			}
		}

		protected override void StopDetectionInternal()
		{
			if (started)
			{
				detectionAction = null;
				started = false;
				isRunning = false;
			}
		}

		protected override void DisposeInternal()
		{
			base.DisposeInternal();
			if (Instance == this)
			{
				Instance = null;
			}
		}

		private void ResetStartTicks()
		{
			ticksOnStart = DateTime.UtcNow.Ticks;
			vulnerableTicksOnStart = (long)Environment.TickCount * 10000L;
			prevTicks = ticksOnStart;
			prevIntervalTicks = ticksOnStart;
		}
	}
}
