using System.Collections.Generic;
using UnityEngine;

public class EmoticonSystem : MonoBehaviour
{
	private class EmoticonBackLog
	{
		public float _evocationTime;

		public Transform _location;

		public EFACE_TYPE _faceType;

		public bool _isPlayer;

		public bool _isCrook;

		public VehiclePart _ownerVehiclePart;
	}

	public enum EFACE_TYPE
	{
		INVALID = 0,
		CROOK_DIZZY = 1,
		CROOK_GRINNING = 2,
		EXPLORER_DIZZY = 3,
		EXPLORER_HAPPY = 4,
		EXPLORER_OVERJOYED = 5,
		FIREMAN_DIZZY = 6,
		FIREMAN_HAPPY = 7,
		FIREMAN_OVERJOYED = 8,
		NPC_DIZZY = 9,
		NPC_HAPPY = 10,
		NPC_WORRIED = 11,
		POLICE_DETERMINED = 12,
		POLICE_DIZZY = 13,
		POLICE_HAPPY = 14,
		POLICE_OVERJOYED = 15,
		NUM_FACE_TYPES = 16
	}

	private const float DELAY_BETWEEN_FACE_DISPLAY = 0.5f;

	private const float DELAY_UNTIL_FACE_DISPLAY_DROP = 2f;

	private static EmoticonSystem _instance;

	public UIAtlas[] _atlases;

	public Emoticon[] _emoticons;

	private Dictionary<string, UIAtlas> _atlasLookup = new Dictionary<string, UIAtlas>();

	private List<EmoticonBackLog> _backlog = new List<EmoticonBackLog>();

	private float _lastFaceShowTime;

