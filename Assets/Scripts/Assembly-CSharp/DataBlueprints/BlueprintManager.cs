using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using LitJson;
using UnityEngine;

namespace DataBlueprints
{
	public static class BlueprintManager
	{
		private const string TEXT_ASSET_RESOURCE_PATH = "DataBlueprints";

		private const string NEW_BLUEPRINT_NAME = "New Blueprint";

		public static ListWrapper _blueprintsListWrapper;

		public static bool _dirty;

		public static List<DataBlueprint> _pBlueprints
		{
			get
			{
				if (_blueprintsListWrapper == null)
				{
					LoadBlueprints();
					if (_blueprintsListWrapper == null)
					{
						return null;
					}
				}
				return _blueprintsListWrapper._list;
			}
		}

		public static string _pNextNewBlueprintName
		{
			get
			{
				int num = -1;
				foreach (DataBlueprint pBlueprint in _pBlueprints)
				{
					if (pBlueprint._name.StartsWith("New Blueprint"))
					{
						string text = pBlueprint._name.Substring("New Blueprint".Length).Trim();
						int result;
						if (text.Length == 0)
						{
							num = Mathf.Max(1, num);
						}
						else if (int.TryParse(text, out result))
						{
							num = Mathf.Max(result, num);
						}
					}
				}
				return "New Blueprint" + ((num < 0) ? string.Empty : (" " + (num + 1)));
			}
		}

		private static TextAsset _pTextAsset
		{
			get
			{
				return Resources.Load<TextAsset>("DataBlueprints");
			}
		}

		public static string _pTextAssetPath
		{
			get
			{
				Debug.LogError("Not supported outside of Unity Editor");
				return string.Empty;
			}
		}

		public static DataBlueprint GetBlueprint(string name, IList<DataBlueprint> inList = null)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			foreach (DataBlueprint item in inList ?? _pBlueprints)
			{
				if (item._name == name)
				{
					return item;
				}
			}
			return null;
		}

		public static void DeleteBlueprint(string blueprintName, bool deleteChildren = false)
		{
			DeleteBlueprint(GetBlueprint(blueprintName), deleteChildren);
		}

		public static void DeleteBlueprint(DataBlueprint blueprint, bool deleteChildren = false)
		{
			if (blueprint == null)
			{
				return;
			}
			if (!_pBlueprints.Contains(blueprint))
			{
				Debug.LogError(string.Format("Can't delete blueprint \"{0}\" - does not exist", blueprint._name));
				return;
			}
			_pBlueprints.Remove(blueprint);
			DataBlueprint[] children = blueprint.GetChildren();
			foreach (DataBlueprint dataBlueprint in children)
			{
				dataBlueprint._parentBlueprintName = null;
				if (deleteChildren)
				{
					DeleteBlueprint(dataBlueprint, true);
				}
			}
		}

		public static DataBlueprint AddBlueprint(string name = null, string parent = null)
		{
			DataBlueprint dataBlueprint = new DataBlueprint(name ?? _pNextNewBlueprintName, parent);
			_pBlueprints.Add(dataBlueprint);
			return dataBlueprint;
		}

		public static void AddBlueprint(DataBlueprint blueprint, int atIndex = -1)
		{
			if (atIndex == -1)
			{
				_pBlueprints.Add(blueprint);
			}
			else
			{
				_pBlueprints.Insert(atIndex, blueprint);
			}
		}

		public static void LoadBlueprints()
		{
			_dirty = false;
			_blueprintsListWrapper = null;
			TextAsset pTextAsset = _pTextAsset;
			if (!pTextAsset)
			{
				Debug.LogError(string.Format("Couldn't load DataBlueprints text asset file at \"{0}\" (must be in resources)", "DataBlueprints"));
				return;
			}
			JsonData jsonData = JsonMapper.ToObject(new JsonReader(pTextAsset.text)) ?? new JsonData();
			_blueprintsListWrapper = ScriptableObject.CreateInstance<ListWrapper>();
			foreach (DictionaryEntry item in (IOrderedDictionary)jsonData)
			{
				_pBlueprints.Add(new DataBlueprint((string)item.Key, (JsonData)item.Value));
			}
		}

		public static void SaveBlueprints()
		{
		}

		public static string GetNextDuplicatedNewBlueprintName(string name, IList<DataBlueprint> inList = null)
		{
			int num = 0;
			Regex regex = new Regex(Regex.Escape(name) + "\\s*\\((?<num>[0-9]+)\\)\\s*");
			foreach (DataBlueprint item in inList ?? _pBlueprints)
			{
				if (regex.IsMatch(item._name))
				{
					int num2 = int.Parse(regex.Match(item._name).Groups["num"].Value);
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return string.Format("{0} ({1})", name, num + 1);
		}

		public static bool HasAllPropertiesOfBlueprint(this object target, DataBlueprint blueprint)
		{
			if (blueprint == null)
			{
				Debug.LogError("Blueprint is null!");
				return false;
			}
			DataBlueprintProperty[] allProperties = blueprint.GetAllProperties();
			foreach (DataBlueprintProperty property in allProperties)
			{
				if (!target.HasDataBlueprintProperty(property))
				{
					return false;
				}
			}
			return true;
		}

		public static void SetValuesFromBlueprint(this object target, DataBlueprint blueprint)
		{
			if (blueprint == null)
			{
				Debug.LogError("Blueprint is null!");
				return;
			}
			DataBlueprintProperty[] allProperties = blueprint.GetAllProperties();
			foreach (DataBlueprintProperty dataBlueprintProperty in allProperties)
			{
				if (target.HasDataBlueprintProperty(dataBlueprintProperty))
				{
					target.SetFieldOrProperty(dataBlueprintProperty._objectMemberName, dataBlueprintProperty._pValue);
				}
			}
		}

		public static DataBlueprintProperty[] GetPropertiesThatMatchBlueprint(this object target, DataBlueprint blueprint)
		{
			List<DataBlueprintProperty> list = new List<DataBlueprintProperty>();
			if (blueprint == null)
			{
				return list.ToArray();
			}
			DataBlueprintProperty[] allProperties = blueprint.GetAllProperties();
			foreach (DataBlueprintProperty dataBlueprintProperty in allProperties)
			{
				if (target.HasDataBlueprintProperty(dataBlueprintProperty))
				{
					list.Add(dataBlueprintProperty);
				}
			}
			return list.ToArray();
		}

		public static bool HasPropertiesThatMatchBlueprint(this object target, DataBlueprint blueprint)
		{
			return target.GetPropertiesThatMatchBlueprint(blueprint).Length > 0;
		}

		public static bool HasDataBlueprintProperty(this object target, DataBlueprintProperty property, bool ignoreType = false)
		{
			return target.HasFieldOrProperty(property._objectMemberName, (!ignoreType) ? property._pSystemType : null);
		}
	}
}
