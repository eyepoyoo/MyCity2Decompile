using UnityEngine;

public class ScreenTerms : ScreenBase
{
	public UIScrollView _scrollView;

	protected override void OnShowScreen()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0.25f);
	}

	public void OnBack()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
		Navigate("Back");
	}

	protected override void Update()
	{
		base.Update();
		UpdateKeyboard();
	}

	public void OnTermsUp()
	{
		_scrollView.Scroll(1f);
	}

	public void OnTermsDown()
	{
		_scrollView.Scroll(-1f);
	}

	private void UpdateKeyboard()
	{
		if (base._pIsOverlay || !ScreenBase._pIsOverlayShowing)
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				OnTermsDown();
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				OnTermsUp();
			}
			if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
			{
				OnBack();
			}
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
			{
				OnBack();
			}
		}
	}
}