	public static void OnPlayerTriggerAreaDiscover(PlayerTrigger area)
	{
		if (MinigameManager._pInstance == null || area._type != PlayerTrigger.EType.Hidden)
		{
			return;
		}
		VehiclePart component = VehicleController_Player._pInstance.GetComponent<VehiclePart>();
		if (component != null)
		{
			Transform pFaceTransform = component._pFaceTransform;
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.POLICE)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.POLICE_OVERJOYED, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.FIRE)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.FIREMAN_OVERJOYED, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.VOLCANO)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.EXPLORER_OVERJOYED, true);
			}
		}
	}

	public static void OnPlayerHitVehicle(Vehicle other, Collision col)
	{
		if (MinigameManager._pInstance == null || MinigameController._pInstance == null)
		{
			return;
		}
		VehiclePart component = other.GetComponent<VehiclePart>();
		if ((bool)component && component.uniqueID == VehiclePart.EUNIQUE_ID.BODY_SHARK)
		{
			VehiclePart component2 = VehicleController_Player._pInstance.GetComponent<VehiclePart>();
			Transform pFaceTransform = VehicleController_Player._pInstance.transform;
			if (component2 != null)
			{
				pFaceTransform = component2._pFaceTransform;
			}
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.POLICE)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.POLICE_DIZZY, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.FIRE)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.FIREMAN_DIZZY, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.VOLCANO)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.EXPLORER_DIZZY, true);
			}
		}
		else if ((bool)component && component.uniqueID != VehiclePart.EUNIQUE_ID.BODY_SHARK && !component.GetComponent<MinigameObjective_Destroyable_Car>())
		{
			VehiclePart component3 = other.GetComponent<VehiclePart>();
			_instance.ShowFace(component3._pFaceTransform, EFACE_TYPE.NPC_DIZZY, true);
		}
	}

	public static void OnPlayerHitCrook(Transform crookTransform)
	{
		if (MinigameManager._pInstance == null || MinigameController._pInstance == null || MinigameController._pInstance._pMinigame._pHasBeenCompleted)
		{
			return;
		}
		VehiclePart component = VehicleController_Player._pInstance.GetComponent<VehiclePart>();
		if (component != null)
		{
			Transform pFaceTransform = component._pFaceTransform;
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.POLICE)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.POLICE_HAPPY, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.FIRE)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.FIREMAN_HAPPY, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.VOLCANO)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.EXPLORER_HAPPY, true);
			}
		}
		_instance.ShowFace(crookTransform, EFACE_TYPE.CROOK_DIZZY, false, true);
	}

	public static void OnPlayerEnterWaypoint(MinigameObjective_Waypoint sourceWP)
	{
		if (sourceWP == null || MinigameManager._pInstance == null || MinigameController._pInstance == null || MinigameController._pInstance._pMinigame._pHasBeenCompleted)
		{
			return;
		}
		Transform facePos = sourceWP.facePos;
		VehiclePart component = VehicleController_Player._pInstance.GetComponent<VehiclePart>();
		if (component != null)
		{
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			switch (currentMinigameData.minigameCategory)
			{
			case MinigameManager.EMINIGAME_CATEGORY.POLICE:
				_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.POLICE_HAPPY, true);
				break;
			case MinigameManager.EMINIGAME_CATEGORY.FIRE:
				_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.FIREMAN_DIZZY, true);
				break;
			case MinigameManager.EMINIGAME_CATEGORY.VOLCANO:
				if ((bool)sourceWP.GetComponent<MinigameCutscene_Evacuee>())
				{
					_instance.ShowFace(sourceWP.facePos, EFACE_TYPE.EXPLORER_OVERJOYED);
				}
				else
				{
					_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.EXPLORER_HAPPY, true);
				}
				break;
			}
		}
		MinigameManager.EMINIGAME_TYPE pCurrentMinigameType = MinigameManager._pInstance._pCurrentMinigameType;
		if (pCurrentMinigameType == MinigameManager.EMINIGAME_TYPE.CROOK_ROUNDUP)
		{
			_instance.ShowFace(facePos, EFACE_TYPE.CROOK_DIZZY, false, true);
		}
	}

	public static void OnWaypointComplete(MinigameObjective_Waypoint sourceWP)
	{
		if (sourceWP == null || MinigameManager._pInstance == null || MinigameController._pInstance == null || MinigameController._pInstance._pMinigame._pHasBeenCompleted)
		{
			return;
		}
		Transform facePos = sourceWP.facePos;
		VehiclePart component = VehicleController_Player._pInstance.GetComponent<VehiclePart>();
		if (component != null)
		{
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.FIRE)
			{
				_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.FIREMAN_HAPPY, true);
			}
		}
	}

	public static void OnAirBonus(float thresh)
	{
		if (MinigameManager._pInstance == null || MinigameController._pInstance == null || MinigameController._pInstance._pMinigame._pHasBeenCompleted)
		{
			return;
		}
		VehiclePart component = VehicleController_Player._pInstance.GetComponent<VehiclePart>();
		if (component != null)
		{
			Transform pFaceTransform = component._pFaceTransform;
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.POLICE)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.POLICE_HAPPY, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.FIRE)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.FIREMAN_HAPPY, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.VOLCANO)
			{
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.EXPLORER_HAPPY, true);
			}
		}
	}

	public static void OnCrookTaunt(Transform crookTransform)
	{
		if (!(MinigameManager._pInstance == null) && !(MinigameController._pInstance == null) && !MinigameController._pInstance._pMinigame._pHasBeenCompleted)
		{
			VehiclePart component = VehicleController_Player._pInstance.GetComponent<VehiclePart>();
			if (component != null)
			{
				Transform pFaceTransform = component._pFaceTransform;
				_instance.ShowFace(pFaceTransform, EFACE_TYPE.POLICE_DETERMINED, true);
			}
			_instance.ShowFace(crookTransform, EFACE_TYPE.CROOK_GRINNING, false, true);
		}
	}

	public static void OnHoseHitCrook(Transform crookTransform)
	{
		if (!(MinigameManager._pInstance == null) && !(MinigameController._pInstance == null) && !MinigameController._pInstance._pMinigame._pHasBeenCompleted)
		{
			_instance.ShowFace(crookTransform, EFACE_TYPE.CROOK_DIZZY, false, true);
		}
	}

	public static void OnVehicleEnteredWhirlpool(Vehicle vehicle)
	{
		if (MinigameManager._pInstance == null || MinigameController._pInstance == null)
		{
			return;
		}
		VehiclePart component = vehicle.GetComponent<VehiclePart>();
		if (VehicleController_Player.IsPlayer(vehicle))
		{
			VehiclePart component2 = VehicleController_Player._pInstance.GetComponent<VehiclePart>();
			if (component2 != null)
			{
				Transform pFaceTransform = component2._pFaceTransform;
				MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
				if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.POLICE)
				{
					_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.POLICE_DIZZY, true);
				}
				else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.FIRE)
				{
					_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.FIREMAN_DIZZY, true);
				}
				else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.VOLCANO)
				{
					_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.EXPLORER_DIZZY, true);
				}
			}
		}
		else if (vehicle._pController is VehicleController_RobberTyre)
		{
			_instance.ShowFace(vehicle.GetComponent<MinigameObjective_Waypoint>().facePos, EFACE_TYPE.CROOK_DIZZY, false, true);
		}
		else if (component.uniqueID != VehiclePart.EUNIQUE_ID.BODY_SHARK)
		{
			_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.NPC_DIZZY);
		}
	}

	public static void OnPlayerAirTime()
	{
		if (!(MinigameManager._pInstance == null) && !(MinigameController._pInstance == null) && !MinigameController._pInstance._pMinigame._pHasBeenCompleted)
		{
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.POLICE)
			{
				_instance.ShowFace(VehicleController_Player._pInstance.GetComponent<VehiclePart>()._pFaceTransform, EFACE_TYPE.POLICE_OVERJOYED, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.FIRE)
			{
				_instance.ShowFace(VehicleController_Player._pInstance.GetComponent<VehiclePart>()._pFaceTransform, EFACE_TYPE.FIREMAN_OVERJOYED, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.VOLCANO)
			{
				_instance.ShowFace(VehicleController_Player._pInstance.GetComponent<VehiclePart>()._pFaceTransform, EFACE_TYPE.EXPLORER_OVERJOYED, true);
			}
		}
	}

	public static void OnVehicleEnteredLavaJet(Vehicle vehicle)
	{
		if (MinigameManager._pInstance == null || MinigameController._pInstance == null)
		{
			return;
		}
		VehiclePart component = vehicle.GetComponent<VehiclePart>();
		if (VehicleController_Player.IsPlayer(vehicle))
		{
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.POLICE)
			{
				_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.POLICE_DIZZY, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.FIRE)
			{
				_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.FIREMAN_DIZZY, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.VOLCANO)
			{
				_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.EXPLORER_DIZZY, true);
			}
		}
		else
		{
			_instance.ShowFace(component._pFaceTransform, EFACE_TYPE.NPC_DIZZY, false, false, component);
		}
	}

	public static void OnLavaBombHitPlayer()
	{
		if (!(MinigameManager._pInstance == null) && !(MinigameController._pInstance == null))
		{
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.POLICE)
			{
				_instance.ShowFace(VehicleController_Player._pInstance.GetComponent<VehiclePart>()._pFaceTransform, EFACE_TYPE.POLICE_DIZZY, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.FIRE)
			{
				_instance.ShowFace(VehicleController_Player._pInstance.GetComponent<VehiclePart>()._pFaceTransform, EFACE_TYPE.FIREMAN_DIZZY, true);
			}
			else if (currentMinigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.VOLCANO)
			{
				_instance.ShowFace(VehicleController_Player._pInstance.GetComponent<VehiclePart>()._pFaceTransform, EFACE_TYPE.EXPLORER_DIZZY, true);
			}
		}
	}

	private void Awake()
	{
		_instance = this;
		int num = 16;
		for (int i = 0; i < num; i++)
		{
			string faceName = GetFaceName((EFACE_TYPE)i);
			if (faceName != null)
			{
				_atlasLookup[faceName] = FindAtlas(faceName);
			}
		}
	}

	private void OnDestroy()
	{
		_instance = null;
	}

	private void LateUpdate()
	{
		int num = _emoticons.Length;
		for (int i = 0; i < num; i++)
		{
			if (_emoticons[i]._pInUse)
			{
				_emoticons[i].DoUpdate();
			}
		}
		int count = _backlog.Count;
		if (count > 0)
		{
			float num2 = Time.time - _backlog[0]._evocationTime;
			if (num2 >= 2f)
			{
				_backlog.RemoveAt(0);
			}
			if (Time.time - _lastFaceShowTime > 0.5f)
			{
				ShowFace(_backlog[0]._location, _backlog[0]._faceType, _backlog[0]._isPlayer, _backlog[0]._isCrook, _backlog[0]._ownerVehiclePart, true);
				_backlog.RemoveAt(0);
			}
		}
	}

	public string GetFaceName(EFACE_TYPE faceType)
	{
		string result = null;
		switch (faceType)
		{
		case EFACE_TYPE.CROOK_DIZZY:
			result = "Crook_Dizzy";
			break;
		case EFACE_TYPE.CROOK_GRINNING:
			result = "Crook_Grinning";
			break;
		case EFACE_TYPE.EXPLORER_DIZZY:
			result = "ExplorerDizzy";
			break;
		case EFACE_TYPE.EXPLORER_HAPPY:
			result = "ExplorerHappy";
			break;
		case EFACE_TYPE.EXPLORER_OVERJOYED:
			result = "ExplorerOverjoyed";
			break;
		case EFACE_TYPE.FIREMAN_DIZZY:
			result = "Fireman_Dizzy";
			break;
		case EFACE_TYPE.FIREMAN_HAPPY:
			result = "Fireman_Happy";
			break;
		case EFACE_TYPE.FIREMAN_OVERJOYED:
			result = "Fireman_Overjoyed";
			break;
		case EFACE_TYPE.NPC_DIZZY:
			result = "NPC_Dizzy";
			break;
		case EFACE_TYPE.NPC_HAPPY:
			result = "NPC_Happy";
			break;
		case EFACE_TYPE.NPC_WORRIED:
			result = "NPC_Worried";
			break;
		case EFACE_TYPE.POLICE_DETERMINED:
			result = "Police_Determined";
			break;
		case EFACE_TYPE.POLICE_DIZZY:
			result = "Police_Dizzy";
			break;
		case EFACE_TYPE.POLICE_HAPPY:
			result = "Police_Happy";
			break;
		case EFACE_TYPE.POLICE_OVERJOYED:
			result = "Police_Overjoyed";
			break;
		}
		return result;
	}

	public void ShowFace(Transform location, EFACE_TYPE faceType, bool isPlayer = false, bool isCrook = false, VehiclePart ownerVehiclePart = null, bool forceFromBacklog = false)
	{
		if (!forceFromBacklog && (Time.time - _lastFaceShowTime < 0.5f || _backlog.Count > 0))
		{
			EmoticonBackLog emoticonBackLog = new EmoticonBackLog();
			emoticonBackLog._evocationTime = Time.time;
			emoticonBackLog._location = location;
			emoticonBackLog._faceType = faceType;
			emoticonBackLog._isPlayer = isPlayer;
			emoticonBackLog._isCrook = isCrook;
			emoticonBackLog._ownerVehiclePart = ownerVehiclePart;
			_backlog.Add(emoticonBackLog);
			return;
		}
		_lastFaceShowTime = Time.time;
		int num = _emoticons.Length;
		if (!isPlayer && !isCrook && ownerVehiclePart != null && !ownerVehiclePart.IsVehicleType(MinigameManager._pInstance._pCurrentVehicleTypeForMinigame))
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			if ((_emoticons[i]._pInUse && _emoticons[i]._pTrackedTransform == location) || (_emoticons[i]._pInUse && _emoticons[i]._pIsCrook && isCrook) || (_emoticons[i]._pInUse && _emoticons[i]._pIsPlayer && isPlayer))
			{
				return;
			}
		}
		for (int j = 0; j < num; j++)
		{
			if (!_emoticons[j]._pInUse)
			{
				string faceName = GetFaceName(faceType);
				UIAtlas atlas = _atlasLookup[faceName];
				_emoticons[j].Display(location, faceType, faceName, atlas);
				break;
			}
		}
	}

	private UIAtlas FindAtlas(string spriteName)
	{
		bool flag = false;
		int num = _atlases.Length;
		for (int i = 0; i < num; i++)
		{
			if (flag)
			{
				break;
			}
			UIAtlas uIAtlas = _atlases[i];
			int count = uIAtlas.spriteList.Count;
			for (int j = 0; j < count; j++)
			{
				if (flag)
				{
					break;
				}
				if (uIAtlas.spriteList[j].name == spriteName)
				{
					return uIAtlas;
				}
			}
		}
		return null;
	}

	public void OnShowScreen()
	{
		int num = _emoticons.Length;
		for (int i = 0; i < num; i++)
		{
			_emoticons[i].OnShowScreen();
		}
	}
}
