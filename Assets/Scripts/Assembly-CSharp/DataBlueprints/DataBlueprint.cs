using System;
using System.Collections.Generic;
using GameDefines;
using LitJson;
using UnityEngine;

namespace DataBlueprints
{
	[Serializable]
	public class DataBlueprint
	{
		private const string KEY_PARENT_BLUEPRINT = "parentBlueprint";

		private const string KEY_PROPERTIES = "properties";

		private const string NONE = "[None]";

		private const string NEW_PROPERTY_NAME = "Property";

		public string _name;

		public string _parentBlueprintName;

		public List<DataBlueprintProperty> _properties = new List<DataBlueprintProperty>();

		public bool _dirty;

		public DataBlueprint()
		{
			_properties = new List<DataBlueprintProperty>();
		}

		public DataBlueprint(string name, JsonData source)
		{
			_name = name;
			_parentBlueprintName = (string)source["parentBlueprint"];
			_properties = new List<DataBlueprintProperty>();
			for (int i = 0; i < source["properties"].Count; i++)
			{
				_properties.Add(new DataBlueprintProperty(source["properties"][i]));
			}
		}

		public DataBlueprint(string name, string parent)
		{
			_name = name;
			_parentBlueprintName = parent;
		}

		public DataBlueprintProperty[] GetAllProperties(bool includeInheritedDuplicates = false, IList<DataBlueprint> inList = null)
		{
			List<DataBlueprintProperty> list = new List<DataBlueprintProperty>();
			if (GetParent(inList) != null)
			{
				DataBlueprintProperty[] allProperties = GetParent(inList).GetAllProperties(includeInheritedDuplicates);
				foreach (DataBlueprintProperty dataBlueprintProperty in allProperties)
				{
					if (includeInheritedDuplicates || GetProperty(dataBlueprintProperty._name) == null)
					{
						list.Add(dataBlueprintProperty);
					}
				}
			}
			list.AddRange(_properties);
			return list.ToArray();
		}

		public DataBlueprintProperty GetProperty(string name, bool includeAncestors = false, IList<DataBlueprint> inList = null)
		{
			foreach (DataBlueprintProperty property in _properties)
			{
				if (property._name == name)
				{
					return property;
				}
			}
			if (includeAncestors && GetParent(inList) != null)
			{
				return GetParent(inList).GetProperty(name, true);
			}
			return null;
		}

		public DataBlueprintProperty GetPropertyByObjectMemberName(string objectMemberName, bool includeAncestors = false, IList<DataBlueprint> inList = null)
		{
			if (string.IsNullOrEmpty(objectMemberName))
			{
				return null;
			}
			foreach (DataBlueprintProperty property in _properties)
			{
				if (property._objectMemberName == objectMemberName)
				{
					return property;
				}
			}
			if (includeAncestors && GetParent(inList) != null)
			{
				return GetParent(inList).GetPropertyByObjectMemberName(objectMemberName, true);
			}
			return null;
		}

		public DataBlueprint GetRootPropertyOwner(string propertyName, IList<DataBlueprint> inList = null)
		{
			DataBlueprint result = this;
			DataBlueprint dataBlueprint = this;
			do
			{
				if (dataBlueprint.GetProperty(propertyName) != null)
				{
					result = dataBlueprint;
				}
				dataBlueprint = dataBlueprint.GetParent(inList);
			}
			while (dataBlueprint != null);
			return result;
		}

		public bool HasBlueprintAsAncestor(DataBlueprint blueprint, IList<DataBlueprint> inList = null)
		{
			return blueprint == this || (GetParent(inList) != null && GetParent(inList).HasBlueprintAsAncestor(blueprint));
		}

		public void DeleteProperty(DataBlueprintProperty property, EDeletePropertyType type, IList<DataBlueprint> inList = null)
		{
			if (property != null)
			{
				DeleteProperty(property._name, type, inList);
			}
		}

