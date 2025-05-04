using DataBlueprints;
using UnityEngine;

public class TestDataBlueprintBehaviour : DataBlueprintBehaviour
{
	public float _float1;

	public float _float21;

	public int _int1;

	public string _string1;

	public bool _bool1;

	public GameObject _prefab1;

	public Texture2D _texture1;

	public AudioClip _audio1;

	private new void Awake()
	{
		Debug.LogError(base._pBlueprint.GetInt("asd"));
		Debug.LogError(base._pBlueprint.GetFloat("Int"));
		Debug.LogError(base._pBlueprint.GetInt("Int"));
		Debug.LogError(base._pBlueprint.GetFloat("Float"));
		Debug.LogError(base._pBlueprint.GetString("Float"));
		Debug.LogError(base._pBlueprint.GetString("String"));
		Debug.LogError(base._pBlueprint.GetBool("Bool"));
		string value;
		if (base._pBlueprint.TryGetString("String", out value))
		{
			Debug.LogError(value);
		}
		else
		{
			Debug.LogError("none");
		}
	}

	public void SetValues(DataBlueprint blueprint)
	{
		DataBlueprintProperty[] allProperties = blueprint.GetAllProperties();
		foreach (DataBlueprintProperty dataBlueprintProperty in allProperties)
		{
			switch (dataBlueprintProperty._name)
			{
			case "Float1":
				_float1 = (float)dataBlueprintProperty._pValue;
				break;
			case "_float21":
				_float21 = (float)dataBlueprintProperty._pValue;
				break;
			case "_int1":
				_int1 = (int)dataBlueprintProperty._pValue;
				break;
			case "_string1":
				_string1 = (string)dataBlueprintProperty._pValue;
				break;
			case "_bool1":
				_bool1 = (bool)dataBlueprintProperty._pValue;
				break;
			}
		}
	}
}
