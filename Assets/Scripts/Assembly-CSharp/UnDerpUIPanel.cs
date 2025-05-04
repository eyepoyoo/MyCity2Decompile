using UnityEngine;

public class UnDerpUIPanel : MonoBehaviour
{
	private UIPanel _panel;

	private UIPanel _pPanel
	{
		get
		{
			if (_panel == null)
			{
				_panel = GetComponent<UIPanel>();
			}
			return _panel;
		}
	}

	private void Update()
	{
		if (Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			_pPanel.SetDirty();
		}
	}
}