		public void DeleteProperty(string name, EDeletePropertyType type, IList<DataBlueprint> inList = null)
		{
			DataBlueprintProperty property = GetProperty(name);
			if (property != null)
			{
				_properties.Remove(property);
				_dirty = true;
			}
			switch (type)
			{
			case EDeletePropertyType.MoveToChildren:
			{
				if (property == null)
				{
					break;
				}
				DataBlueprint[] children2 = GetChildren(inList);
				foreach (DataBlueprint dataBlueprint2 in children2)
				{
					if (dataBlueprint2.GetProperty(name) == null)
					{
						dataBlueprint2._properties.Add(property.Clone());
					}
				}
				break;
			}
			case EDeletePropertyType.All:
			{
				DataBlueprint[] children = GetChildren(inList);
				foreach (DataBlueprint dataBlueprint in children)
				{
					dataBlueprint.DeleteProperty(name, EDeletePropertyType.All);
				}
				break;
			}
			}
		}

		public static string NameOrNone(DataBlueprint blueprint)
		{
			return (blueprint == null) ? "[None]" : blueprint._name;
		}

		public DataBlueprint GetParent(IList<DataBlueprint> inList = null)
		{
			return BlueprintManager.GetBlueprint(_parentBlueprintName, inList);
		}

		public void SetParent(DataBlueprint parent)
		{
			_parentBlueprintName = ((parent == null) ? null : parent._name);
		}

