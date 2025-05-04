using System;
using UnityEngine;

namespace GameDefines
{
	public static class GlobalDefines
	{
		public class LeaderboardDefines
		{
			public const string CUMULATIVE_STUDS_LEADERBOARD = "CumulativeStuds";
		}

		public const string AMUZO_TOOLS_MENU_PREFIX = "Amuzo Tools/";

		public const int INVALID_INT = 9898989;

		public const int INVALID_ID = -1;

		public const int INVALID_INDEX = -1;

		public const int DEBUG_TEXT_HIEGHT = 25;

		public const uint INVALID_UID = 9898989u;

		public const float INVALID_FLOAT = 98989896f;

		public const float QUARTER = 0.25f;

		public const float HALF = 0.5f;

		public const float THREE_QUARTERS = 0.75f;

		public const float THIRD = 1f / 3f;

		public const float TWO_THIRDS = 2f / 3f;

		public const bool ENABLE_CHEATS = true;

		public const string TAG_IGNORE_RAYCAST = "IgnoreRaycast";

		private const int MEMORY_THRESHOLD_FOR_LOW_RES = 1050;

		public static readonly string DEFAULT_LANGUAGE = "EN";

		public static readonly string EMPTY_STRING = string.Empty;

		public static readonly string INVALID_STRING = "INVALID";

		public static readonly string UNITY_PREFAB_SUFFIX = "(Clone)";

		public static readonly Vector2 INVALID_VECTOR_2 = new Vector2(9.666f, 9.666f);

		public static readonly Vector3 INVALID_VECTOR_3 = new Vector3(9.666f, 9.666f, 9.666f);

		public static readonly int ANIM_LAYERINDEX_BASE = 0;

		public static bool _isMobile = true;

		public static bool _doUseInternetReachabilityVerifier = true;

		public static bool _killSwitchRaised = false;

		public static bool _minVersionWrong = false;

		private static bool _lowMemGameplay = false;

		private static bool _isMute = false;

		private static string _urlPrivacy = INVALID_STRING;

		private static string _urlCookies = INVALID_STRING;

		private static string _exitAppMessage = INVALID_STRING;

		private static bool _hasSetLowResTexturesFlag;

		private static bool _doUseLowResTextures;

		public static bool _pUseLowMemGameplay
		{
			get
			{
				return _lowMemGameplay;
			}
			set
			{
				_lowMemGameplay = value;
			}
		}

		public static bool _pIsMute
		{
			get
			{
				return _isMute;
			}
			set
			{
				_isMute = value;
				AudioListener.volume = ((!_isMute) ? 1f : 0f);
				SoundFacade._pInstance._pMusicMuted = _isMute;
			}
		}

		public static string _pUrlPrivacy
		{
			get
			{
				if (_urlPrivacy == INVALID_STRING)
				{
					return Facades<LocalisationFacade>.Instance.GetString("Url.PrivacyPolicyURL");
				}
				return _urlPrivacy;
			}
			set
			{
				_urlPrivacy = value;
			}
		}

		public static string _pUrlCookies
		{
			get
			{
				if (_urlCookies == INVALID_STRING)
				{
					return Facades<LocalisationFacade>.Instance.GetString("Url.CookiePolicyURL");
				}
				return _urlCookies;
			}
			set
			{
				_urlCookies = value;
			}
		}

		public static string _pExitAppMessage
		{
			get
			{
				if (_exitAppMessage == null || _exitAppMessage == INVALID_STRING)
				{
					if (_killSwitchRaised)
					{
						return Facades<LocalisationFacade>.Instance.GetString("VersionCheck.Killswitch");
					}
					return Facades<LocalisationFacade>.Instance.GetString("VersionCheck.MinVersion");
				}
				return _exitAppMessage;
			}
			set
			{
				_exitAppMessage = value;
			}
		}

		public static string _pAmuzoDeviceUniqueId
		{
			get
			{
				return SystemInfo.deviceUniqueIdentifier;
			}
		}

		public static bool _pUseLegoSDK
		{
			get
			{
				return _isMobile && !Application.isEditor && LEGOSDKAmuzo._pInstance != null;
			}
		}

		public static bool _pDoUseLowResTextures
		{
			get
			{
				if (!_hasSetLowResTexturesFlag)
				{
					_doUseLowResTextures = SystemInfo.systemMemorySize < 1050;
					_hasSetLowResTexturesFlag = true;
				}
				return _doUseLowResTextures;
			}
		}

		public static bool IsApproximately(float a, float b, float tolerance = 0.01f)
		{
			return Mathf.Abs(a - b) < tolerance;
		}

		public static bool IsApproximately(Vector3 a, Vector3 b, float tolerance = 0.01f)
		{
			return Mathf.Abs(a.x - b.x) < tolerance && Mathf.Abs(a.y - b.y) < tolerance && Mathf.Abs(a.z - b.z) < tolerance;
		}

		public static bool IsValidString(string toCheck)
		{
			return !(toCheck == INVALID_STRING);
		}

		public static bool IsValidInt(int toCheck)
		{
			return toCheck != 9898989;
		}

		public static bool IsValidId(int toCheck)
		{
			return toCheck != -1;
		}

		public static bool IsValidIndex(int toCheck)
		{
			return toCheck != -1;
		}

		public static bool IsValidFloat(float toCheck)
		{
			return toCheck != 98989896f;
		}

		public static T ParseEnum<T>(string value)
		{
			try
			{
				return (T)Enum.Parse(typeof(T), value, true);
			}
			catch (Exception ex)
			{
				Debug.LogError("GlobalDefines::ParseEnum - value passed is not a valid enum type. Value: " + value + ", Enum: " + typeof(T).ToString() + ", Exeption: " + ex.ToString());
				throw;
			}
		}
	}
}
