using UnityEngine;

public class NGUIMessagePassThru : MonoBehaviour
{
	public GameObject targetObj;

	private void OnHover(bool isOver)
	{
		targetObj.SendMessage("OnHover", isOver);
	}

	private void OnPress(bool isDown)
	{
		targetObj.SendMessage("OnPress", isDown);
	}

	private void OnSelect(bool selected)
	{
		targetObj.SendMessage("OnSelect", selected);
	}

	private void OnClick()
	{
		targetObj.SendMessage("OnClick");
	}

	private void OnDoubleClick()
	{
		targetObj.SendMessage("OnDoubleClick");
	}

	private void OnDragStart()
	{
		targetObj.SendMessage("OnDragStart");
	}

	private void OnDrag(Vector2 delta)
	{
		targetObj.SendMessage("OnDrag", delta);
	}

	private void OnDragOver(GameObject draggedObject)
	{
		targetObj.SendMessage("OnDragOver", draggedObject);
	}

	private void OnDragOut(GameObject draggedObject)
	{
		targetObj.SendMessage("OnDragOut", draggedObject);
	}

	private void OnDragEnd()
	{
		targetObj.SendMessage("OnDragEnd");
	}

	private void OnInput(string text)
	{
		targetObj.SendMessage("OnInput", text);
	}

	private void OnTooltip(bool show)
	{
		targetObj.SendMessage("OnTooltip", show);
	}

	private void OnScroll(float delta)
	{
		targetObj.SendMessage("OnScroll", delta);
	}

	private void OnKey(KeyCode key)
	{
		targetObj.SendMessage("OnKey", key);
	}
}
