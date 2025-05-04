using System;
using DataBlueprints;
using UnityEngine;

public class DataBlueprintsTestRig : MonoBehaviour
{
	private const float UI_SCALE = 3f;

	private const float LINE_HEIGHT = 90f;

	private const float COLUMN_WIDTH_1 = 240f;

	private const float COLUMN_WIDTH_2 = 300f;

	public GameObject _dbb;

	private int _numObjects = 10;

	private int _numTestIterations = 1;

	private int _lastTestTime;

	private void Awake()
	{
		CreateObjects(_numObjects);
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(0f, 0f, 240f, 90f), "Objects:");
		string s = GUI.TextField(new Rect(240f, 0f, 300f, 90f), _numObjects.ToString());
		int result;
		if (int.TryParse(s, out result) && result != _numObjects)
		{
			_numObjects = result;
			CreateObjects(_numObjects);
		}
		GUI.Label(new Rect(0f, 90f, 240f, 90f), "Iterations:");
		string s2 = GUI.TextField(new Rect(240f, 90f, 300f, 90f), _numTestIterations.ToString());
		int result2;
		if (int.TryParse(s2, out result2) && result2 != _numTestIterations)
		{
			_numTestIterations = result2;
		}
		if (GUI.Button(new Rect(0f, 180f, 540f, 90f), "Test"))
		{
			_lastTestTime = RunTest(_numTestIterations);
		}
		GUI.Label(new Rect(0f, 270f, 240f, 90f), _lastTestTime + "ms");
	}

	private void CreateObjects(int numToCreate)
	{
		TestDataBlueprintBehaviour[] componentsInChildren = GetComponentsInChildren<TestDataBlueprintBehaviour>();
		foreach (TestDataBlueprintBehaviour testDataBlueprintBehaviour in componentsInChildren)
		{
			UnityEngine.Object.Destroy(testDataBlueprintBehaviour.gameObject);
		}
		for (int j = 0; j < numToCreate; j++)
		{
			TestDataBlueprintBehaviour component = UnityEngine.Object.Instantiate(_dbb).GetComponent<TestDataBlueprintBehaviour>();
			component.transform.parent = base.transform;
		}
	}

	private int RunTest(int numIterations)
	{
		DateTime now = DateTime.Now;
		TestDataBlueprintBehaviour[] componentsInChildren = GetComponentsInChildren<TestDataBlueprintBehaviour>();
		for (int i = 0; i < numIterations; i++)
		{
			for (int num = componentsInChildren.Length - 1; num >= 0; num--)
			{
				componentsInChildren[num].SetValuesFromBlueprint(componentsInChildren[num]._pBlueprint);
				componentsInChildren[num].SetValues(componentsInChildren[num]._pBlueprint);
			}
		}
		return (int)((DateTime.Now - now).Ticks / 10000);
	}
}
