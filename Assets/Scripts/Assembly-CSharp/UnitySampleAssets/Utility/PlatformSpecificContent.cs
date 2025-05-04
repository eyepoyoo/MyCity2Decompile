using UnityEngine;

namespace UnitySampleAssets.Utility
{
	public class PlatformSpecificContent : MonoBehaviour
	{
		private enum BuildTargetGroup
		{
			Standalone = 0,
			Mobile = 1
		}

		[SerializeField]
		private BuildTargetGroup showOnlyOn;

		[SerializeField]
		private GameObject[] content = new GameObject[0];

		[SerializeField]
		private bool childrenOfThisObject;

		private void OnEnable()
		{
			CheckEnableContent();
		}

		private void CheckEnableContent()
		{
			if (showOnlyOn == BuildTargetGroup.Mobile)
			{
				EnableContent(true);
			}
			else
			{
				EnableContent(false);
			}
		}

		private void EnableContent(bool enabled)
		{
			if (content.Length > 0)
			{
				GameObject[] array = content;
				foreach (GameObject gameObject in array)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(enabled);
					}
				}
			}
			if (!childrenOfThisObject)
			{
				return;
			}
			foreach (Transform item in base.transform)
			{
				item.gameObject.SetActive(enabled);
			}
		}
	}
}