		public DataBlueprint[] GetChildren(IList<DataBlueprint> inList = null)
		{
			List<DataBlueprint> list = new List<DataBlueprint>();
			foreach (DataBlueprint item in inList ?? BlueprintManager._pBlueprints)
			{
				if (item._parentBlueprintName == _name)
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		public bool HasChildren(IList<DataBlueprint> inList = null)
		{
			return GetChildren(inList).Length > 0;
		}

		public DataBlueprint GetNextSibling(IList<DataBlueprint> inList = null)
		{
			if (GetParent(inList) == null)
			{
				return null;
			}
			bool flag = false;
			DataBlueprint[] children = GetParent(inList).GetChildren(inList);
			foreach (DataBlueprint dataBlueprint in children)
			{
				if (dataBlueprint == this)
				{
					flag = true;
				}
				else if (flag)
				{
					return dataBlueprint;
				}
			}
			return null;
		}

		public int GetInheritanceLevel(IList<DataBlueprint> inList = null)
		{
			return (GetParent(inList) != null) ? (GetParent(inList).GetInheritanceLevel(inList) + 1) : 0;
		}

		public string GetInheritancePath(IList<DataBlueprint> inList = null)
		{
			return (string.IsNullOrEmpty(_parentBlueprintName) ? string.Empty : (GetParent(inList).GetInheritancePath(inList) + "/")) + _name;
		}

		public string GetNextNewPropertyName(IList<DataBlueprint> inList = null)
		{
			int num = -1;
			DataBlueprintProperty[] allProperties = GetAllProperties(false, inList);
			foreach (DataBlueprintProperty dataBlueprintProperty in allProperties)
			{
				if (dataBlueprintProperty._name.StartsWith("Property"))
				{
					string text = dataBlueprintProperty._name.Substring("Property".Length).Trim();
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
			return "Property" + ((num < 0) ? string.Empty : (num + 1).ToString());
		}

		public string GetPropertiesInfo(IList<DataBlueprint> inList = null)
		{
			if (_properties.Count == 0)
			{
				return "Contains no properties";
			}
			string text = "Properties:";
			foreach (DataBlueprintProperty property in _properties)
			{
				text += string.Format("\n• {0}: {1}", property._name, property._pValue);
			}
			return text;
		}

		public void UpdateChildrensOverridingProperties(IList<DataBlueprint> inList, IList<DataBlueprintProperty> rootProperties = null)
		{
			if (rootProperties == null)
			{
				rootProperties = _properties;
			}
			DataBlueprint[] children = GetChildren(inList);
			foreach (DataBlueprint dataBlueprint in children)
			{
				foreach (DataBlueprintProperty rootProperty in rootProperties)
				{
					DataBlueprintProperty property = dataBlueprint.GetProperty(rootProperty._name);
					if (property != null)
					{
						property._objectMemberName = rootProperty._objectMemberName;
						property._description = rootProperty._description;
						property._valueType = rootProperty._valueType;
					}
				}
				dataBlueprint.UpdateChildrensOverridingProperties(inList, rootProperties);
			}
		}

		public void SetNameOfPropertyInChildren(IList<DataBlueprint> inList, string currName, string newName)
		{
			DataBlueprint[] children = GetChildren(inList);
			foreach (DataBlueprint dataBlueprint in children)
			{
				DataBlueprintProperty property = dataBlueprint.GetProperty(currName);
				if (property != null)
				{
					property._name = newName;
				}
			}
		}

		public void CopyProperties(DataBlueprint from)
		{
			foreach (DataBlueprintProperty property2 in from._properties)
			{
				DataBlueprintProperty property = GetProperty(property2._name);
				if (property != null)
				{
					property._pValue = property2._pValue;
				}
				else
				{
					_properties.Add(property2.Clone());
				}
			}
		}

		public void SetNameRecursively(string newName, IList<DataBlueprint> inList = null)
		{
			DataBlueprint[] children = GetChildren(inList);
			foreach (DataBlueprint dataBlueprint in children)
			{
				dataBlueprint._parentBlueprintName = newName;
			}
			_name = newName;
		}

		public int GetInt(string property)
		{
			DataBlueprintProperty property2 = GetProperty(property, true);
			if (property2 != null && property2._valueType == DataBlueprintProperty.ValueType.Int)
			{
				return (int)property2._pValue;
			}
			return 9898989;
		}

		public float GetFloat(string property)
		{
			DataBlueprintProperty property2 = GetProperty(property, true);
			if (property2 != null && property2._valueType == DataBlueprintProperty.ValueType.Float)
			{
				return (float)property2._pValue;
			}
			return 98989896f;
		}

		public string GetString(string property)
		{
			DataBlueprintProperty property2 = GetProperty(property, true);
			if (property2 != null && property2._valueType == DataBlueprintProperty.ValueType.String)
			{
				return (string)property2._pValue;
			}
			return GlobalDefines.INVALID_STRING;
		}

		public bool GetBool(string property)
		{
			DataBlueprintProperty property2 = GetProperty(property, true);
			if (property2 != null && property2._valueType == DataBlueprintProperty.ValueType.Bool)
			{
				return (bool)property2._pValue;
			}
			return false;
		}

		public bool TryGetInt(string property, out int value)
		{
			DataBlueprintProperty property2 = GetProperty(property, true);
			if (property2 != null && property2._valueType == DataBlueprintProperty.ValueType.Int)
			{
				value = (int)property2._pValue;
				return true;
			}
			value = 9898989;
			return false;
		}

		public bool TryGetFloat(string property, out float value)
		{
			DataBlueprintProperty property2 = GetProperty(property, true);
			if (property2 != null && property2._valueType == DataBlueprintProperty.ValueType.Float)
			{
				value = (float)property2._pValue;
				return true;
			}
			value = 98989896f;
			return false;
		}

		public bool TryGetString(string property, out string value)
		{
			DataBlueprintProperty property2 = GetProperty(property, true);
			if (property2 != null && property2._valueType == DataBlueprintProperty.ValueType.String)
			{
				value = (string)property2._pValue;
				return true;
			}
			value = GlobalDefines.INVALID_STRING;
			return false;
		}

		public bool TryGetBool(string property, out bool value)
		{
			DataBlueprintProperty property2 = GetProperty(property, true);
			if (property2 != null && property2._valueType == DataBlueprintProperty.ValueType.Bool)
			{
				value = (bool)property2._pValue;
				return true;
			}
			value = false;
			return false;
		}
	}
}
