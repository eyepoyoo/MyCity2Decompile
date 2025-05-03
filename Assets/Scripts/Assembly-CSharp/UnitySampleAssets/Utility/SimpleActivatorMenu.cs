using UnityEngine;

namespace UnitySampleAssets.Utility
{
	public class SimpleActivatorMenu : MonoBehaviour
	{
		public GUIText camSwitchButton;

		public GameObject[] objects;

		private int currentActiveObject;

		private void OnEnable()
		{
			currentActiveObject = 0;
			camSwitchButton.text = objects[currentActiveObject].name;
		}

		public void NextCamera()
		{
			int num = ((currentActiveObject + 1 < objects.Length) ? (currentActiveObject + 1) : 0);
			for (int i = 0; i < objects.Length; i++)
			{
				objects[i].SetActive(i == num);
			}
			currentActiveObject = num;
			camSwitchButton.text = objects[currentActiveObject].name;
		}
	}
}
