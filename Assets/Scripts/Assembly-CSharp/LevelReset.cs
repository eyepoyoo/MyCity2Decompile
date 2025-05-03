using UnityEngine;
using UnityEngine.EventSystems;

public class LevelReset : MonoBehaviour, IEventSystemHandler, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData data)
	{
		Application.LoadLevelAsync(Application.loadedLevelName);
	}

	private void Update()
	{
	}
}
