using GameDefines;
using UnityEngine;

namespace DataBlueprints
{
	public abstract class DataBlueprintBehaviour : MonoBehaviour
	{
		[HideInInspector]
		public EInitializeType _initialisationType = EInitializeType.MANUAL;

		[HideInInspector]
		[SerializeField]
		private string _blueprintName;

		public string _pBlueprintName
		{
			get
			{
				return _blueprintName;
			}
			set
			{
				DataBlueprint blueprint = BlueprintManager.GetBlueprint(value);
				if (!string.IsNullOrEmpty(value) && blueprint == null)
				{
					Debug.LogError(string.Format("Blueprint \"{0}\" not found!", value));
				}
				else
				{
					_blueprintName = value;
				}
			}
		}

		public DataBlueprint _pBlueprint
		{
			get
			{
				return BlueprintManager.GetBlueprint(_blueprintName);
			}
			set
			{
				_pBlueprintName = ((value == null) ? string.Empty : value._name);
			}
		}

		public virtual void Awake()
		{
			if (_initialisationType == EInitializeType.AWAKE)
			{
				Initialise();
			}
		}

		public virtual void Start()
		{
			if (_initialisationType == EInitializeType.START)
			{
				Initialise();
			}
		}

		public virtual void Initialise()
		{
			if (_pBlueprint != null)
			{
				this.SetValuesFromBlueprint(_pBlueprint);
			}
		}
	}
}
